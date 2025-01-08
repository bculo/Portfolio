namespace Crypto.API.Controllers;

public static class EndpointsConfigurations
{
    private const string ApiUrl = "api";
    
    public static class InfoEndpoints
    {
        private const string Controller = ApiUrl + "/info";

        public const string AssemblyInfo = Controller + "/assembly-info";
    }

    public static class CryptoEndpoints
    {
        private const string Controller = ApiUrl + "/crypto";
        
        public const string Create = Controller + "/create";
        public const string CreateWithDelay = Controller + "/create-with-delay";
        public const string UndoDelayCreate = Controller + "/undo-create-with-delay";
        public const string UpdateInfo = Controller + "/update-info";
        public const string UpdatePrice = Controller + "/update-price";
        public const string UpdatePriceAll = Controller + "/update-price-all";
        public const string Page = Controller + "/page";
        public const string Single = Controller + "/single/{symbol}";
        public const string History = Controller + "/price-history/{cryptoId}";
        public const string Popular = Controller + "/popular";
        
        public static string BuildSingleUrl(string symbol) => Path.Combine(Controller, "single", symbol);
        public static string BuildHistoryUrl(Guid cryptoId) => Path.Combine("single", cryptoId.ToString());
    }
}