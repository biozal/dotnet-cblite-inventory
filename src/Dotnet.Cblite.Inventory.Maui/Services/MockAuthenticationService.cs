using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.Messages;
using Dotnet.Cblite.Inventory.Models;


namespace Dotnet.Cblite.Inventory.Services;


public class MockAuthenticationService
    : IAuthenticationService
{
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

    public void AuthenticateUser(string username, string password, CancellationToken token)
    {
        try
        {
            // Check for cancellation
            token.ThrowIfCancellationRequested();

            CurrentUser = _mockUsers.First(x => x.Username == username && x.Password == password);
            WeakReferenceMessenger.Default.Send(
                new AuthenticationMessage(
                    new UserAuthenticationStatus(AuthenticationStatus.Authenticated, username)
                )
            );
        }
        catch (OperationCanceledException)
        {
            CurrentUser = null;
            Debug.WriteLine($"{DateTime.Now}::Authentication Cancelled for {username}"); 
        }
        catch (Exception)
        {
            CurrentUser = null;
            WeakReferenceMessenger.Default.Send(
                new AuthenticationMessage(
                    new UserAuthenticationStatus(AuthenticationStatus.AuthenticationErrorUsernamePassword, username)
                    )
                );
        }
    }

    public void Logout()
    {
        string? currentUsername = CurrentUser?.Username.Clone() as string;

        CurrentUser = null;
        WeakReferenceMessenger.Default.Send(
            new AuthenticationMessage(
                new UserAuthenticationStatus(AuthenticationStatus.SignedOut, currentUsername ?? "")
                )
            );
    }
}