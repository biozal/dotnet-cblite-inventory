using Windows.ApplicationModel;
using Windows.Storage;

namespace Dotnet.Cblite.Inventory.Services;

/// <summary>
/// The implementation of <see cref="IDatabaseSeedService"/> that copies a prebuilt
/// database from the Assets folder
/// </summary>
public sealed class DatabaseSeedService : IDatabaseSeedService
{
    private const string StartingWarehouseFilename = "startingWarehouses.cblite2";

    public async Task CopyDatabaseAsync(string directoryPath)
    {
        var finalPath = Path.Combine(directoryPath, StartingWarehouseFilename);
        Directory.CreateDirectory(finalPath);
        var destFolder = await StorageFolder.GetFolderFromPathAsync(finalPath);
        var assetsFolder = await Package.Current.InstalledLocation.GetFolderAsync($"Assets\\{StartingWarehouseFilename}");
        var filesList = await assetsFolder.GetFilesAsync();
        foreach (var file in filesList)
        {
            await file.CopyAsync(destFolder);
        }
    }

}