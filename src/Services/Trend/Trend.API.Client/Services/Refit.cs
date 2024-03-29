// <auto-generated>
//     This code was generated by Refitter.
// </auto-generated>


using Refit;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trend.API.Client.Services
{
    [System.CodeDom.Compiler.GeneratedCode("Refitter", "0.8.5.0")]
    public partial interface INewsApi
    {
        [Headers("Accept: text/plain, application/json, text/json")]
        [Get("/api/v1/News/GetLatestNews")]
        Task<IApiResponse<ICollection<ArticleTypeDto>>> GetLatestNews();

        [Headers("Accept: text/plain, application/json, text/json")]
        [Get("/api/v1/News/GetLatestCryptoNews")]
        Task<IApiResponse<ICollection<ArticleDto>>> GetLatestCryptoNews();

        [Headers("Accept: text/plain, application/json, text/json")]
        [Get("/api/v1/News/GetLatestStockNews")]
        Task<IApiResponse<ICollection<ArticleDto>>> GetLatestStockNews();

        [Headers("Accept: text/plain, application/json, text/json")]
        [Get("/api/v1/News/GetLatestEtfNews")]
        Task<IApiResponse<ICollection<ArticleDto>>> GetLatestEtfNews();

        [Headers("Accept: text/plain, application/json, text/json")]
        [Get("/api/v1/News/GetLatestEconomyNews")]
        Task<IApiResponse<ICollection<ArticleDto>>> GetLatestEconomyNews();
    }

    [System.CodeDom.Compiler.GeneratedCode("Refitter", "0.8.5.0")]
    public partial interface ISearchWordApi
    {
        [Headers("Accept: text/plain, application/json, text/json")]
        [Get("/api/v1/SearchWord/GetSearchWords")]
        Task<IApiResponse<ICollection<SearchWordDto>>> GetSearchWords();

        [Headers("Accept: text/plain, application/json, text/json")]
        [Get("/api/v1/SearchWord/GetAvailableSearchEngines")]
        Task<IApiResponse<ICollection<KeyValueElementDto>>> GetAvailableSearchEngines();

        [Headers("Accept: text/plain, application/json, text/json")]
        [Get("/api/v1/SearchWord/GetAvailableContextTypes")]
        Task<IApiResponse<ICollection<KeyValueElementDto>>> GetAvailableContextTypes();

        [Headers("Accept: text/plain, application/json, text/json")]
        [Post("/api/v1/SearchWord/AddNewSearchWord")]
        Task<IApiResponse<SearchWordDto>> AddNewSearchWord([Body] SearchWordCreateDto body);

        [Headers("Accept: text/plain, application/json, text/json")]
        [Delete("/api/v1/SearchWord/RemoveSearchWord/{id}")]
        Task<IApiResponse<SearchWordDto>> RemoveSearchWord(string id);
    }

    [System.CodeDom.Compiler.GeneratedCode("Refitter", "0.8.5.0")]
    public partial interface ISyncApi
    {
        [Headers("Accept: text/plain, application/json, text/json")]
        [Get("/api/v1/Sync/Sync")]
        Task Sync();

        [Headers("Accept: text/plain, application/json, text/json")]
        [Get("/api/v1/Sync/GetSync/{id}")]
        Task<IApiResponse<SyncStatusDto>> GetSync(string id);

        [Headers("Accept: text/plain, application/json, text/json")]
        [Get("/api/v1/Sync/GetSyncStatuses")]
        Task<IApiResponse<ICollection<SyncStatusDto>>> GetSyncStatuses();

        [Headers("Accept: text/plain, application/json, text/json")]
        [Post("/api/v1/Sync/GetSyncStatusesPage")]
        Task<IApiResponse<SyncStatusDtoPageResponseDto>> GetSyncStatusesPage([Body] PageRequestDto body);

        [Headers("Accept: text/plain, application/json, text/json")]
        [Get("/api/v1/Sync/GetSyncStatusWords/{id}")]
        Task<IApiResponse<ICollection<SyncStatusWordDto>>> GetSyncStatusWords(string id);
    }


}


//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 612 // Disable "CS0612 '...' is obsolete"
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"
#pragma warning disable 8604 // Disable "CS8604 Possible null reference argument for parameter"

namespace Trend.API.Client.Services
{
    using System = global::System;

    

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0))")]
    public partial class ArticleDto
    {

        [JsonPropertyName("title")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Title { get; set; }

        [JsonPropertyName("text")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Text { get; set; }

        [JsonPropertyName("url")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Url { get; set; }

        [JsonPropertyName("pageSource")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string PageSource { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0))")]
    public partial class ArticleTypeDto
    {

        [JsonPropertyName("title")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Title { get; set; }

        [JsonPropertyName("text")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Text { get; set; }

        [JsonPropertyName("url")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Url { get; set; }

        [JsonPropertyName("pageSource")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string PageSource { get; set; }

        [JsonPropertyName("typeName")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string TypeName { get; set; }

        [JsonPropertyName("typeId")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int TypeId { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0))")]
    public partial class ErrorResponseModel
    {

        [JsonPropertyName("message")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Message { get; set; }

        [JsonPropertyName("statusCode")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int StatusCode { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0))")]
    public partial class KeyValueElementDto
    {

        [JsonPropertyName("key")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int Key { get; set; }

        [JsonPropertyName("value")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Value { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0))")]
    public partial class PageRequestDto
    {

        [JsonPropertyName("page")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int Page { get; set; }

        [JsonPropertyName("take")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int Take { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0))")]
    public partial class SearchWordCreateDto
    {

        [JsonPropertyName("searchWord")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string SearchWord { get; set; }

        [JsonPropertyName("searchEngine")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int SearchEngine { get; set; }

        [JsonPropertyName("contextType")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int ContextType { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0))")]
    public partial class SearchWordDto
    {

        [JsonPropertyName("id")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Id { get; set; }

        [JsonPropertyName("created")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public System.DateTimeOffset Created { get; set; }

        [JsonPropertyName("searchWord")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string SearchWord { get; set; }

        [JsonPropertyName("searchEngineName")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string SearchEngineName { get; set; }

        [JsonPropertyName("searchEngineId")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int SearchEngineId { get; set; }

        [JsonPropertyName("contextTypeName")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string ContextTypeName { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0))")]
    public partial class SyncStatusDto
    {

        [JsonPropertyName("id")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Id { get; set; }

        [JsonPropertyName("started")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public System.DateTimeOffset Started { get; set; }

        [JsonPropertyName("finished")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public System.DateTimeOffset Finished { get; set; }

        [JsonPropertyName("totalRequests")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int TotalRequests { get; set; }

        [JsonPropertyName("succeddedRequests")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int SucceddedRequests { get; set; }

        [JsonPropertyName("searchWords")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public ICollection<SyncStatusWordDto> SearchWords { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0))")]
    public partial class SyncStatusDtoPageResponseDto
    {

        [JsonPropertyName("count")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public long Count { get; set; }

        [JsonPropertyName("items")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public ICollection<SyncStatusDto> Items { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0))")]
    public partial class SyncStatusWordDto
    {

        [JsonPropertyName("contextTypeName")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string ContextTypeName { get; set; }

        [JsonPropertyName("contextTypeId")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int ContextTypeId { get; set; }

        [JsonPropertyName("word")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Word { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v10.0.0.0))")]
    public partial class ValidationProblemDetails
    {

        [JsonPropertyName("type")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Type { get; set; }

        [JsonPropertyName("title")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Title { get; set; }

        [JsonPropertyName("status")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int? Status { get; set; }

        [JsonPropertyName("detail")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Detail { get; set; }

        [JsonPropertyName("instance")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Instance { get; set; }

        [JsonPropertyName("errors")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public IDictionary<string, ICollection<string>> Errors { get; set; }

        private IDictionary<string, object> _additionalProperties;

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }


}
