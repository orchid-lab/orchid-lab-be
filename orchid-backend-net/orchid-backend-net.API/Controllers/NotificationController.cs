using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Notification.NotifcationMarkAsRead;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// notification api 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/notification")]
    [ApiController]
    public class NotificationController(ISender sender, ILogger<NotificationController> logger) : BaseController(sender)
    {
        /// <summary>
        /// use this api to mark notification has read
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut("mark-as-read")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> MarkAsRead([FromBody] NotificationMarkAsReadCommand command, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received Put request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
