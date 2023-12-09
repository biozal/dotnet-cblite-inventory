using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.Maui.Views;
using Dotnet.Cblite.Inventory.Messages;
using Dotnet.Cblite.Inventory.Services;

namespace Dotnet.Cblite.Inventory.Maui;

public partial class App 
	: Application
{
	private readonly LoginView _loginView;
	private readonly IAuthenticationService _authenticationService;
	
	public App(LoginView loginView, IAuthenticationService authenticationService)
	{
		_authenticationService = authenticationService;
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
			MainPage = new AppShell(_authenticationService);
			
		}
		else if (userAuthMessage.Status == AuthenticationStatus.SignedOut)
		{
			MainPage = _loginView;
		}
		
	}
}
