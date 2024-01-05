using Dtos.Common;
using Dtos.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Swashbuckle.AspNetCore.Annotations;
using Trend.API.Filters.Models;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Dtos;

namespace Trend.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SearchWordController : ControllerBase
    {
        private readonly ISearchWordService _service;

        public SearchWordController(ISearchWordService service)
        {
            _service = service;
        }

        [HttpGet("GetActiveSearchWords")]
        [SwaggerOperation(OperationId = "GetActiveSearchWords")]
        [OutputCache(PolicyName = "SearchWordPolicy")]
        [ProducesResponseType(typeof(List<SearchWordResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetActiveSearchWords(CancellationToken token)
        {
            return Ok(await _service.GetActiveSearchWords(token));
        }
        
        [HttpGet("GetDeactivatedSearchWords")]
        [SwaggerOperation(OperationId = "GetDeactivatedSearchWords")]
        [OutputCache(PolicyName = "SearchWordPolicy")]
        [ProducesResponseType(typeof(List<SearchWordResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDeactivatedSearchWords(CancellationToken token)
        {
            return Ok(await _service.GetDeactivatedSearchWords(token));
        }
        
        [HttpPost("AddNew")]
        [SwaggerOperation(OperationId = "AddNew")]
        [ProducesResponseType(typeof(SearchWordResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNewSearchWord([FromBody] SearchWordAddReqDto request, CancellationToken token)
        {
            return Ok(await _service.AddNewSearchWord(request, token));
        }
        
        [HttpPost("Filter")]
        [SwaggerOperation(OperationId = "Filter")]
        [ProducesResponseType(typeof(SearchWordResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromBody] SearchWordFilterReqDto request, CancellationToken token)
        {
            return Ok(await _service.FilterSearchWords(request, token));
        }
        
        [HttpPost("AttachImage/{searchWordId}")]
        [SwaggerOperation(OperationId = "AttachImage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AttachImageToSearchWord([FromForm] FileUploadDto request, 
            [FromRoute] string searchWordId,
            CancellationToken token)
        {
            var command = await request.File.ToDetailsDto<SearchWordAttachImageReqDto>();
            command.SearchWordId = searchWordId;
            await _service.AttachImageToSearchWord(command, token);
            return NoContent();
        }

        [HttpDelete("Deactivate/{id}")]
        [SwaggerOperation(OperationId = "Deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Deactivate(string id, CancellationToken token)
        {
            await _service.DeactivateSearchWord(id, token);
            return NoContent();
        }
        
        [HttpGet("Activate/{id}")]
        [SwaggerOperation(OperationId = "Activate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Activate(string id, CancellationToken token)
        {
            await _service.ActivateSearchWord(id, token);
            return NoContent();
        }
    }
}
