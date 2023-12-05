using Dotnet.Cblite.Inventory.Shared.Services;

using Windows.ApplicationModel;
using Windows.Storage;

namespace Dotnet.Cblite.Inventory.Maui.Services;

/// <summary>
/// The implementation of <see cref="IDatabaseSeedService"/> that copies a prebuilt
/// database from the Assets folder
/// </summary>
public sealed class WindowsDatabaseSeedService : IDatabaseSeedService
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