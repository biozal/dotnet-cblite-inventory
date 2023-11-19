using System.Threading.Channels;
using Dotnet.Cblite.Inventory.Shared.Messages;
using Dotnet.Cblite.Inventory.Shared.Models;

namespace Dotnet.Cblite.Inventory.Shared.Services;

public interface IAuthenticationService
{
    User? CurrentUser { get; set; }
    void AuthenticateUser(string username, string password, CancellationToken token);
    void Logout();
}