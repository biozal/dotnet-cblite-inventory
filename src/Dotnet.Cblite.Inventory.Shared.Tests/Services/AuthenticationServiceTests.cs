using CommunityToolkit.Mvvm.Messaging;
using Dotnet.Cblite.Inventory.Maui.Messages;
using Xunit;

namespace Dotnet.Cblite.Inventory.Shared.Tests.Services;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestPriorityAttribute : Attribute
{
    public TestPriorityAttribute(int priority)
    {
        Priority = priority;
    }

    public int Priority { get; private set; }
}

[TestCaseOrderer("Dotnet.Cblite.Inventory.Shared.Tests.Services.AuthenticationServiceTests",
    "Dotnet.Cblite.Inventory.Shared.Tests")]
public class AuthenticationServiceTests(AuthenticationServiceFixture fixture)
    : IClassFixture<AuthenticationServiceFixture>
{
    private readonly string _username = "demo@example.com";
    private readonly string _password = "P@ssw0rd12";
    private readonly string _wrongPassword = "password";

    [Fact, TestPriority(1)]
    public void AuthenticationPassTest()
    {
        //arrange
        var authService = fixture.AuthenticationService;
        var messenger = fixture.Messenger;
        var token = new CancellationToken();
        UserAuthenticationStatus? status = null;

        Assert.NotNull(authService);
        Assert.NotNull(messenger);

        //act
        messenger.Register<AuthenticationMessage>(this, (r, m) =>
        {
            status = m.Value;
            
            //assert
            Assert.NotNull(status);
            Assert.Equal(AuthenticationStatus.Authenticated, status.Status);
            Assert.Equal(_username, status.Username);
            Assert.NotNull(authService.CurrentUser);
            Assert.Equal(_username, authService.CurrentUser?.Username);
        });
        authService.AuthenticateUser(_username, _password, token);
    }

    [Fact, TestPriority(2)]
    public void AuthenticationFailedTest()
    {
        //arrange
        var authService = fixture.AuthenticationService;
        var messenger = fixture.Messenger;
        var token = new CancellationToken();
        UserAuthenticationStatus? status = null;

        Assert.NotNull(authService);
        Assert.NotNull(messenger);

        //act
        messenger.Register<AuthenticationMessage>(this, (r, m) =>
        {
            status = m.Value;
            //assert
            Assert.NotNull(status);
            Assert.Equal(AuthenticationStatus.AuthenticationErrorUsernamePassword, status.Status);
            Assert.Equal(_username, status.Username);
            Assert.Null(authService.CurrentUser);
        });
        
        authService.AuthenticateUser(_username, _wrongPassword, token);
    }

    [Fact, TestPriority(3)]
    public void AuthenticationLogoutTest()
    {
        //arrange
        var authService = fixture.AuthenticationService;
        var messenger = fixture.Messenger;
        var token = new CancellationToken();
        UserAuthenticationStatus? status = null;

        Assert.NotNull(authService);
        Assert.NotNull(messenger);

        //act 
        authService.AuthenticateUser(_username, _password, token);
        messenger.Register<AuthenticationMessage>(this, (r, m) =>
        {
            status = m.Value;
            
            //assert
            Assert.NotNull(status);
            Assert.Equal(AuthenticationStatus.SignedOut, status.Status);
            Assert.Equal(_username, status.Username);
            Assert.Null(authService.CurrentUser);
        });
        authService.Logout();
    }
}