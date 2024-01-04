using Dtos.Common;
using Microsoft.AspNetCore.Http;

namespace Trend.Application.Interfaces.Models.Dtos;

public record SearchWordAttachImageReqDto : FileDetailsDto
{
    public string SearchWordId { get; set; }
}