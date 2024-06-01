using Dtos.Common;
using Dtos.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Swashbuckle.AspNetCore.Annotations;
using Trend.API.Extensions;
using Trend.Application.Configurations.Constants;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;

namespace Trend.API.Controllers.v1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SearchWordController(ISearchWordService service) : ControllerBase
{
    [HttpGet("GetSearchWordSyncStatistic/{wordId}", Name = "GetSearchWordSyncStatistic")]
    [OutputCache(PolicyName = "SearchWordPolicy")]
    [ProducesResponseType(typeof(SearchWordSyncStatisticResDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSearchWordSyncStatistic([FromRoute] string wordId, CancellationToken token)
    {
        var request = new SearchWordSyncStatisticReqDto(wordId);
        var result = await service.GetSyncStatistic(request, token);
        return result.ToActionResult();
    }
    
    [HttpGet("GetActiveSearchWords", Name = "GetActiveSearchWords")]
    [OutputCache(PolicyName = "SearchWordPolicy")]
    [ProducesResponseType(typeof(List<SearchWordResDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetActiveSearchWords(CancellationToken token)
    {
        return Ok(await service.GetActiveItems(token));
    }
    
    [HttpGet("GetDeactivatedSearchWords", Name = "GetDeactivatedSearchWords")]
    [OutputCache(PolicyName = "SearchWordPolicy")]
    [ProducesResponseType(typeof(List<SearchWordResDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDeactivatedSearchWords(CancellationToken token)
    {
        return Ok(await service.GetDeactivatedItems(token));
    }
    
    [HttpPost("AddNew", Name = "AddNewSearchWord")]
    [Authorize(Roles = AppRoles.Admin)]
    [ProducesResponseType(typeof(SearchWordResDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddNewSearchWord([FromBody] AddWordReqDto request, CancellationToken token)
    {
        var result = await service.CreateNew(request, token);
        return result.ToActionResult();
    }
    
    [HttpGet("Filter", Name = "FilterSearchWords")]
    [OutputCache(PolicyName = "SearchWordPolicy")]
    [ProducesResponseType(typeof(PageResponseDto<SearchWordResDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromQuery] FilterSearchWordsReqDto request, CancellationToken token)
    {
        var result = await service.Filter(request, token);
        return result.ToActionResult();
    }
    
    [HttpPut("AttachImage/{searchWordId}", Name = "AttachImage")]
    [Authorize(Roles = AppRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AttachImageToSearchWord([FromForm] FileUploadDto request, 
        [FromRoute] string searchWordId,
        CancellationToken token)
    {
        var command = await request.File.ToDetailsDto<AttachImageToSearchWordReqDto>();
        command.Id = searchWordId;
        var result = await service.AttachImage(command, token);
        return result.ToActionResult();
    }

    [HttpPut("Deactivate/{id}", Name = "DeactivateSearchWord")]
    [Authorize(Roles = AppRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deactivate(string id, CancellationToken token)
    {
        var request = new DeactivateSearchWordReqDto(id);
        var result = await service.Deactivate(request, token);
        return result.ToActionResult();
    }
    
    [HttpPut("Activate/{id}", Name = "ActivateSearchWord")]
    [Authorize(Roles = AppRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Activate(string id, CancellationToken token)
    {
        var request = new ActivateSearchWordReqDto(id);
        var result = await service.Activate(request, token);
        return result.ToActionResult();
    }
}

