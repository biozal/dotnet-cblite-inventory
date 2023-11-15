using System.Diagnostics;
using System.Threading.Channels;
using Dotnet.Cblite.Inventory.Shared.Messages;
using Dotnet.Cblite.Inventory.Shared.Models;

namespace Dotnet.Cblite.Inventory.Shared.Services;

public class MockAuthenticationService
    : IAuthenticationService
{
    private readonly Channel<AuthenticationMessage> _channel = Channel.CreateBounded<AuthenticationMessage>(
        new BoundedChannelOptions(1)
        {
            SingleWriter = true,
            SingleReader = false,
            AllowSynchronousContinuations = false,
            FullMode = BoundedChannelFullMode.DropOldest
        });

    public ChannelReader<AuthenticationMessage> ChannelReader => _channel.Reader;

    private readonly IEnumerable<User> _mockUsers = new List<User>
    {
        new User("demo@example.com", "P@ssw0rd12", "team1"),
        new User("demo1@example.com", "P@ssw0rd12", "team1"),
        new User("demo2@example.com", "P@ssw0rd12", "team2"),
        new User("demo3@example.com", "P@ssw0rd12", "team2"),
        new User("demo4@example.com", "P@ssw0rd12", "team3"),
        new User("demo5@example.com", "P@ssw0rd12", "team3"),
        new User("demo6@example.com", "P@ssw0rd12", "team4"),
        new User("demo7@example.com", "P@ssw0rd12", "team4"),
        new User("demo8@example.com", "P@ssw0rd12", "team5"),
        new User("demo9@example.com", "P@ssw0rd12", "team5"),
        new User("demo10@example.com", "P@ssw0rd12", "team6"),
        new User("demo11@example.com", "P@ssw0rd12", "team6"),
        new User("demo12@example.com", "P@ssw0rd12", "team7"),
        new User("demo13@example.com", "P@ssw0rd12", "team8"),
        new User("demo14@example.com", "P@ssw0rd12", "team9"),
        new User("demo15@example.com", "P@ssw0rd12", "team10"),
    };

    public User? CurrentUser { get; set; }

    public async ValueTask AuthenticateUserAsync(string username, string password, CancellationToken token)
    {
        try
        {
            // Check for cancellation
            token.ThrowIfCancellationRequested();

            CurrentUser = _mockUsers.First(x => x.Username == username && x.Password == password);
            await _channel.Writer.WriteAsync(new AuthenticationMessage(AuthenticationStatus.Authenticated, username),
                token);
        }
        catch (OperationCanceledException)
        {
            CurrentUser = null;
            Debug.WriteLine($"{DateTime.Now}::Authentication Cancelled for {username}"); 
        }
        catch (Exception)
        {
            CurrentUser = null;
            await _channel.Writer.WriteAsync(
                new AuthenticationMessage(AuthenticationStatus.AuthenticationErrorUsernamePassword, username), token);
        }
    }

    public async ValueTask Logout()
    {
        string? currentUsername = CurrentUser?.Username.Clone() as string;

        CurrentUser = null;
        await _channel.Writer.WriteAsync(new AuthenticationMessage(AuthenticationStatus.SignedOut,
            currentUsername ?? ""));
    }
}