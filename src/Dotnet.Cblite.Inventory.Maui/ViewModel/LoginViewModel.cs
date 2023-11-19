using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dotnet.Cblite.Inventory.Shared.Messages;
using Dotnet.Cblite.Inventory.Shared.Services;

namespace Dotnet.Cblite.Inventory.Maui.ViewModel;

public partial class LoginViewModel(IAuthenticationService authenticationServices) 
    : ObservableObject
{
    [ObservableProperty]
    private string _username;
    
    [ObservableProperty]
    private string _password;
    
    [ObservableProperty]
    private string _errorMessage;

    [RelayCommand]
    private void SetDefaultUsernamePassword()
    {
        Username = "demo@example.com";
        Password = "P@ssw0rd12";
    } 

    [RelayCommand]
    public async Task Authenticate()
    {
        var token = new CancellationToken();
        await authenticationServices.AuthenticateUserAsync(_username, _password, token);
        
        var isComplete = await authenticationServices.ChannelReader.WaitToReadAsync(token);
        if (isComplete)
        {
            var authMessage = await authenticationServices.ChannelReader.ReadAsync(token);
            if (authMessage.Status == AuthenticationStatus.AuthenticationErrorUsernamePassword)
            {
                ErrorMessage = "Error in username or password";
            }
            else if (authMessage.Status == AuthenticationStatus.AuthenticationErrorServiceNotAvailable)
            {
                ErrorMessage = "Authentication Service not available";
            }
            else
            {
                ErrorMessage = string.Empty;
                //typecast App.Current to my app and then swap out the main view to the shell
            }
        }
    }
}