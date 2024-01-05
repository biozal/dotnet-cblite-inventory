using CommunityToolkit.Maui;
using Dotnet.Cblite.Inventory.Maui.ViewModels;
using Dotnet.Cblite.Inventory.ViewModel;
using Dotnet.Cblite.Inventory.Maui.Views;
using Dotnet.Cblite.Inventory.Services;
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
			.RegisterViewModels()
			.RegisterViews()
			.RegisterServices()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("FontAwesomeSolid.otf", "FontAwesomeSolid");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		
		builder.Services.AddTransient<App>();
		
		return builder.Build();
	}

	private static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddSingleton<IAuthenticationService, MockAuthenticationService>();
		#if IOS 
			mauiAppBuilder.Services.AddSingleton<IDatabaseSeedService, DatabaseSeedServiceMaciOS>();
		#elif ANDROID	
			mauiAppBuilder.Services.AddSingleton<IDatabaseSeedService, DatabaseSeedServiceAndroid>();
		#elif MACCATALYST
			mauiAppBuilder.Services.AddSingleton<IDatabaseSeedService, DatabaseSeedServiceMaciOS>();
		#elif WINDOWS
			mauiAppBuilder.Services.AddSingleton<IDatabaseSeedService, DatabaseSeedServiceWindows>();
		#endif
		return mauiAppBuilder;	
	}

	private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
	{
		//authentication screens
		mauiAppBuilder.Services.AddTransient<LoginView>();
		
		//business screens
		mauiAppBuilder.Services.AddTransient<ProjectsView>();
		mauiAppBuilder.Services.AddTransient<ProjectView>();
		mauiAppBuilder.Services.AddTransient<AuditsView>();
		mauiAppBuilder.Services.AddTransient<AuditView>();
		
		//developer screens	
		mauiAppBuilder.Services.AddTransient<DeveloperMenuView>();
		mauiAppBuilder.Services.AddTransient<DeveloperInfoView>();
		mauiAppBuilder.Services.AddTransient<DeveloperLogsView>();
		mauiAppBuilder.Services.AddTransient<ReplicatorView>();	
		
		return mauiAppBuilder;
	}
	
	private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddTransient<AppShellViewModel>();
		mauiAppBuilder.Services.AddTransient<LoginViewModel>();
		mauiAppBuilder.Services.AddTransient<ProjectsViewModel>();
		mauiAppBuilder.Services.AddTransient<ProjectViewModel>();
		mauiAppBuilder.Services.AddTransient<AuditsViewModel>();
		mauiAppBuilder.Services.AddTransient<AuditViewModel>();
		mauiAppBuilder.Services.AddTransient<DeveloperMenuViewModel>();
		mauiAppBuilder.Services.AddTransient<DeveloperInfoViewModel>();
		mauiAppBuilder.Services.AddTransient<DeveloperLogsViewModel>();
		mauiAppBuilder.Services.AddTransient<ReplicatorViewModel>();

		return mauiAppBuilder;
	}
	
	
}
