using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.Shared.Messages;
using Dotnet.Cblite.Inventory.Shared.Services;

namespace Dotnet.Cblite.Inventory.Maui.ViewModel;

public partial class LoginViewModel 
    : ObservableObject
{
    private readonly IAuthenticationService _authenticationService;
    
    [ObservableProperty]
    private string _username;
    
    [ObservableProperty]
    private string _password;
    
    [ObservableProperty]
    private string _errorMessage;

    public LoginViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        
        WeakReferenceMessenger.Default.Register<AuthenticationMessage>(this, (r, m) => { ProcessAuthentication(m); });
    }
    
    [RelayCommand]
    private void SetDefaultUsernamePassword()
    {
        Username = "demo@example.com";
        Password = "P@ssw0rd12";
    } 

    [RelayCommand]
    public void Authenticate()
    {
        var token = new CancellationToken();
        _authenticationService.AuthenticateUser(Username, Password, token);
    }
    
    private void ProcessAuthentication(AuthenticationMessage message)
    {
        var userAuthMessage = message.Value;

        if (userAuthMessage.Status == AuthenticationStatus.AuthenticationErrorUsernamePassword)
        {
            ErrorMessage = "Error in username or password";
        }
        else if (userAuthMessage.Status == AuthenticationStatus.AuthenticationErrorServiceNotAvailable)
        {
            ErrorMessage = "Authentication Service not available";
        }
        else if (userAuthMessage.Status == AuthenticationStatus.Authenticated)
        {
            ErrorMessage = string.Empty;
        }
    }
}