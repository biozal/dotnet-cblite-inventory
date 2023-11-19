using Dotnet.Cblite.Inventory.Maui.ViewModel;
using Dotnet.Cblite.Inventory.Maui.Views;
using Dotnet.Cblite.Inventory.Shared.Services;

namespace Dotnet.Cblite.Inventory.Maui;

public partial class App : Application
{
	public App(LoginView loginView, AppShell appShell)
	{
		InitializeComponent();
		
		MainPage = loginView;
	}
}
