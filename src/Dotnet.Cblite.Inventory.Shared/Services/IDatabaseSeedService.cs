namespace Dotnet.Cblite.Inventory.Shared.Services;

public interface IDatabaseSeedService
{
    Task CopyDatabaseAsync(string targetDirectoryPath);
}