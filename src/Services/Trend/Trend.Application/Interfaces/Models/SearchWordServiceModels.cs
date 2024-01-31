using Dtos.Common;

namespace Trend.Application.Interfaces.Models;

public record AddWordReqDto
{
    public string SearchWord { get; set; } = default!;
    public int SearchEngine { get; set; } 
    public int ContextType { get; set; }
}

public record AttachImageToSearchWordReqDto : FileDetailsDto
{
    public string Id { get; set; } = default!;
}

public record DeactivateSearchWordReqDto(string Id) : GetItemByStringIdReqDto(Id);

public record ActivateSearchWordReqDto(string Id) : GetItemByStringIdReqDto(Id);

public record FilterSearchWordsReqDto : PageRequestDto
{
    public int Active { get; set; }
    public int ContextType { get; set; }
    public int SearchEngine { get; set; }
    public string Query { get; set; } = default!;
    public int Sort { get; set; }
}

public record SearchWordResDto
{
    public string Id { get; set; } = default!;
        
    public bool IsActive { get; set; }
        
    public DateTime Created { get; set; }
    public string SearchWord { get; set; } = default!;
    public string SearchEngineName { get; set; } = default!;
    public int SearchEngineId { get; set; }
    public string ContextTypeName { get; set; } = default!;
    public int ContextTypeId { get; set; }
    public string ImageUrl { get; set; } = default!;
}

public record SearchWordSyncStatisticReqDto(string Id) : GetItemByStringIdReqDto(Id);

public record SearchWordSyncStatisticResDto
{
    public string WordId { get; set; } = default!;
    public int Count { get; set; }
    public long TotalCount { get; set; }
}

public record GetSyncStatusReqDto(string Id) : GetItemByStringIdReqDto(Id);

public record SyncStatusResDto
{
    public string Id { get; set; } = default!;
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public int TotalRequests { get; set; }
    public int SucceddedRequests { get; set; }
    public List<SyncSearchWordResDto> SearchWords { get; set; } = default!;
}

public record SyncSearchWordsReqDto(string Id) : GetItemByStringIdReqDto(Id);

public record SyncSearchWordResDto
{
    public string ContextTypeName { get; set; } = default!;
    public int ContextTypeId { get; set; }
    public string Word { get; set; } = default!;
}