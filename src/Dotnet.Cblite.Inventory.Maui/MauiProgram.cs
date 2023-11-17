using CommunityToolkit.Maui;
using Dotnet.Cblite.Inventory.Maui.ViewModel;
using Dotnet.Cblite.Inventory.Maui.Views;
using Dotnet.Cblite.Inventory.Shared.Services;
using Microsoft.Extensions.Logging;

namespace Dotnet.Cblite.Inventory.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			// Initialize the .NET MAUI Community Toolkit by adding the below line of code
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("FontAwesomeSolid.otf", "AwesomeSolid");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<IAuthenticationService, MockAuthenticationService>();
		
		builder.Services.AddTransient<LoginView>();
		builder.Services.AddTransient<LoginViewModel>();

		return builder.Build();
	}
}
