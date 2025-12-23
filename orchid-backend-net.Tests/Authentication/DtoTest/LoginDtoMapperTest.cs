using orchid_backend_net.Application.Authentication.Login;
using orchid_backend_net.Application.Tests.Config.AuthenticationConfig;
using orchid_backend_net.Application.Tests.TestData;

namespace orchid_backend_net.Application.Tests;

internal class LoginDtoMapperTest : AuthenticationHandlerTestConfig
{

    [Test]
    public void MapUser_ValidUserInformation_ReturnTrue()
    {
        var user = UserTestData.CreateValidAdminUser();

        var dto = _mapper.Map<LoginDTO>(user);

        Assert.That(dto.Id, Is.EqualTo("user-1"));
        Assert.That(dto.Name, Is.EqualTo("Test User"));
        Assert.That(dto.Role, Is.EqualTo("Admin"));
    }
}
