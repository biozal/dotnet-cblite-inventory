using System.Text.Json.Serialization;

namespace Dotnet.Cblite.Inventory.Models;
public readonly record struct OfficeAssignment(
	[property:JsonPropertyName("office")] Office Office, 
	[property:JsonPropertyName("type")] string Type) {}

public readonly record struct Office(
	[property:JsonPropertyName("officeId")] string OfficeId,
	[property:JsonPropertyName("name")] string Name)
{
	public static readonly string TypePrimary = "primary";
	public static readonly string TypeSecondary = "secondary";
}