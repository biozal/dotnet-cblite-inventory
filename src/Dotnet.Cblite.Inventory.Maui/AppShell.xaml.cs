using System;
using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.Maui.Views;
using Dotnet.Cblite.Inventory.Messages;
using Dotnet.Cblite.Inventory.Services;
using Microsoft.Maui.Controls;

namespace Dotnet.Cblite.Inventory.Maui;

public partial class AppShell : Shell
{
    private readonly IAuthenticationService _authenticationService;

    public AppShell(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;

        InitializeComponent();

        //setup routes
        Routing.RegisterRoute("Projects", typeof(ProjectsView));
        Routing.RegisterRoute("Project", typeof(ProjectView));
        Routing.RegisterRoute("Audits", typeof(AuditsView));
        Routing.RegisterRoute("Audit", typeof(AuditView));
        Routing.RegisterRoute("DeveloperMenu", typeof(DeveloperMenuView));
        Routing.RegisterRoute("DeveloperInfo", typeof(DeveloperInfoView));
        Routing.RegisterRoute("DeveloperLogs", typeof(DeveloperLogsView));
        Routing.RegisterRoute("UserProfile", typeof(UserProfileView));
        Routing.RegisterRoute("Replicator", typeof(ReplicatorView));
    }

    private void Logout_Clicked(object sender, EventArgs e)
    {
        FlyoutIsPresented = false; 
        
        WeakReferenceMessenger.Default.Send(
            new AuthenticationMessage(
                new UserAuthenticationStatus(AuthenticationStatus.SignedOut,
                    _authenticationService.CurrentUser?.Username ?? string.Empty)
            )
        );
    }
}