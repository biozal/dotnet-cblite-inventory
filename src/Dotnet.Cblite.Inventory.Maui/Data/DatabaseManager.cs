using System.Diagnostics;
using Couchbase.Lite;
using Couchbase.Lite.Logging;

namespace Dotnet.Cblite.Inventory.Maui.Data;

public class DatabaseManager 
    : IDisposable
{
    private const string DEFAULT_INVENTORY_DATABASE_NAME = "inventory";
    private const string WAREHOUSE_DATABASE_NAME = "warehouse";
    
    private const string STARTING_WAREHOUSE_DATABASE_NAME = "startingWarehouses";

    private readonly Uri? _remoteSyncUrl;
    
    public Database? InventoryDatabase => null;
    public Database? WarehouseDatabase => null;

    public DatabaseManager()
    {
        //set the default sync URL based on the platform you are using
        //ANDROID emulators require a special network to talk to the host machine
        //where iOS simulators, MacOS, and Winders can use localhost
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            _remoteSyncUrl = new Uri("ws://10.0.2.2:4984");
        }
        else
        {
            _remoteSyncUrl = new Uri("ws://localhost:4984");
        }
        
        //turn on uber logging - in production apps this shouldn't be turn on
        #if DEBUG
            Database.Log.Console.Domains = LogDomain.All;
            Database.Log.Console.Level = LogLevel.Verbose;
        #endif
    }

    public void Dispose()
    {
        CloseDatabases();
        
        //TODO dispose the replicators once created
        /*
        _replicator.Dispose();
        */
    }

    public void CloseDatabases()
    {
        try
        {
            if (InventoryDatabase != null)
            {
                InventoryDatabase.Close();
                InventoryDatabase.Dispose();
            }

            if (WarehouseDatabase != null)
            {
                WarehouseDatabase.Close();
                WarehouseDatabase.Dispose();
            }
        }
        catch (Exception ex)
        {
            //TODO add better logging
            Debug.WriteLine($"{DateTime.Now} ERROR CLOSING DATABASE:: {ex.Message}\n\n STACKTRACE: {ex.StackTrace}"); 
        }
        
    }
}