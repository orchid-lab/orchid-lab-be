using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace orchid_backend_net.API.Controllers.ResponseTypes
{
    [Route("api/v{apiVersion:apiVersion}/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly ISender _sender;
        public BaseController(ISender sender)
        {
            this._sender = sender;
        }
    }
}
