using System.IO.Compression;
using Android.Content;

namespace Dotnet.Cblite.Inventory.MPShared.Services;

public class DatabaseSeedService 
    : IDatabaseSeedService
{
    private const string StartingWarehouseFilename = "startingWarehouses.zip";
    private readonly Context _context;

    public DatabaseSeedService(Context context)
    {
        _context = context;
    }

    public async Task CopyDatabaseAsync(string targetDirectoryPath)
    {
        if (_context.Assets != null)
        {
            Directory.CreateDirectory(targetDirectoryPath);
            var assetStream = _context.Assets.Open(StartingWarehouseFilename);

            using (var archive = new ZipArchive(assetStream, ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    var entryPath = Path.Combine(targetDirectoryPath, entry.FullName);

                    if (entryPath.EndsWith("/"))
                    {
                        Directory.CreateDirectory(entryPath);
                    }
                    else
                    {
                        using (var entryStream = entry.Open())
                        using (var writeStream = File.OpenWrite(entryPath))
                        {
                            await entryStream.CopyToAsync(writeStream).ConfigureAwait(false);
                        }
                    }
                }
            }
        }
    }
}