using Stock.Core.Models.Base;

namespace Stock.Application.Interfaces.Repositories;

public interface IDataSourceProvider
{
    IQueryable<T> GetQuery<T>() where T : AuditableEntity;
    IQueryable<T> GetReadOnlySourceQuery<T>() where T : ReadOnlySource;
}