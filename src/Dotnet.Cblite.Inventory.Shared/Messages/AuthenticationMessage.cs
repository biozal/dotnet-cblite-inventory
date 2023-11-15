namespace Dotnet.Cblite.Inventory.Shared.Messages;

public enum AuthenticationStatus
{
    Authenticated,
    SignedOut,
    AuthenticationErrorUsernamePassword,
    AuthenticationErrorServiceNotAvailable,
}

public readonly record struct AuthenticationMessage(AuthenticationStatus Status, string Username);