using Dotnet.Cblite.Inventory.Maui.Models;

namespace Dotnet.Cblite.Inventory.Maui.Services;

public interface IAuthenticationService
{
    User? CurrentUser { get; set; }
    void AuthenticateUser(string username, string password, CancellationToken token);
    void Logout();
}