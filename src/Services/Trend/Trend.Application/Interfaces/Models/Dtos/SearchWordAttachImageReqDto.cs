using Microsoft.AspNetCore.Http;

namespace Trend.Application.Interfaces.Models.Dtos;

public record SearchWordAttachImageReqDto(IFormFile File);