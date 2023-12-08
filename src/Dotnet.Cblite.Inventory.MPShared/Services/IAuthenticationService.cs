using Dotnet.Cblite.Inventory.MPShared.Models;

namespace Dotnet.Cblite.Inventory.MPShared.Services;

public interface IAuthenticationService
{
    User? CurrentUser { get; set; }
    void AuthenticateUser(string username, string password, CancellationToken token);
    void Logout();
}