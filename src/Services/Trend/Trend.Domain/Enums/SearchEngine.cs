using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Exceptions;

namespace Trend.Domain.Enums
{
    public record SearchEngine(int Value, string DisplayValue) : Enumeration<SearchEngine>(Value, DisplayValue)
    {
        public static readonly SearchEngine Google = new (0, "Google search engine");
        public static readonly SearchEngine All = new (999, "All search engines");
        
        public static implicit operator int(SearchEngine context) => context.Value;
        public static implicit operator SearchEngine(int value) => Create(value);
        
        public static bool IsValidForNewSearchWord(int value)
        {
            return All.Value != value;
        }
    }
}
