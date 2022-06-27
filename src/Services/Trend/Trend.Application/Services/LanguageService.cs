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
    public class LanguageService<T> : ILanguageService<T> where T : class
    {
        private readonly IStringLocalizer<T> _localizer;

        public LanguageService(IStringLocalizer<T> localizer)
        {
            _localizer = localizer;
        }

        public string Get(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return string.Empty;
            }

            var result = _localizer.GetString(identifier.Trim());

            return result?.Value ?? string.Empty;
        }

        public string GetCurrentCulture()
        {
            return CultureInfo.CurrentCulture.DisplayName;
        }
    }
}
