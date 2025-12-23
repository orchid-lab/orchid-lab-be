using AutoMapper;
using orchid_backend_net.Application.Authentication.Login;
using orchid_backend_net.Application.Authentication.Logout;
using orchid_backend_net.Application.Authentication.Refreshtoken.GenerateRefreshToken;
using orchid_backend_net.Application.Authentication.Refreshtoken.RefreshTokenQuery;
using orchid_backend_net.Application.Authentication.Register;

namespace orchid_backend_net.Application.Tests.Config.AuthenticationConfig;

internal abstract class AuthenticationHandlerTestConfig : BaseHandlerTestConfig
{
    protected LoginQueryHandler LoginHandler = null!;
    protected LogoutCommandHandler LogoutHandler = null!;
    protected RefreshTokenQueryHandler RefreshTokenQueryHandler = null!;
    protected RefreshTokenCommandHandler RefreshTokenCommandHandler = null!;
    protected RegisterCommandHandler RegisterCommandHandler = null!;
    protected IMapper _mapper = null!;
    [SetUp]
    public void Setup()
    {
        LoginHandler = new LoginQueryHandler(
            UserRepositoryMock.Object,
            SenderMock.Object
        );
        LogoutHandler = new LogoutCommandHandler(
            CacheServiceMock.Object
        );
        RefreshTokenQueryHandler = new RefreshTokenQueryHandler(
            UserRepositoryMock.Object,
            CacheServiceMock.Object,
            SenderMock.Object
        );
        RefreshTokenCommandHandler = new RefreshTokenCommandHandler(
            CacheServiceMock.Object
        );
        RegisterCommandHandler = new RegisterCommandHandler(
            UserRepositoryMock.Object,
            CurrentUserServiceMock.Object,
            EmailSenderMock.Object
        );  

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(LoginDTO).Assembly);
        });
        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();
    }
}
