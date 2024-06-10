namespace Tracker.API.Endpoints;

public static class TrackerEndpointConfigurations
{
    public const string ApiUrl = "/api";
    
    public static class Portfolio
    {
        public const string Label = "Portfolio endpoints";
        public const string EndpointUrl = ApiUrl + "/portfolio";

        public const string FetchItems = EndpointUrl + "/fetch";
        public const string FetchItem = EndpointUrl + "/fetch/{id:guid}";
        public const string AddItem = EndpointUrl + "/create";
    }
}