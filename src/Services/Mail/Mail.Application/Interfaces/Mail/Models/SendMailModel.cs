namespace Mail.Application.Interfaces.Mail.Models;

public record SendMailModel
{
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
}