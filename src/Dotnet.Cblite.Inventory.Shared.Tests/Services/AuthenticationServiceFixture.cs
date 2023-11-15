using Dotnet.Cblite.Inventory.Shared.Services;

namespace Dotnet.Cblite.Inventory.Shared.Tests;

public class AuthenticationServiceFixture 
    : IDisposable
{
    public IAuthenticationService? AuthenticationService { get; private set; }

    public AuthenticationServiceFixture()
    {
        AuthenticationService = new MockAuthenticationService();
    }

    public void Dispose()
    {
        AuthenticationService = null;
    }
}