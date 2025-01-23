using Stock.Core.Models.Base;

namespace Stock.Application.Interfaces.Repositories;

public interface IDataSourceProvider
{
    IQueryable<T> GetQuery<T>() where T : class;
}