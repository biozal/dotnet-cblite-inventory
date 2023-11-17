using CommunityToolkit.Mvvm.ComponentModel;
using Dotnet.Cblite.Inventory.Shared.Messages;
using Dotnet.Cblite.Inventory.Shared.Services;

namespace Dotnet.Cblite.Inventory.Maui.ViewModel;

public class LoginViewModel(IAuthenticationService authenticationServices) 
    : ObservableObject
{

    private string _username;
    private string _password;
    private string _errorMessage;

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }
    
    public async ValueTask Authenticate()
    {
        var token = new CancellationToken();
        await authenticationServices.AuthenticateUserAsync(_username, _password, token);
        
        var isComplete = await authenticationServices.ChannelReader.WaitToReadAsync(token);
        if (isComplete)
        {
            var authMessage = await authenticationServices.ChannelReader.ReadAsync(token);
            if (authMessage.Status == AuthenticationStatus.Authenticated)
            {
                ErrorMessage = string.Empty;
            }
            else if (authMessage.Status == AuthenticationStatus.AuthenticationErrorUsernamePassword)
            {
                ErrorMessage = "Error in username or password";
            }
            else
            {
                ErrorMessage = "Authentication Service not available";
            }
        }
    }
}