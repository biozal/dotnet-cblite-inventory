using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.Maui.Messages;
using Dotnet.Cblite.Inventory.Models;

namespace Dotnet.Cblite.Inventory.Maui.Services;

public class MockAuthenticationService
    : IAuthenticationService
{
    private readonly IEnumerable<User> _mockUsers = new List<User>
    {
        new User(
            "jmoore@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Santa Clara",
                        OfficeId = "office::santaclara"
                    },
                    Type = Office.TypePrimary 
                },
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Atlanta",
                        OfficeId = "office::atlanta"
                    },
                    Type = Office.TypeSecondary 
                },
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "New York City",
                        OfficeId = "office::nyc"
                    },
                    Type = Office.TypeSecondary 
                },
            ]),
        new User(
            "jthomas@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Santa Clara",
                        OfficeId = "office::santaclara"
                    },
                    Type = Office.TypePrimary 
                },
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Atlanta",
                        OfficeId = "office::atlanta"
                    },
                    Type = Office.TypeSecondary
                },
            ]),
        new User(
            "msmith@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "New York City",
                        OfficeId = "office::nyc"
                    },
                    Type = Office.TypePrimary 
                },
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Atlanta",
                        OfficeId = "office::atlanta"
                    },
                    Type = Office.TypeSecondary 
                },
            ]),
        new User(
            "pjackson@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Santa Clara",
                        OfficeId = "office::santaclara"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "brodriguez@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Santa Clara",
                        OfficeId = "office::santaclara"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "jrobinson@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Santa Clara",
                        OfficeId = "office::santaclara"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "jjohnsonson@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Santa Clara",
                        OfficeId = "office::santaclara"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "wjones@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Atlanta",
                        OfficeId = "office::atlanta"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "thernandez@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Atlanta",
                        OfficeId = "office::atlanta"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "cmiller@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Atlanta",
                        OfficeId = "office::atlanta"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "sanderson@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Atlanta",
                        OfficeId = "office::atlanta"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "slee@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "Atlanta",
                        OfficeId = "office::atlanta"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "kwhite@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "New York City",
                        OfficeId = "office::nyc"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "jlewis@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "New York City",
                        OfficeId = "office::nyc"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "dbrown@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "New York City",
                        OfficeId = "office::nyc"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "dsanchez@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "New York City",
                        OfficeId = "office::nyc"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
        new User(
            "lgonzalez@example.com",
            "P@ssw0rd12",
            [
                new OfficeAssignment
                {
                    Office = new Office
                    {
                        Name = "New York City",
                        OfficeId = "office::nyc"
                    },
                    Type = Office.TypePrimary 
                },
            ]),
    };

    public User? CurrentUser { get; set; }

    public void AuthenticateUser(string username, string password, CancellationToken token)
    {
        try
        {
            // Check for cancellation
            token.ThrowIfCancellationRequested();

            CurrentUser = _mockUsers.First(x => x.Username == username && x.Password == password);
            WeakReferenceMessenger.Default.Send(
                new AuthenticationMessage(
                    new UserAuthenticationStatus(AuthenticationStatus.Authenticated, username)
                )
            );
        }
        catch (OperationCanceledException)
        {
            CurrentUser = null;
            Debug.WriteLine($"{DateTime.Now}::Authentication Cancelled for {username}");
        }
        catch (Exception)
        {
            CurrentUser = null;
            WeakReferenceMessenger.Default.Send(
                new AuthenticationMessage(
                    new UserAuthenticationStatus(AuthenticationStatus.AuthenticationErrorUsernamePassword, username)
                )
            );
        }
    }

    public void Logout()
    {
        string? currentUsername = CurrentUser?.Username.Clone() as string;

        CurrentUser = null;
        WeakReferenceMessenger.Default.Send(
            new AuthenticationMessage(
                new UserAuthenticationStatus(AuthenticationStatus.SignedOut, currentUsername ?? "")
            )
        );
    }
}