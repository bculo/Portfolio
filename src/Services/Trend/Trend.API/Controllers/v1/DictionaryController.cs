using Dtos.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Swashbuckle.AspNetCore.Annotations;
using Trend.API.Filters.Models;
using Trend.Application.Interfaces;

namespace Trend.API.Controllers.v1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DictionaryController : ControllerBase
{
    private readonly IDictionaryService _service;

    public DictionaryController(IDictionaryService service)
    {
        _service = service;
    }
    
    
    [HttpGet("GetDefaultAllValue")]
    [SwaggerOperation(OperationId = "GetDefaultAllValue")]
    [OutputCache(PolicyName = "DictionaryPolicy")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDefaultAllValue(CancellationToken token)
    {
        return Ok(await _service.GetDefaultAllValue(token));
    }
    
    [HttpGet("GetSearchEngines")]
    [SwaggerOperation(OperationId = "GetSearchEngines")]
    [OutputCache(PolicyName = "DictionaryPolicy")]
    [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAvailableSearchEngines(CancellationToken token)
    {
        return Ok(await _service.GetSearchEngines(token));
    }

    [HttpGet("GetContextTypes")]
    [SwaggerOperation(OperationId = "GetContextTypes")]
    [OutputCache(PolicyName = "DictionaryPolicy")]
    [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAvailableContextTypes(CancellationToken token)
    {
        return Ok(await _service.GetContextTypes(token));
    }
    
    [HttpGet("GetActiveFilterOptions")]
    [SwaggerOperation(OperationId = "GetActiveFilterOptions")]
    [OutputCache(PolicyName = "DictionaryPolicy")]
    [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetActiveFilterOptions(CancellationToken token)
    {
        return Ok(await _service.GetActiveFilterOptions(token));
    }
    
    [HttpGet("GetSortFilterOptions")]
    [SwaggerOperation(OperationId = "GetSortFilterOptions")]
    [OutputCache(PolicyName = "DictionaryPolicy")]
    [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSortFilterOptions(CancellationToken token)
    {
        return Ok(await _service.GetSortFilterOptions(token));
    }
}