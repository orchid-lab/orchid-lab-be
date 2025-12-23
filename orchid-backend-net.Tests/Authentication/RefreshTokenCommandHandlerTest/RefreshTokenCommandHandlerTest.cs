using orchid_backend_net.Application.Authentication.Refreshtoken.GenerateRefreshToken;
using orchid_backend_net.Application.Tests.Config.AuthenticationConfig;
using orchid_backend_net.Application.Tests.TestData;

namespace orchid_backend_net.Application.Tests.Authentication.RefreshTokenCommandHandlerTest;

internal class RefreshTokenCommandHandlerTest : AuthenticationHandlerTestConfig
{
    [Test]
    public async Task GenerateRefreshToken_ValidUserID_ReturnsRefreshToken()
    {
        // Arrange
        var user = UserTestData.CreateValidResearcherUser();
        var expiredDateMock = DateTime.UtcNow.AddDays(7);
        TimeSpan expiryDateInRedisMock = expiredDateMock - DateTime.UtcNow;
        CacheServiceMock
            .Setup(x => x.SetAsync(CacheTestData.ExpectedCacheKey, user.ID, expiryDateInRedisMock));

        // Act
        var command = new RefreshTokenCommand(user.ID);
        var result = await RefreshTokenCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Token, Is.Not.Null);
            Assert.That(result.Token, Is.Not.Empty);
            Assert.That(result.Expired, Is.GreaterThan(DateTime.UtcNow));
        });
    }
}
