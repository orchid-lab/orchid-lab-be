using MediatR;
using Moq;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tests.Config;

public abstract class BaseHandlerTestConfig
{
    protected Mock<IUserRepository> UserRepositoryMock = null!;
    protected Mock<ISender> SenderMock = null!;
    protected Mock<IUnitOfWork> UnitOfWorkMock = null!;
    protected Mock<ICacheService> CacheServiceMock = null!;
    protected Mock<ICurrentUserService> CurrentUserServiceMock = null!;
    protected Mock<IEmailSender> EmailSenderMock = null!;
    [SetUp]
    public virtual void BaseSetup()
    {
        UserRepositoryMock = new Mock<IUserRepository>();
        SenderMock = new Mock<ISender>();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
        CacheServiceMock = new Mock<ICacheService>();
        CurrentUserServiceMock = new Mock<ICurrentUserService>();
        EmailSenderMock = new Mock<IEmailSender>();
    }
}
