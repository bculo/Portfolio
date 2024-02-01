namespace User.Functions.Models;

public class FailureValidationResponse
{
    public object ValidationDict { get; set; } = default!;
    public int AppCode { get; set; }
}