using Crypto.Core.Entities;
using Crypto.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastracture.Persistence.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly CryptoDbContext _context;

        public BaseRepository(CryptoDbContext context)
        {
            _context = context;
        }

        public async virtual Task Add(T newInstance)
        {
            _context.Set<T>().Add(newInstance);
        }

        public async virtual Task AddRange(IEnumerable<T> instances)
        {
            _context.Set<T>().AddRange(instances);
        }

        public async virtual Task<long> Count()
        {
            return await _context.Set<T>().LongCountAsync();
        }

        public async virtual Task<List<T>> FetchPage(int page, int take)
        {
            return await _context.Set<T>().OrderByDescending(i => i.CreatedOn)
                .Skip((page - 1) * take)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();
        }

        public async virtual Task<List<T>> FetchPage(Expression<Func<T, bool>> predicate, int page, int take)
        {
            return await _context.Set<T>()
                .Where(predicate)
                .OrderByDescending(i => i.CreatedOn)
                .Skip((page - 1) * take)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();
        }

        public async virtual Task<List<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                .Where(predicate)
                .OrderByDescending(i => i.CreatedOn)
                .AsNoTracking()
                .ToListAsync();
        }

        public async virtual Task<T> FindById(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async virtual Task<T> FindSingle(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async virtual Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }


        public async virtual Task Remove(T instance)
        {
            _context.Set<T>().Remove(instance);
        }
    }
}
