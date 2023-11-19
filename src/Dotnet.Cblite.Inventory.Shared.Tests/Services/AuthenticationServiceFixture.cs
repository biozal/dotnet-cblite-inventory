using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.Shared.Messages;
using Dotnet.Cblite.Inventory.Shared.Services;

namespace Dotnet.Cblite.Inventory.Shared.Tests;

public class AuthenticationServiceFixture 
    : IDisposable
{
    public IAuthenticationService? AuthenticationService { get; private set; }
    public IMessenger? Messenger { get; private set; }

    public AuthenticationServiceFixture()
    {
        AuthenticationService = new MockAuthenticationService();
        Messenger = new WeakReferenceMessenger();
    }

    public void Dispose()
    {
        AuthenticationService = null;
        Messenger = null;
    }
}