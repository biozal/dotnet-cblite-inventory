using Dotnet.Cblite.Inventory.Shared.Services;
using Microsoft.Extensions.Hosting;

namespace InventoryUno;

public class App : Application
{
    protected IHost? Host { get; private set; } 
    protected Window? MainWindow { get; private set; }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var appBuilder = this.CreateBuilder(args)
            .Configure(hostBuilder =>
            {
                hostBuilder
                    .ConfigureServices((context, services) =>
                        {
                            // Register your services
                            services.AddSingleton<IAuthenticationService, MockAuthenticationService>();
                        }
                    );
            });
    
        
#if NET6_0_OR_GREATER && WINDOWS && !HAS_UNO
        MainWindow = new Window();
#else
        MainWindow = Microsoft.UI.Xaml.Window.Current;
#endif

#if DEBUG
        MainWindow.EnableHotReload();
#endif


        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (MainWindow.Content is not Frame rootFrame)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();

            // Place the frame in the current Window
            MainWindow.Content = rootFrame;

            rootFrame.NavigationFailed += OnNavigationFailed;
        }

        if (rootFrame.Content == null)
        {
            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            rootFrame.Navigate(typeof(MainPage), args.Arguments);
        }

        // Ensure the current window is active
        MainWindow.Activate();
        Host = appBuilder.Build();
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new InvalidOperationException($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
    }
}