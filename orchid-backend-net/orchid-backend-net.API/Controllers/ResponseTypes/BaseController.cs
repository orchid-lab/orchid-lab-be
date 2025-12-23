using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace orchid_backend_net.API.Controllers.ResponseTypes
{
    /// <summary>
    /// Base controller inherit controller base 
    /// only for injecting ISender from MediatR to all controllers
    /// </summary>
    [Route("api/v{apiVersion:apiVersion}/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// sender is used to send commands and queries to MediatR handlers
        /// </summary>
        protected readonly ISender _sender;
        /// <summary>
        /// Base controller constructor
        /// </summary>
        /// <param name="sender"></param>
        public BaseController(ISender sender)
        {
            this._sender = sender;
        }
    }
}
