using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Service;
using orchid_backend_net.Application.Authentication.Login;
using orchid_backend_net.Application.Authentication.Logout;
using orchid_backend_net.Application.Authentication.Refreshtoken.RefreshTokenQuery;
using orchid_backend_net.Application.Authentication.Register;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// authentication controller
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sender"></param>
    /// <param name="jwtService"></param>
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController(ILogger<AuthenticationController> logger, ISender sender, JwtService jwtService) : BaseController(sender)
    {
        /// <summary>
        /// using for login
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var loginDTO = await this._sender.Send(new LoginQuery(query.Email, query.Password), cancellationToken);
                var token = jwtService.CreateToken(loginDTO.Id, loginDTO.Role, loginDTO.RefreshToken, loginDTO.Name);
                var response = new
                {
                    Message = "Đăng nhập thành công.",
                    token.AccessToken,
                    token.RefreshToken,
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Đăng nhập thất bại.", Detail = ex.Message });
            }
        }

        //using for refresh token
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
                logger.LogInformation("Received POST request for token refresh at {Time}", DateTime.UtcNow);
                var loginDTO = await this._sender.Send(new RefreshTokenQuery(refreshToken), cancellationToken);
                var token = jwtService.CreateToken(loginDTO.Id, loginDTO.Role, loginDTO.RefreshToken, loginDTO.Name);
                var response = new
                {
                    Message = "Refresh token thành công.",
                    token.AccessToken,
                    token.RefreshToken,
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing token refresh at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Refresh token thất bại.", Detail = ex.Message });
            }
        }

        /// <summary>
        /// using for create user in system
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(
           [FromBody] RegisterCommand command, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received POST request for token refresh at {Time}", DateTime.UtcNow);
                var response = await this._sender.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing token refresh at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Refresh token thất bại.", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Using for logout the cache
        /// </summary>
        /// <param name="logoutCommand"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
                logger.LogInformation("Received POST request for logout at {Time}", DateTime.UtcNow);
                var result = await _sender.Send(logoutCommand, cancellationToken);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing logout at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Đăng xuất thất bại.", Detail = ex.Message });
            }
        }
    }
}
