using Couchbase.Lite;
using Couchbase.Lite.Sync;

namespace Dotnet.Cblite.Inventory.Shared.Data;

public class DatabaseManager 
    : IDisposable
{
    private readonly Uri _remoteSyncUrl;
    private readonly string _defaultdatabaseName;
    private readonly string _prebuiltDatabaseName;

    private Replicator _replicator;
    private ListenerToken _replicatorListenerToken;

    private Database _database;

    public void Dispose()
    {
        /*
        _replicator.Dispose();
        _database.Dispose();
        */
    }
}