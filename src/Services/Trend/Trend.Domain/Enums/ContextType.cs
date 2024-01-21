using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Exceptions;

namespace Trend.Domain.Enums
{
    public record ContextType(int Value, string DisplayValue) : Enumeration<ContextType>(Value, DisplayValue)
    {
        public static readonly ContextType Crypto = new (0, "Crypto market");
        public static readonly ContextType Stock = new (1, "Stock market");
        public static readonly ContextType Forex = new (2, "Forex market");
        public static readonly ContextType All = new (999, "All market");
        
        public static implicit operator int(ContextType context) => context.Value;
        
        public static implicit operator ContextType(int value) => Create(value);
        
        public static bool IsValidForNewSearchWord(int value)
        {
            return All.Value != value;
        }
        
        public static bool IsValidForNewSearchWord(ContextType type)
        {
            return All != type;
        }
    }
}
