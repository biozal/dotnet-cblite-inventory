using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.Shared.Messages;
using Dotnet.Cblite.Inventory.Shared.Services;

namespace Dotnet.Cblite.Inventory.Shared.Tests;

public class AuthenticationServiceFixture 
    : IDisposable
{
    private bool isDisposed;
    
    public IAuthenticationService? AuthenticationService { get; private set; } = new MockAuthenticationService();
    public IMessenger? Messenger { get; private set; } = new WeakReferenceMessenger();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed) return;

        if (disposing)
        {
            AuthenticationService = null;
            Messenger = null;
        }

        isDisposed = true;
    }
}