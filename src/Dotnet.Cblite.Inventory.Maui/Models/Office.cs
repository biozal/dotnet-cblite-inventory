namespace Dotnet.Cblite.Inventory.Maui.Models;
public readonly record struct OfficeAssignment(
	Office Office, 
	string Type) {}

public readonly record struct Office(
	string OfficeId,
	string Name)
{
	public static readonly string TypePrimary = "primary";
	public static readonly string TypeSecondary = "secondary";
}