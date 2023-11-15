using System.Threading.Channels;
using Dotnet.Cblite.Inventory.Shared.Messages;
using Dotnet.Cblite.Inventory.Shared.Models;

namespace Dotnet.Cblite.Inventory.Shared.Services;

public interface IAuthenticationService
{
    User? CurrentUser { get; set; }
    public ChannelReader<AuthenticationMessage> ChannelReader { get; }
    ValueTask AuthenticateUserAsync(string username, string password, CancellationToken token);
    ValueTask Logout();
}