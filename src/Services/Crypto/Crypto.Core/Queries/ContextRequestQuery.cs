namespace Crypto.Core.Queries
{
    public abstract class ContextRequestQuery
    {
        public string? Query { get; protected set; }
        protected Dictionary<string, dynamic>? Parameters { get; set; }

        public ContextRequestQuery()
        {
            Parameters = new Dictionary<string, dynamic>();
        }

        public abstract void BuildQuery();
    }
}
