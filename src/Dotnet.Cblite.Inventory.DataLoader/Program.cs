using System.Collections;
using System.Diagnostics;
using System.Text.Json;
using Couchbase;
using Dotnet.Cblite.Inventory.Models;

const string jsonFileLocation = "../../../../../automation/data-gen/";

//TODO move this to read environmental variables
const string COUCHBASE_USERNAME = "dataloader";
const string COUCHBASE_PASSWORD = @"P@$sw0rd12";
const string COUCHBASE_CONNECTION_STRING = @"couchbase://localhost";
const string COUCHBASE_BUCKET_NAME = "demoErpInventory";

ICluster? cluster = null;
IBucket? bucket = null;

try
{
    (cluster, bucket) = await GetClusterBucket(COUCHBASE_CONNECTION_STRING, COUCHBASE_USERNAME, COUCHBASE_PASSWORD);
    if (bucket != null)
    {
        // ## load data types into Couchbase Server
        await LoadUserProfiles(cluster, bucket);
        await LoadOffices(cluster, bucket);
    }
    else
    {
        Console.WriteLine("ERROR:  Bucket is NULL");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"{ex.Message} - StackTrace: {ex.StackTrace}");
}
finally
{
    //remove cluster from memory and close connection
    if (cluster != null)
    {
        await cluster.DisposeAsync();
    }
}

// ## EOF ##
Console.ReadKey();

async Task LoadData<T>(
    ICluster? cluster,
    IBucket? bucket,
    string filePath,
    string scopeName,
    string collectionName)
{
    string jsonFilePath = Path.Combine(jsonFileLocation, filePath);
    
    if (!File.Exists(jsonFilePath))
    {
        Console.WriteLine($"File {jsonFilePath} does not exist");
    }
    else
    {
        if (bucket is not null)
        {
            string json = File.ReadAllText(jsonFilePath);
            try
            {
                var data = JsonSerializer.Deserialize<List<T>>(json);
                if (data is { Count: > 0 })
                {
                    var scope = await bucket.ScopeAsync(scopeName);
                    var collection = await scope.CollectionAsync(collectionName);

                    var tasks = new List<Task>();

                    foreach (var item in data)
                    {
                        var documentId = (item as IDocumentId)?.DocumentId;
                        if (!string.IsNullOrEmpty(documentId))
                        {
                            tasks.Add(collection.UpsertAsync(documentId, item));
                        }
                    }

                    await Task.WhenAll(tasks);
                    Console.WriteLine($"Data Loaded for scope: {scopeName} collection: {collectionName}");
                }
                else
                {
                    //TODO make file logger and write to that
                    Console.WriteLine($"Issue with loading from json {json}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:  {ex.Message} {ex.StackTrace}");
            }
        }
    }
}

async Task LoadOffices(ICluster? cluster, IBucket? bucket)
{
    string fileLocation = "offices.json";
    string scopeName = "personnel";
    string collectionName = "offices";

    await LoadData<Office>(cluster, bucket, fileLocation, scopeName, collectionName);
}

async Task LoadUserProfiles(ICluster? cluster, IBucket? bucket)
{
    string fileLocation = "userProfiles.json";
    string scopeName = "personnel";
    string collectionName = "userProfiles";
    
    await LoadData<UserProfile>(cluster, bucket, fileLocation, scopeName, collectionName);
}


async Task<(ICluster, IBucket?)> GetClusterBucket(
    string connectionString,
    string username, string
        password)
{
    // Initialize the Couchbase cluster
    var cluster =
        await Cluster.ConnectAsync(connectionString, username, password);

    // Get the bucket
    var bucket = await cluster.BucketAsync(COUCHBASE_BUCKET_NAME);

    return (cluster, bucket);
}