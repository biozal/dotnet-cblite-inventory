
namespace Dotnet.Cblite.Inventory.Models;

public readonly record struct User(
	string Username, 
	string Password, 
	List<OfficeAssignment> OfficeAssignments) { }