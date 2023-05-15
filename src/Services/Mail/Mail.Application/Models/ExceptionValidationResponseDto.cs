namespace Mail.Application.Models;

public class ExceptionValidationResponseDto : ExceptionResponseDto
{
    public Dictionary<string, string[]> Errors { get; set; }
}