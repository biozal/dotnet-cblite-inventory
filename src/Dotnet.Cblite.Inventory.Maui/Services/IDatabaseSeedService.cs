namespace Dotnet.Cblite.Inventory.Maui.Services;

public interface IDatabaseSeedService
{
    Task CopyDatabaseAsync(string targetDirectoryPath);
}