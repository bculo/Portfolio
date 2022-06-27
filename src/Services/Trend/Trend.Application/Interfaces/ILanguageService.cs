using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces
{
    public interface ILanguageService<T> where T : class
    {
        string Get(string identifier);
        string GetCurrentCulture();
    }
}
