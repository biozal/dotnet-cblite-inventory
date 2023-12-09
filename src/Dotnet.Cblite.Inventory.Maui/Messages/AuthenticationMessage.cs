using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Dotnet.Cblite.Inventory.Messages;


public enum AuthenticationStatus
{
    Authenticated,
    SignedOut,
    AuthenticationErrorUsernamePassword,
    AuthenticationErrorServiceNotAvailable,
}

public record UserAuthenticationStatus(AuthenticationStatus Status, string Username);

public class AuthenticationMessage(UserAuthenticationStatus status)
    : ValueChangedMessage<UserAuthenticationStatus>(status);