using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Exceptions;

namespace Trend.Domain.Enums
{
    public record SearchEngine(int Id, string Name)
    {
        public static SearchEngine Google { get; } = new (0, "Google search engine");
        public static SearchEngine All { get; } = new (999, "All search engines");
        
        public static implicit operator int(SearchEngine context) => context.Id;
        
        public static implicit operator SearchEngine(int id) => Create(id);
        
        private static SearchEngine Create(int id) =>
            id switch
            {
                0 => Google,
                999 => All,
                _ => throw new TrendAppCoreException($"Search engine is not supported: {id}")
            };

        public override string ToString()
        {
            return Name;
        }
        
        public static bool IsValidSearchEngineForSearchWord(int id)
        {
            return GetSearchEngines().Where(i => All.Id != id).Any(i => i.Id == id);
        }

        public static bool IsValidContext(int id)
        {
            return GetSearchEngines().Any(i => i.Id == id);
        }

        public static IEnumerable<SearchEngine> GetSearchEngines()
        {
            yield return Google;
            yield return All;
        }
    }
}
