using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Domain.Interfaces
{
    public interface IRepository<T> where T : IDocument
    {
        void Add(T entity);
    }
}
