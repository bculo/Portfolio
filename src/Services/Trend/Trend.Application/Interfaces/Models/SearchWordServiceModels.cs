using Dtos.Common;

namespace Trend.Application.Interfaces.Models;

public record SearchWordAddReqDto
{
    public string SearchWord { get; set; }
    public int SearchEngine { get; set; }
    public int ContextType { get; set; }
}

public record SearchWordAttachImageReqDto : FileDetailsDto
{
    public string SearchWordId { get; set; }
}

public record SearchWordFilterReqDto : PageRequestDto
{
    public int Active { get; set; }
    public int ContextType { get; set; }
    public int SearchEngine { get; set; }
    public string Query { get; set; }
    public int Sort { get; set; }
}

public record SearchWordResDto
{
    public string Id { get; set; }
        
    public bool IsActive { get; set; }
        
    public DateTime Created { get; set; }
    public string SearchWord { get; set; }
    public string SearchEngineName { get; set; }
    public int SearchEngineId { get; set; }
    public string ContextTypeName { get; set; }
    public int ContextTypeId { get; set; }
    public string ImageUrl { get; set; }
}

public class SearchWordSyncDetailResDto
{
    public string WordId { get; set; }
    public int Count { get; set; }
    
    public long TotalCount { get; set; }
}

public record SyncStatusResDto
{
    public string Id { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public int TotalRequests { get; set; }
    public int SucceddedRequests { get; set; }
    public List<SyncStatusWordResDto> SearchWords { get; set; }
}

public record SyncStatusWordResDto
{
    public string ContextTypeName { get; set; }
    public int ContextTypeId { get; set; }
    public string Word { get; set; }
}