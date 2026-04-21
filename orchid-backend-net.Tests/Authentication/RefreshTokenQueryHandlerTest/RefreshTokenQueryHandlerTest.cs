using FluentAssertions;
using Moq;
using orchid_backend_net.Application.Authentication.Refreshtoken.GenerateRefreshToken;
using orchid_backend_net.Application.Authentication.Refreshtoken.RefreshTokenQuery;
using orchid_backend_net.Application.Tests.Config.AuthenticationConfig;
using orchid_backend_net.Application.Tests.TestData;
using orchid_backend_net.Domain.Entities;
using System.Linq.Expressions;

namespace orchid_backend_net.Application.Tests.Authentication.RefreshTokenQueryHandlerTest;

internal class RefreshTokenQueryHandlerTest : AuthenticationHandlerTestConfig
{
    [Test]
    public async Task RefreshTokenQuery_MissingUserIdInRedisCache_ThrowException()
    {
        //Arrange 
        CacheServiceMock
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(string.Empty);

        var query = new RefreshTokenQuery(CacheTestData.NormalizedToken);

        //Act
        Func<Task> act = async () => { await RefreshTokenQueryHandler.Handle(query, CancellationToken.None); };

        //Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Refresh Token không hợp lệ.");
    }

    [Test]
    public async Task RefreshTokenQuery_UserNotFoundInDatabase_ThrowException()
    {
        //Arrange 
        CacheServiceMock
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync("invalid-user-id");
        UserRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Users?)null);
        CacheServiceMock
            .Setup(x => x.RemoveAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var query = new RefreshTokenQuery(CacheTestData.NormalizedToken);

        //Act
        Func<Task> act = async () => { await RefreshTokenQueryHandler.Handle(query, CancellationToken.None); };

        //Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Người dùng không tồn tại hoặc đã bị vô hiệu hóa. Vui lòng đăng nhập lại.");
        
        // Verify that Redis cleanup was called
        CacheServiceMock.Verify(
            x => x.RemoveAsync(It.IsAny<string>()),
            Times.Once);
    }

    [Test]
    public async Task RefreshTokenQuery_TokenMismatch_ThrowException()
    {
        //Arrange 
        var user = UserTestData.CreateValidResearcherUser();
        user.RefreshToken = "different-token-in-database";
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        CacheServiceMock
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(user.ID);
        UserRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        CacheServiceMock
            .Setup(x => x.RemoveAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var query = new RefreshTokenQuery(CacheTestData.NormalizedToken);

        //Act
        Func<Task> act = async () => { await RefreshTokenQueryHandler.Handle(query, CancellationToken.None); };

        //Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Refresh Token không hợp lệ. Vui lòng đăng nhập lại.");
        
        // Verify that Redis cleanup was called
        CacheServiceMock.Verify(
            x => x.RemoveAsync(It.IsAny<string>()),
            Times.Once);
    }

    [Test]
    public async Task RefreshTokenQuery_RemoveRefreshTokenFromRedisFails_ThrowException()
    {
        //Arrange 
        var user = UserTestData.CreateValidResearcherUser();
        user.RefreshToken = CacheTestData.NormalizedToken; // Match the token
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        CacheServiceMock
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(user.ID);
        UserRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        CacheServiceMock
            .Setup(x => x.RemoveAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        var query = new RefreshTokenQuery(CacheTestData.NormalizedToken);

        //Act
        Func<Task> act = async () => { await RefreshTokenQueryHandler.Handle(query, CancellationToken.None); };

        //Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Có lỗi xảy ra, vui lòng thử lại sau.");
    }

    [Test]
    public async Task RefreshTokenQuery_UserWithUndefinedRole_ThrowException()
    {
        //Arrange 
        var user = UserTestData.CreateInvalidRoleUser();
        user.RefreshToken = CacheTestData.NormalizedToken; // Match the token
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        CacheServiceMock
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(user.ID);
        UserRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        CacheServiceMock
            .Setup(x => x.RemoveAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var query = new RefreshTokenQuery(CacheTestData.NormalizedToken);

        //Act
        Func<Task> act = async () => { await RefreshTokenQueryHandler.Handle(query, CancellationToken.None); };

        //Assert
        await act.Should().ThrowAsync<NotImplementedException>().WithMessage("Tài khoản này chưa có vai trò xác định.");
    }

    [Test]
    public async Task RefreshTokenQuery_AdminUserToken_ReturnLoginDto()
    {
        //Arrange 
        var user = UserTestData.CreateValidAdminUser();
        user.RefreshToken = CacheTestData.NormalizedToken; // Match the token
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        CacheServiceMock
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(user.ID);

        CacheServiceMock
            .Setup(x => x.RemoveAsync(It.IsAny<string>()))
            .ReturnsAsync(true);


        UserRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        UserRepositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(UnitOfWorkMock.Object);

        UnitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        SenderMock
            .Setup(x => x.Send(
                It.IsAny<RefreshTokenCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(CacheTestData.CreateValidRefreshToken());

        var query = new RefreshTokenQuery(CacheTestData.NormalizedToken);

        //Act
        var result = await RefreshTokenQueryHandler.Handle(query, CancellationToken.None);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(user.ID));
            Assert.That(result.Role, Is.EqualTo("Admin"));
            Assert.That(user.RefreshToken, Is.EqualTo(CacheTestData.CreateValidRefreshToken().Token));
            Assert.That(user.RefreshTokenExpiryTime, Is.EqualTo(CacheTestData.CreateValidRefreshToken().Expired));
        });

        CacheServiceMock.Verify(
            x => x.RemoveAsync(It.IsAny<string>()),
            Times.Once);

        SenderMock.Verify(
            x => x.Send(
                It.Is<RefreshTokenCommand>(c => c.UserID == user.ID),
                It.IsAny<CancellationToken>()),
            Times.Once);

        UnitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task RefreshTokenQuery_ResearcherUserToken_ReturnLoginDto()
    {
        //Arrange 
        var user = UserTestData.CreateValidResearcherUser();
        user.RefreshToken = CacheTestData.NormalizedToken; // Match the token
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        CacheServiceMock
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(user.ID);

        CacheServiceMock
            .Setup(x => x.RemoveAsync(It.IsAny<string>()))
            .ReturnsAsync(true);


        UserRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        UserRepositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(UnitOfWorkMock.Object);

        UnitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        SenderMock
            .Setup(x => x.Send(
                It.IsAny<RefreshTokenCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(CacheTestData.CreateValidRefreshToken());

        var query = new RefreshTokenQuery(CacheTestData.NormalizedToken);

        //Act
        var result = await RefreshTokenQueryHandler.Handle(query, CancellationToken.None);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(user.ID));
            Assert.That(result.Role, Is.EqualTo("Researcher"));
            Assert.That(user.RefreshToken, Is.EqualTo(CacheTestData.CreateValidRefreshToken().Token));
            Assert.That(user.RefreshTokenExpiryTime, Is.EqualTo(CacheTestData.CreateValidRefreshToken().Expired));
        });

        CacheServiceMock.Verify(
            x => x.RemoveAsync(It.IsAny<string>()),
            Times.Once);

        SenderMock.Verify(
            x => x.Send(
                It.Is<RefreshTokenCommand>(c => c.UserID == user.ID),
                It.IsAny<CancellationToken>()),
            Times.Once);

        UnitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task ResfreshTokenQuery_TechnicianUserToken_ReturnLoginDto()
    {
        //Arrange 
        var user = UserTestData.CreateValidTechnicianUser();
        user.RefreshToken = CacheTestData.NormalizedToken; // Match the token
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        CacheServiceMock
           .Setup(x => x.GetAsync(It.IsAny<string>()))
           .ReturnsAsync(user.ID);

        CacheServiceMock
            .Setup(x => x.RemoveAsync(It.IsAny<string>()))
            .ReturnsAsync(true);


        UserRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        UserRepositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(UnitOfWorkMock.Object);

        UnitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        SenderMock
            .Setup(x => x.Send(
                It.IsAny<RefreshTokenCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(CacheTestData.CreateValidRefreshToken());

        var query = new RefreshTokenQuery(CacheTestData.NormalizedToken);

        //Act
        var result = await RefreshTokenQueryHandler.Handle(query, CancellationToken.None);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(user.ID));
            Assert.That(result.Role, Is.EqualTo("Technician"));
            Assert.That(user.RefreshToken, Is.EqualTo(CacheTestData.CreateValidRefreshToken().Token));
            Assert.That(user.RefreshTokenExpiryTime, Is.EqualTo(CacheTestData.CreateValidRefreshToken().Expired));
        });

        CacheServiceMock.Verify(
            x => x.RemoveAsync(It.IsAny<string>()),
            Times.Once);

        SenderMock.Verify(
            x => x.Send(
                It.Is<RefreshTokenCommand>(c => c.UserID == user.ID),
                It.IsAny<CancellationToken>()),
            Times.Once);

        UnitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
