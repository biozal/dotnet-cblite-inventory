using Dotnet.Cblite.Inventory.Shared.Services;
using Foundation;

namespace Dotnet.Cblite.Inventory.Maui;

public class IOSDatabaseSeedService 
    : IDatabaseSeedService
{
    private const string StartingWarehouseFilename = "startingWarehouses.cblite2";
    
    public async Task CopyDatabaseAsync(string targetDirectoryPath)
    {
        var finalPath = Path.Combine(targetDirectoryPath, StartingWarehouseFilename);

        Directory.CreateDirectory(finalPath);

        var sourcePath = Path.Combine(NSBundle.MainBundle.ResourcePath, StartingWarehouseFilename);
        var dirInfo = new DirectoryInfo(sourcePath);

        foreach (var file in dirInfo.EnumerateFiles())
        {
            using (var inStream = File.OpenRead(file.FullName))
            using (var outStream = File.OpenWrite(Path.Combine(finalPath, file.Name)))
            {
                await inStream.CopyToAsync(outStream);
            }
        }
    }
}