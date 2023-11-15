using Dotnet.Cblite.Inventory.Shared.Messages;
using Xunit.Sdk;

namespace Dotnet.Cblite.Inventory.Shared.Tests.Services;

public class AuthenticationServiceTests(AuthenticationServiceFixture fixture)
    : IClassFixture<AuthenticationServiceFixture>
{
    private readonly string _username = "demo@example.com";
    private readonly string _password = "P@ssw0rd12";
    private readonly string _wrongPassword = "password";
    
    [Fact]
    public async Task AuthenticationPassTest()
    {
        //arrange
        var authService = fixture.AuthenticationService;
        var reader = authService?.ChannelReader; 
        var token = new CancellationToken();
           
        Assert.NotNull(authService);
        Assert.NotNull(reader);
        
        //act
        await authService.AuthenticateUserAsync(_username, _password, token);
        var isComplete = await reader.WaitToReadAsync(token);
        if (isComplete)
        {
            var authMessage = await reader.ReadAsync(token);

            //assert
            Assert.Equal(AuthenticationStatus.Authenticated, authMessage.Status);
            Assert.Equal(_username, authMessage.Username);
            Assert.NotNull(authService.CurrentUser);
            Assert.Equal(_username, authService.CurrentUser?.Username);
        }
    }
    
    [Fact]
    public async Task AuthenticationFailedTest()
    {
        //arrange
        var authService = fixture.AuthenticationService;
        var reader = authService?.ChannelReader; 
        var token = new CancellationToken();

        Assert.NotNull(authService);
        Assert.NotNull(reader);
        
        //act
        await authService.AuthenticateUserAsync(_username, _wrongPassword, token);
        var isComplete = await reader.WaitToReadAsync(token);
        if (isComplete)
        {
            var authMessage = await reader.ReadAsync(token);

            //assert
            Assert.Equal(AuthenticationStatus.AuthenticationErrorUsernamePassword, authMessage.Status);
            Assert.Equal(_username, authMessage.Username);
            Assert.Null(authService.CurrentUser);
        }    
    }
    
    [Fact]
    public async Task AuthenticationLogoutTest()
    {
        //arrange
        var authService = fixture?.AuthenticationService;
        var reader = authService?.ChannelReader; 
        var token = new CancellationToken();
        
        Assert.NotNull(authService);
        Assert.NotNull(reader);

        //act
        await authService.AuthenticateUserAsync(_username, _password, token);
        var isComplete = await reader.WaitToReadAsync(token);
        if (isComplete)
        {
            await authService.Logout();
            var logoutIsCompleted = await reader.WaitToReadAsync(token);
            if (logoutIsCompleted){
                var authMessage = await reader.ReadAsync(token);
            
                //assert
                Assert.Equal(AuthenticationStatus.SignedOut, authMessage.Status);
                Assert.Equal(_username, authMessage.Username);
                Assert.Null(authService.CurrentUser);
            }
        }
        
    }
}