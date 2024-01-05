using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Exceptions;

namespace Trend.Domain.Enums
{
    public record ContextType(int Id, string Name, string ShortName)
    {
        public static ContextType Crypto { get; } = new (0, "Crypto market", "Crypto");
        public static ContextType Stock { get; } = new (1, "Stock market", "Stock");
        public static ContextType Forex { get; } = new (2, "Forex market", "Forex");
        public static ContextType All { get; } = new (-1, "All market", "All");
        
        public static implicit operator int(ContextType context) => context.Id;
        
        public static implicit operator ContextType(int id) => Create(id);
        
        private static ContextType Create(int id) =>
            id switch
            {
                0 => Crypto,
                1 => Stock,
                2 => Forex,
                -1 => All,
                _ => throw new TrendAppCoreException($"Context type is not supported: {id}")
            };

        public override string ToString()
        {
            return Name;
        }
        
        public static bool IsValidContextForSearchWord(int id)
        {
            return GetContextTypes().Where(i => All.Id != id).Any(i => i.Id == id);
        }

        public static bool IsValidContext(int id)
        {
            return GetContextTypes().Any(i => i.Id == id);
        }

        public static IEnumerable<ContextType> GetContextTypes()
        {
            yield return Crypto;
            yield return Stock;
            yield return Forex;
            yield return All;
        }
    }
}
