using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.MPShared.Messages;
using Dotnet.Cblite.Inventory.MPShared.Services;

namespace Dotnet.Cblite.Inventory.Shared.Tests;

public class AuthenticationServiceFixture 
    : IDisposable
{
    private bool _isDisposed;
    
    public IAuthenticationService? AuthenticationService { get; private set; } = new MockAuthenticationService();
    public IMessenger? Messenger { get; private set; } = new WeakReferenceMessenger();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing)
        {
            AuthenticationService = null;
            Messenger = null;
        }

        _isDisposed = true;
    }
}