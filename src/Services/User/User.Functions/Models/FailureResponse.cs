namespace User.Functions.Models
{
    public class FailureResponse
    {
        public object Message { get; set; } = default!;
        public int AppCode { get; set; }
    }
}
