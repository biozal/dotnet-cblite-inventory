using Dotnet.Cblite.Inventory.Maui.Views;

namespace Dotnet.Cblite.Inventory.Maui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		MainPage = new LoginView();
		
		//MainPage = new AppShell();
	}
}
