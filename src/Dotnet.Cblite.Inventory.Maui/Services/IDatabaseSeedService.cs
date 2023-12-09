namespace Dotnet.Cblite.Inventory.Services;

public interface IDatabaseSeedService
{
    Task CopyDatabaseAsync(string targetDirectoryPath);
}