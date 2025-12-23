using FluentAssertions;
using Moq;
using orchid_backend_net.Application.Authentication.Login;
using orchid_backend_net.Application.Authentication.Refreshtoken.GenerateRefreshToken;
using orchid_backend_net.Application.Tests.Config.AuthenticationConfig;
using orchid_backend_net.Application.Tests.TestData;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Entities.Base;
using System.Linq.Expressions;

namespace orchid_backend_net.Application.Tests.Authentication.LoginQueryHandlerTest;

[TestFixture]
internal class LoginQueryHandlerTest : AuthenticationHandlerTestConfig
{
    [Test]
    public async Task FindUser_GivenCorrectEmailAndPassword_ReturnAdminLoginDTO()
    {
        //Arrange
        UserRepositoryMock
             .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(UserTestData.CreateValidAdminUser());

        UserRepositoryMock
            .Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        UserRepositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(UnitOfWorkMock.Object);

        UnitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var refreshTokenResult = new RefreshToken
        {
            Token = "sample-refresh-token",
            Expired = DateTime.UtcNow.AddDays(7)
        };

        SenderMock
            .Setup(x => x.Send(
                It.IsAny<RefreshTokenCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(refreshTokenResult);

        //Act
        var query = new LoginQuery("test@gmail.com", "hashed-password");

        var result = await LoginHandler.Handle(query, CancellationToken.None);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo("user-1"));
            Assert.That(result.Role, Is.EqualTo("Admin"));
            Assert.That(result.RefreshToken, Is.EqualTo("sample-refresh-token"));
            Assert.That(result.Name, Is.EqualTo("Test User"));
        });

        UserRepositoryMock.Verify(
            x => x.VerifyPassword("hashed-password", "hashed-password"),
            Times.Once);

        UnitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task FindUser_GivenCorrectEmailAndPassword_ReturnResearcherLoginDTO()
    {
        //Arrange
        UserRepositoryMock
             .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(UserTestData.CreateValidResearcherUser());

        UserRepositoryMock
            .Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        UserRepositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(UnitOfWorkMock.Object);

        UnitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var refreshTokenResult = new RefreshToken
        {
            Token = "sample-refresh-token",
            Expired = DateTime.UtcNow.AddDays(7)
        };

        SenderMock
            .Setup(x => x.Send(
                It.IsAny<RefreshTokenCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(refreshTokenResult);

        //Act
        var query = new LoginQuery("test@gmail.com", "hashed-password");

        var result = await LoginHandler.Handle(query, CancellationToken.None);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo("user-2"));
            Assert.That(result.Role, Is.EqualTo("Researcher"));
            Assert.That(result.RefreshToken, Is.EqualTo("sample-refresh-token"));
            Assert.That(result.Name, Is.EqualTo("Test User"));
        });

        UserRepositoryMock.Verify(
            x => x.VerifyPassword("hashed-password", "hashed-password"),
            Times.Once);

        UnitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task FindUser_GivenCorrectEmailAndPassword_ReturnTechnicianLoginDTO()
    {
        //Arrange
        UserRepositoryMock
             .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(UserTestData.CreateValidTechnicianUser());

        UserRepositoryMock
            .Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        UserRepositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(UnitOfWorkMock.Object);

        UnitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var refreshTokenResult = new RefreshToken
        {
            Token = "sample-refresh-token",
            Expired = DateTime.UtcNow.AddDays(7)
        };

        SenderMock
            .Setup(x => x.Send(
                It.IsAny<RefreshTokenCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(refreshTokenResult);

        //Act
        var query = new LoginQuery("test@gmail.com", "hashed-password");

        var result = await LoginHandler.Handle(query, CancellationToken.None);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo("user-3"));
            Assert.That(result.Role, Is.EqualTo("Technician"));
            Assert.That(result.RefreshToken, Is.EqualTo("sample-refresh-token"));
            Assert.That(result.Name, Is.EqualTo("Test User"));
        });

        UserRepositoryMock.Verify(
            x => x.VerifyPassword("hashed-password", "hashed-password"),
            Times.Once);

        UnitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Test]
    public async Task FindUser_GivenCorrectEmailAndPasswordButInvalidRole_ThrowException()
    {
        //Arrange
        UserRepositoryMock
             .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(UserTestData.CreateInvalidRoleUser());

        UserRepositoryMock
            .Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var refreshTokenResult = new RefreshToken
        {
            Token = "sample-refresh-token",
            Expired = DateTime.UtcNow.AddDays(7)
        };

        SenderMock
            .Setup(x => x.Send(
                It.IsAny<RefreshTokenCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(refreshTokenResult);

        //Act
        var query = new LoginQuery("test@gmail.com", "hashed-password");

        Func<Task> act = async () => { await LoginHandler.Handle(query, CancellationToken.None); };

        //Assert
        await act.Should().ThrowAsync<NotImplementedException>().WithMessage("Tài khoản này chưa có vai trò xác định.");
    }

    [Test]
    public async Task FindUser_GivenWrongEmailAndWrongPassword_ThrowException()
    {
        //Arrange
        UserRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Users?)null);

        //Act
        var query = new LoginQuery("a@gmail.comm", "123");
        Func<Task> act = async () => { await LoginHandler.Handle(query, CancellationToken.None); };

        //Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Không tìm thấy người dùng.");
    }

    [Test]
    public async Task FindUser_GivenCorrectEmailAndWrongPassWord_ThrowException()
    {
        //Arrange 
        UserRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(UserTestData.CreateValidAdminUser());

        UserRepositoryMock
           .Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
           .Returns(false);

        //Act
        var query = new LoginQuery("a@gmail.com", "123");
        Func<Task> act = async () => { await LoginHandler.Handle(query, CancellationToken.None); };

        //Assert
        await act.Should().ThrowAsync<IncorrectPasswordException>().WithMessage("Sai mật khẩu.");
    }

    [Test]
    public async Task FindUser_GivenUserHasBeenDeleted_ThrowException()
    {
        //Arrange
        UserRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Users, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(UserTestData.CreateDeletedTechnicianUser);

        //Act
        var query = new LoginQuery("a@gmail.com", "123");
        Func<Task> act = async () => { await LoginHandler.Handle(query, CancellationToken.None); };

        //Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Tài khoản đã bị vô hiệu hóa.");
    }
}