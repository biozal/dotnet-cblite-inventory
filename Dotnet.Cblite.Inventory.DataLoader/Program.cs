using System.Diagnostics;
using System.Text.Json;
using Couchbase;
using Dotnet.Cblite.Inventory.Models;

const string jsonFileLocation = "../../../../automation/data-gen/";

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

async Task LoadUserProfiles(ICluster? cluster, IBucket? bucket)
{
    const string jsonFileUserProfilesLocation = "userProfiles.json";

    string userProfilePath = Path.Combine(jsonFileLocation, jsonFileUserProfilesLocation);

    if (!File.Exists(userProfilePath))
    {
        Console.WriteLine($"File {userProfilePath} does not exist");
    }
    else
    {
        if (bucket is not null)
        {
            string json = File.ReadAllText(userProfilePath);
            try
            {
                var userProfiles = JsonSerializer.Deserialize<List<UserProfile>>(json);

                if (userProfiles is { Count: > 0 })
                {
                    var scope = await bucket.ScopeAsync("personnel");
                    var collection = await scope.CollectionAsync("userProfiles");
                    
                    var tasks = new List<Task>();
                    
                    foreach (var userProfile in userProfiles)
                    {
                        tasks.Add(collection.UpsertAsync(userProfile.UserProfileId, userProfile));
                    }
                    
                    await Task.WhenAll(tasks);
                    Console.WriteLine("User Profiles loaded");
                }
                else
                {
                    //TODO make file logger and write to that
                    Debug.WriteLine($"Issue with loading from json {json}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR:  {ex.Message} {ex.StackTrace}");
            }
        }
    }
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