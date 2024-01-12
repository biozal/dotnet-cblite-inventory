using System.Text.Json.Serialization;

namespace Dotnet.Cblite.Inventory.Models;

public readonly record  struct UserProfile(
	[property:JsonPropertyName("userProfileId")] string UserProfileId,
	[property:JsonPropertyName("firstName")] string FirstName,
	[property:JsonPropertyName("lastName")] string LastName,
	[property:JsonPropertyName("email")] string Email, 
	[property:JsonPropertyName("jobTitle")] string JobTitle,
	[property:JsonPropertyName("department")] string Department,
	[property:JsonPropertyName("offices")] List<OfficeAssignment> Offices) { }
