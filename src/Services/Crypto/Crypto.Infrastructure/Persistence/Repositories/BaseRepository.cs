using Crypto.Core.Entities;
using Crypto.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly CryptoDbContext _context;

        public BaseRepository(CryptoDbContext context)
        {
            _context = context;
        }

        public virtual async Task Add(T newInstance)
        {
            _context.Set<T>().Add(newInstance);
        }

        public virtual async Task AddRange(IEnumerable<T> instances)
        {
            _context.Set<T>().AddRange(instances);
        }

        public virtual async Task<long> Count()
        {
            return await _context.Set<T>().LongCountAsync();
        }

        public virtual async Task<List<T>> FetchPage(int page, int take)
        {
            return await _context.Set<T>().OrderByDescending(i => i.CreatedOn)
                .Skip((page - 1) * take)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<List<T>> FetchPage(Expression<Func<T, bool>> predicate, int page, int take)
        {
            return await _context.Set<T>()
                .Where(predicate)
                .OrderByDescending(i => i.CreatedOn)
                .Skip((page - 1) * take)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<List<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                .Where(predicate)
                .OrderByDescending(i => i.CreatedOn)
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<T> FindById(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> FindSingle(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }


        public virtual async Task Remove(T instance)
        {
            _context.Set<T>().Remove(instance);
        }
    }
}
