using Dtos.Common;
using Dtos.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Swashbuckle.AspNetCore.Annotations;
using Trend.API.Extensions;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;

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

        [HttpGet("GetSearchWordSyncStatistic/{wordId}")]
        [SwaggerOperation(OperationId = "GetSearchWordSyncStatistic")]
        [OutputCache(PolicyName = "SearchWordPolicy")]
        [ProducesResponseType(typeof(SearchWordSyncStatisticResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSearchWordSyncStatistic([FromRoute] string wordId, CancellationToken token)
        {
            var request = new SearchWordSyncStatisticReqDto(wordId);
            var result = await _service.GetSyncStatistic(request, token);
            return result.ToActionResult();
        }
        
        [HttpGet("GetActiveSearchWords")]
        [SwaggerOperation(OperationId = "GetActiveSearchWords")]
        [OutputCache(PolicyName = "SearchWordPolicy")]
        [ProducesResponseType(typeof(List<SearchWordResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetActiveSearchWords(CancellationToken token)
        {
            return Ok(await _service.GetActiveItems(token));
        }
        
        [HttpGet("GetDeactivatedSearchWords")]
        [SwaggerOperation(OperationId = "GetDeactivatedSearchWords")]
        [OutputCache(PolicyName = "SearchWordPolicy")]
        [ProducesResponseType(typeof(List<SearchWordResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDeactivatedSearchWords(CancellationToken token)
        {
            return Ok(await _service.GetDeactivatedItems(token));
        }
        
        [HttpPost("AddNew")]
        [SwaggerOperation(OperationId = "AddNew")]
        [ProducesResponseType(typeof(SearchWordResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewSearchWord([FromBody] AddWordReqDto request, CancellationToken token)
        {
            var result = await _service.CreateNew(request, token);
            return result.ToActionResult();
        }
        
        [HttpPost("Filter")]
        [SwaggerOperation(OperationId = "Filter")]
        [OutputCache(PolicyName = "WordPostPolicy")]
        [ProducesResponseType(typeof(PageResponseDto<SearchWordResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromBody] FilterSearchWordsReqDto request, CancellationToken token)
        {
            var result = await _service.Filter(request, token);
            return result.ToActionResult();
        }
        
        [HttpPut("AttachImage/{searchWordId}")]
        [SwaggerOperation(OperationId = "AttachImage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AttachImageToSearchWord([FromForm] FileUploadDto request, 
            [FromRoute] string searchWordId,
            CancellationToken token)
        {
            var command = await request.File.ToDetailsDto<AttachImageToSearchWordReqDto>();
            command.Id = searchWordId;
            var result = await _service.AttachImage(command, token);
            return result.ToActionResult();
        }

        [HttpPut("Deactivate/{id}")]
        [SwaggerOperation(OperationId = "Deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Deactivate(string id, CancellationToken token)
        {
            var request = new DeactivateSearchWordReqDto(id);
            var result = await _service.Deactivate(request, token);
            return result.ToActionResult();
        }
        
        [HttpPut("Activate/{id}")]
        [SwaggerOperation(OperationId = "Activate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Activate(string id, CancellationToken token)
        {
            var request = new ActivateSearchWordReqDto(id);
            var result = await _service.Activate(request, token);
            return result.ToActionResult();
        }
    }
}
