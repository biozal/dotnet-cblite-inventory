using Dotnet.Cblite.Inventory.ViewModel;

namespace Dotnet.Cblite.Inventory.Maui.Views;

public partial class LoginView
{
    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}