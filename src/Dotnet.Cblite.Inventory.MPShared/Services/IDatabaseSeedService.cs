namespace Dotnet.Cblite.Inventory.MPShared.Services;

public interface IDatabaseSeedService
{
    Task CopyDatabaseAsync(string targetDirectoryPath);
}