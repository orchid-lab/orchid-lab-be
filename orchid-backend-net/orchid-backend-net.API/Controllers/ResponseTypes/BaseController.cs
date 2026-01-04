using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace orchid_backend_net.API.Controllers.ResponseTypes
{
    /// <summary>
    /// Base controller inherit controller base 
    /// only for injecting ISender from MediatR to all controllers
    /// </summary>
    /// <remarks>
    /// Base controller constructor
    /// </remarks>
    /// <param name="sender"></param>
    [Route("api/v{apiVersion:apiVersion}/[controller]")]
    [ApiController]
    public class BaseController(ISender sender) : ControllerBase
    {
        /// <summary>
        /// sender is used to send commands and queries to MediatR handlers
        /// </summary>
        protected readonly ISender Sender = sender;
    }
}
