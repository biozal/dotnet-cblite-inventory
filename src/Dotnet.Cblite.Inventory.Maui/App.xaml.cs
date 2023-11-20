using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.Maui.Views;
using Dotnet.Cblite.Inventory.Shared.Messages;

namespace Dotnet.Cblite.Inventory.Maui;

public partial class App 
	: Application
{
	private readonly AppShell _appShell;
	private readonly LoginView _loginView;
	
	public App(LoginView loginView, AppShell appShell)
	{
		_appShell = appShell;
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
			MainPage = _appShell;
		}
		else if (userAuthMessage.Status == AuthenticationStatus.SignedOut)
		{
			MainPage = _loginView;
			
			//TODO send message to database to close out of everything, including the replicator
		}
		
	}
}
