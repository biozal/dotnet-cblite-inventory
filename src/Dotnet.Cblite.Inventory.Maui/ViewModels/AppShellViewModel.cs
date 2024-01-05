using CommunityToolkit.Mvvm.ComponentModel;
using Dotnet.Cblite.Inventory.Models;
using Dotnet.Cblite.Inventory.Services;

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
        
        ProfileImageName = "phprofile.png";
        
        CurrentUser = _authenticationService.CurrentUser;
        if (CurrentUser is { } user)
        {
            //TODO: get user from repository
            FullName = "Not Set";
            EmailAddress = user.Username;
        }
        else
        {
            FullName = string.Empty;
            EmailAddress = string.Empty;
        }
    }
}