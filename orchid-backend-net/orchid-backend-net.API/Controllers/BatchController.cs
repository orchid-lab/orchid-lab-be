using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.Batch;
using orchid_backend_net.Application.Batch.Dto.Batch;
using orchid_backend_net.Application.Batch.UseCase.CreateBatch;
using orchid_backend_net.Application.Batch.UseCase.DeleteBatch;
using orchid_backend_net.Application.Batch.UseCase.GetAllBatch;
using orchid_backend_net.Application.Batch.UseCase.GetBatchById;
using orchid_backend_net.Application.Batch.UseCase.UpdateBatch;
using orchid_backend_net.Application.Batch.UseCase.UpdateBatchStatus;
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
        /// get all batches with pagination and optional search terms
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="BatchNameSearchTerm"></param>
        /// <param name="LabNameSearchTerm"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<BatchDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNo,
            [FromQuery] int pageSize,
            [FromQuery] string? BatchNameSearchTerm,
            [FromQuery] string? LabNameSearchTerm,
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

        /// <summary>
        /// admin use this api to create batch
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> CreateBatch([FromBody] CreateBatchCommand command, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = result }, new JsonResponse<string>("Tạo batch thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// admin use this api to delete batch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> DeleteBatch([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new DeleteBatchCommand(id), cancellationToken);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// admin use this api to update batch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateBatchDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateBatch([FromRoute] int id, [FromBody] UpdateBatchInformationDto updateBatchDto, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(
                    new UpdateBatchCommand(
                        id,
                        updateBatchDto.LabRoomId,
                        updateBatchDto.BatchName,
                        updateBatchDto.BatchSizeWidth,
                        updateBatchDto.BatchSizeHeight,
                        updateBatchDto.WidthUnit,
                        updateBatchDto.HeightUnit), cancellationToken);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// use this api as admin to update batch status
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li><c>Cleaning</c> - when a batch is completed batching in experiment log</li>
        /// <li><c>Ready</c> - when a batch is completed in cleaning </li>
        /// <li><c>Maintenance</c> - when a batch is needed to be maintain</li>
        /// </ul>
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="updateBatchStatus"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateBatchStatus([FromRoute] int id, [FromBody] UpdateBatchStatus updateBatchStatus, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(
                    new UpdateBatchStatusCommand(
                        id,
                        Status: updateBatchStatus.Status), cancellationToken);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
