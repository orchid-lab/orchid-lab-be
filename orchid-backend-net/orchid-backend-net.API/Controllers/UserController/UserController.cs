using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Service;
using orchid_backend_net.Application.Authentication.Login;
using orchid_backend_net.Application.Authentication.Logout;
using orchid_backend_net.Application.Authentication.Refreshtoken.RefreshTokenQuery;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.User;
using orchid_backend_net.Application.User.CreateUser;
using orchid_backend_net.Application.User.DeleteUser;
using orchid_backend_net.Application.User.GetAllUser;
using orchid_backend_net.Application.User.GetUserInfor;
using orchid_backend_net.Application.User.UpdateUser;
using orchid_backend_net.Application.User.UpdateUserAvatar;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers.UserController
{
    [Route("api/user")]
    [ApiController]
    public class UserController(ISender _sender, ILogger<UserController> _logger, JwtService jwtService) : BaseController(_sender)
    {
        //[Authorize(Roles = "Admin,Researcher")]
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<UserDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<PageResult<UserDTO>>>> GetAllUser(
            [FromQuery] GetAllUserQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(await this._sender.Send(query, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<UserDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<UserDTO>>> GetId(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(await this._sender.Send(new GetUserInforQuery(id), cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        [HttpPost("login")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(
            [FromBody] LoginQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var loginDTO = await this._sender.Send(new LoginQuery(query.Email, query.Password), cancellationToken);
                var token = jwtService.CreateToken(loginDTO.Id, loginDTO.RoleID, loginDTO.RefreshToken, loginDTO.Name);
                token.UserDTO = await _sender.Send(new GetUserInforQuery(loginDTO.Id), cancellationToken);
                var response = new
                {
                    Message = "Login successfully",
                    token.AccessToken,
                    token.RefreshToken,
                    User = token.UserDTO
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Đăng nhập thất bại", Detail = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken(
            [FromBody] string refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Received POST request for token refresh at {Time}", DateTime.UtcNow);
                var loginDTO = await this._sender.Send(new RefreshTokenQuery(refreshToken), cancellationToken);
                var token = jwtService.CreateToken(loginDTO.Id, loginDTO.RoleID, loginDTO.RefreshToken, loginDTO.Name);
                token.UserDTO = await _sender.Send(new GetUserInforQuery(loginDTO.Id), cancellationToken);
                var response = new
                {
                    Message = "Login successfully",
                    token.AccessToken,
                    token.RefreshToken,
                    User = token.UserDTO
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing token refresh at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Refresh token thất bại", Detail = ex.Message });
            }
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var result = await this._sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Tạo tài khoản thất bại", Detail = ex.Message });
            }
        }

        [HttpDelete]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Delete(
            [FromBody] DeleteUserCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var result = await this._sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing DELETE request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi xóa với lỗi sau: " + ex.Message);
            }
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateInformation(
            [FromBody] UpdateUserInformationCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var result = await this._sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "User update failed", Detail = ex.Message });
            }
        }

        [HttpPut("images")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateAvatar(
            IFormFile image,
            [FromForm] string userId,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                if(string.IsNullOrEmpty(userId))
                    return BadRequest("User ID is required.");
                if (image == null || image.Length == 0)
                    return BadRequest("Image file is required.");
                using var stream = image.OpenReadStream();
                stream.Position = 0;
                var command = new UpdateUserAvatarCommand(userId, image.FileName, stream);
                var result = await this._sender.Send(command, cancellationToken);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "User update failed", Detail = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> LogOut([FromBody] LogoutCommand logoutCommand, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Received POST request for logout at {Time}", DateTime.UtcNow);
                var result = await _sender.Send(logoutCommand, cancellationToken);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing logout at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Logout failed", Detail = ex.Message });
            }
        }
    }
}
