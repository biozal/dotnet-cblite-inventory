using LoginViewModel = Dotnet.Cblite.Inventory.Maui.ViewModels.LoginViewModel;

namespace Dotnet.Cblite.Inventory.Maui.Views;

public partial class LoginView
{
    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}