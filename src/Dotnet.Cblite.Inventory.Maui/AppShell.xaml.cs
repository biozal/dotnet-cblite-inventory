using Dotnet.Cblite.Inventory.Maui.Views;
using Dotnet.Cblite.Inventory.Shared.Services;

namespace Dotnet.Cblite.Inventory.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		//setup routes
		Routing.RegisterRoute("projects", typeof(ProjectsView));
		Routing.RegisterRoute("project", typeof(ProjectView));
		Routing.RegisterRoute("audits", typeof(AuditsView));
		Routing.RegisterRoute("audit", typeof(AuditView));
		Routing.RegisterRoute("userProfile", typeof(UserProfileView));
		Routing.RegisterRoute("developerMenu", typeof(DeveloperMenuView));
	}
}
