namespace Dotnet.Cblite.Inventory.Maui.Models;

public readonly record struct User(
	string Username, 
	string Password, 
	List<OfficeAssignment> OfficeAssignments) { }
