namespace Dotnet.Cblite.Inventory.Maui.Models;

public readonly record  struct UserProfile(
	string UserProfileId,
	string FirstName,
	string LastName,
	string Email, 
	string JobTitle,
	string Department,
	List<OfficeAssignment> Offices) { }
