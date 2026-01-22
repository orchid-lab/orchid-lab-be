using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Notification.Dto;
using orchid_backend_net.Application.Notification.UseCase.GetAllNotification;
using orchid_backend_net.Application.Notification.UseCase.NotifcationMarkAsRead;
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
        /// use this api to mark notification as read
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut("{id}/mark-as-read")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> MarkAsRead([FromRoute] string id, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received Put request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new NotificationMarkAsReadCommand(id), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// get all notification for a user
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<NotificationDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PageResult<NotificationDto>>> GetNotifications(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] string userId,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received Get request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetAllNotificationQuery(pageNumber, pageSize, userId), cancellationToken);
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
