using FluentAssertions;
using Moq;
using orchid_backend_net.Application.Authentication.Logout;
using orchid_backend_net.Application.Tests.Config.AuthenticationConfig;
using orchid_backend_net.Application.Tests.TestData;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Tests.Authentication.LogoutCommandHandlerTest;

internal class LogoutCommandHandlerTest : AuthenticationHandlerTestConfig
{
    [Test]
    public async Task RemoveAsync_ExistedCache_ReturnTrue()
    {
        //Arrange
        CacheServiceMock
            .Setup(x => x.RemoveAsync(CacheTestData.ExpectedCacheKey))
            .ReturnsAsync(true);

        var command = new LogoutCommand(CacheTestData.NormalizedToken);

        //Act 
        var result = await LogoutHandler.Handle(command, CancellationToken.None);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("Đăng xuất thành công."));

        CacheServiceMock.Verify(
            x => x.RemoveAsync(CacheTestData.ExpectedCacheKey),
            Times.Once);
    }

    [Test]
    public async Task RemoveAsync_NoExistedCacheKey_ReturnFalse()
    {
        //Arrange
        CacheServiceMock
            .Setup(x => x.RemoveAsync(CacheTestData.ExpectedCacheKey))
            .ReturnsAsync(false);

        var command = new LogoutCommand(CacheTestData.NormalizedToken); 
        
        //Act
        var result = await LogoutHandler.Handle(command, CancellationToken.None);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("Đăng xuất thất bại."));

        CacheServiceMock.Verify(
            x => x.RemoveAsync(CacheTestData.ExpectedCacheKey),
            Times.Once);
    }

    [Test]
    public async Task RemoveAsync_ArgumentExceptionOccrs_ThrowException()
    {
        //Arrange 
        CacheServiceMock
            .Setup(x => x.RemoveAsync(CacheTestData.ExpectedCacheKey))
            .ThrowsAsync(new ArgumentException("Có lỗi xảy ra."));
        var command = new LogoutCommand(CacheTestData.NormalizedToken);

        //Act 
        Func<Task> act = async () => { await LogoutHandler.Handle(command, CancellationToken.None); };

        //
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Có lỗi xảy ra.");
    }
}
