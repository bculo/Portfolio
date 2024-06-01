using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Interfaces;

namespace Trend.Application.Services
{
    public class LanguageService<T>(IStringLocalizer<T> localizer) : ILanguageService<T>
        where T : class
    {
        public string Get(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return string.Empty;
            }

            var result = localizer.GetString(identifier.Trim());

            return result?.Value ?? string.Empty;
        }

        public string GetCurrentCulture()
        {
            return CultureInfo.CurrentCulture.DisplayName;
        }
    }
}
