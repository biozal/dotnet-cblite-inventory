using System.Text.Json.Serialization;

namespace Dotnet.Cblite.Inventory.Models;
public readonly record struct OfficeAssignment(
	[property:JsonPropertyName("office")] UserOffice Office, 
	[property:JsonPropertyName("type")] string Type) {}

public readonly record struct UserOffice(
	[property:JsonPropertyName("officeId")] string DocumentId,
	[property:JsonPropertyName("name")] string Name) : IDocumentId
{
	public static readonly string TypePrimary = "primary";
	public static readonly string TypeSecondary = "secondary";
}

public readonly record struct Geo(
	[property:JsonPropertyName("lat")] double Latitude,
	[property:JsonPropertyName("long")] double Longitude) {}

public readonly record struct Office(
	[property:JsonPropertyName("officeId")] string DocumentId,
	[property:JsonPropertyName("name")] string Name,
	[property:JsonPropertyName("address1")] string Address1,
	[property:JsonPropertyName("address2")] string Address2,
	[property:JsonPropertyName("city")] string City,
	[property:JsonPropertyName("state")] string State,
	[property:JsonPropertyName("zip")] string Zip,
	[property:JsonPropertyName("geo")] Geo location) : IDocumentId
{
}