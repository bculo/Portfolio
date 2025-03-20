using Crypto.Core.Entities;

namespace Crypto.Application.Interfaces.Repositories;

public interface IDateSourceProvider
{
    IQueryable<T> GetForEntity<T>() where T : Entity;
}