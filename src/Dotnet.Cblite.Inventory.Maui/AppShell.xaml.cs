﻿using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.Maui.Messages;
using Dotnet.Cblite.Inventory.Maui.ViewModels;
using Dotnet.Cblite.Inventory.Maui.Views;

namespace Dotnet.Cblite.Inventory.Maui;

public partial class AppShell : Shell
{
    private readonly AppShellViewModel _viewModel;
    public string FullName => _viewModel.FullName;
    public string EmailAddress => _viewModel.EmailAddress;
    public ImageSource ProfileImageName => _viewModel.ProfileImageName; 
    
    public AppShell(
        AppShellViewModel viewModel)
    {
        _viewModel = viewModel;
        
        BindingContext = this;
        
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
                    _viewModel.CurrentUser?.Username ?? string.Empty)
            )
        );
    }
}