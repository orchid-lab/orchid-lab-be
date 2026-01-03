using Moq;
using orchid_backend_net.Application.Authentication.Register;
using orchid_backend_net.Application.Tests.Config.AuthenticationConfig;
using orchid_backend_net.Application.Tests.TestData;
using orchid_backend_net.Domain.Entities;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Tests.Authentication.RegisterCommandHandlerTest;

internal class RegisterCommandHandlerTest : AuthenticationHandlerTestConfig
{

    [Test]
    public async Task Test1()
    {
        //Arrange
        var user = UserTestData.CreateValidResearcherUser();

        UserRepositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(UnitOfWorkMock.Object);

        UnitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        EmailSenderMock
            .Setup(x => x.SendEmailAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ))
            .Returns(Task.CompletedTask);

        var command = new RegisterCommand(
            user.Name,
            user.Email,
            user.PhoneNumber,
            user.RoleID
        );

        //Act
        var result = await RegisterCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Does.StartWith($"Tạo tài khoản thành công với id:"));
        EmailSenderMock.Verify(x =>
            x.SendEmailAsync(
                It.Is<string>(email => email == user.Email),
                It.Is<string>(subject => subject.Contains("OrchidLab")),
                It.Is<string>(body =>
                    body.Contains(user.Name) &&
                    body.Contains(user.Email)
                )
            ),
            Times.Once
        );
        UnitOfWorkMock.Verify(x =>
            x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
        UserRepositoryMock.Verify(x =>
        x.Add(It.IsAny<Users>()),
        Times.Once);
    }
}
