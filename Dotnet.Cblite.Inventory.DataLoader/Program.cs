using System.Diagnostics;
using System.Text.Json;
using Dotnet.Cblite.Inventory.Models;

const string jsonFileLocation = "../../../../automation/data-gen/";

// ## load data types into Couchbase Server
LoadUserProfiles();
                                             
// ## EOF ##
Console.ReadKey();

void LoadUserProfiles()
{
    const string jsonFileUserProfilesLocation = "userProfiles.json";
    string userProfilePath = Path.Combine(jsonFileLocation, jsonFileUserProfilesLocation);

    if (!File.Exists(userProfilePath))
    {
        Console.WriteLine($"File {userProfilePath} does not exist");
    }
    else
    {
        string json = File.ReadAllText(userProfilePath);
        try
        {
            var userProfiles = JsonSerializer.Deserialize<List<UserProfile>>(json);

            if (userProfiles is { Count: > 0 })
            {
                foreach (var userProfile in userProfiles)
                {
                    //TODO set x-attributes and save to the database
                    Debug.WriteLine($"{userProfile.FirstName} {userProfile.LastName} {userProfile.Email}");
                }
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