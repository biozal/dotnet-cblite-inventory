using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.Maui.Messages;
using Dotnet.Cblite.Inventory.Models;
using Dotnet.Cblite.Inventory.Maui.Services;

namespace Dotnet.Cblite.Inventory.Maui.ViewModels;

public partial class AppShellViewModel
    : ObservableObject
{
    private readonly IAuthenticationService _authenticationService;

    [ObservableProperty] 
    private ImageSource _profileImageName;
    
    [ObservableProperty]
    private string _emailAddress;

    [ObservableProperty]
    private string _fullName;
    
    [ObservableProperty]
    private User? _currentUser;
    
    public AppShellViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        
        //set default data
        ProfileImageName = "phprofile.png";
        FullName = "Not Set";
        EmailAddress = "Not Set";
        
        WeakReferenceMessenger.Default.Register<AuthenticationMessage>(this, (r, m) => { ProcessAuthentication(m); });
    }
    
    private void ProcessAuthentication(AuthenticationMessage message)
    {
        var userAuthMessage = message.Value;
        if (userAuthMessage.Status == AuthenticationStatus.Authenticated && _authenticationService.CurrentUser is { } user)
        {
            EmailAddress = user.Username;
            //TODO:  get user profile info from the database
        }
    }
}