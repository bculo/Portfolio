namespace Stock.Core.Exceptions
{
    public class StockCoreValidationException(Dictionary<string, string[]> errors)
        : StockCoreException("Validation.Error", "One or more validation errors occurred.")
    {
        public Dictionary<string, string[]> Errors { get; set; } = errors;
    }
}
