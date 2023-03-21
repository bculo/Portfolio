using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Interfaces.Services
{
    public interface ICacheService
    {
        Task<T> Get<T>(string identifier) where T : class;
        Task<string> Get(string identifier);
        Task<List<T>> GetList<T>(string identifier) where T : class;
        Task Add(string identifier, object instance, bool setExpirationTime = true);
    }
}
