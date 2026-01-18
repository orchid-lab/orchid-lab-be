using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Batch.Dto.Batch;
using orchid_backend_net.Application.Batch.UseCase.GetAllBatch;
using orchid_backend_net.Application.Batch.UseCase.GetBatchById;
using orchid_backend_net.Application.Common.Pagination;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// batch api controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/batches")]
    [ApiController]
    public class BatchController(ISender sender, ILogger<BatchController> logger) : BaseController(sender)
    {
        /// <summary>
        /// get all batch 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<BatchDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery]int pageNo, 
            [FromQuery]int pageSize,
            [FromQuery]string? BatchNameSearchTerm,
            [FromQuery]string? LabNameSearchTerm,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetAllBatchQuery(pageNo, pageSize, BatchNameSearchTerm, LabNameSearchTerm), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// get batch by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BatchDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetBatchByIdQuery(id), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
