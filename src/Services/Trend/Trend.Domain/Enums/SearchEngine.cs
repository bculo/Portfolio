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
        public static SearchEngine All { get; } = new (-1, "All search engines");
        
        public static implicit operator int(SearchEngine context) => context.Id;
        
        public static implicit operator SearchEngine(int id) => Create(id);
        
        private static SearchEngine Create(int id) =>
            id switch
            {
                0 => Google,
                -1 => All,
                _ => throw new TrendAppCoreException($"Search engine is not supported: {id}")
            };

        public override string ToString()
        {
            return Name;
        }
        
        public static bool IsValidSearchEngineForSearchWord(int id)
        {
            return GetContextTypes().Where(i => All.Id != id).Any(i => i.Id == id);
        }

        public static bool IsValidContext(int id)
        {
            return GetContextTypes().Any(i => i.Id == id);
        }

        public static IEnumerable<SearchEngine> GetContextTypes()
        {
            yield return Google;
            yield return All;
        }
    }
}
