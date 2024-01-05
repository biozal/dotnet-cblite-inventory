using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.Maui.ViewModels;
using Dotnet.Cblite.Inventory.Maui.Views;
using Dotnet.Cblite.Inventory.Messages;
using Dotnet.Cblite.Inventory.Services;

namespace Dotnet.Cblite.Inventory.Maui;

public partial class App 
	: Application
{
	private readonly LoginView _loginView;
	private readonly AppShellViewModel _appShellViewModel;
	
	public App(
		AppShellViewModel appShellViewModel,
		LoginView loginView)
	{
		_appShellViewModel = appShellViewModel;
		_loginView = loginView;		
		
		InitializeComponent();
		
	 WeakReferenceMessenger.Default	.Register<AuthenticationMessage>(this, (r, m) =>
		{
			ProcessAuthentication(m);
		});
		MainPage = _loginView;
	}

	private void ProcessAuthentication(AuthenticationMessage message)
	{
		var userAuthMessage = message.Value;
		if (userAuthMessage.Status == AuthenticationStatus.Authenticated)
		{
			MainPage = new AppShell(_appShellViewModel);
			
		}
		else if (userAuthMessage.Status == AuthenticationStatus.SignedOut)
		{
			MainPage = _loginView;
		}
		
	}
}
