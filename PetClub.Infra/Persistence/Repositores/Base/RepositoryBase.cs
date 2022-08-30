using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PetClub.Domain.Entities.Base;
using PetClub.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PetClub.Infra.Persistence.Repositores.Base
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        protected readonly DbContext _context;

        public RepositoryBase(DbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            T existing = await GetByIdAsync(x => x.Id.Equals(entity.Id));
            if (existing != null) _context.Set<T>().Remove(existing);
        }

        public async Task<IList<T>> GetAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includeProperties != null)
            {
                query = includeProperties(query);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            if (includeProperties != null)
            {
                query = includeProperties(query);
            }
            return await query.FirstOrDefaultAsync();

        }

        public async Task<T> GetFirstWithOrderTypeAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool descending, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
        {
            try
            {
                IList<T> list;
                if (!descending)
                {
                    list = await _context.Set<T>().Where(predicate).OrderBy(order).ToListAsync();
                }
                else
                {
                    list = await _context.Set<T>().Where(predicate).OrderByDescending(order).ToListAsync();
                }


                var last = list.FirstOrDefault();

                return last;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public async Task<string> AddReturnIdAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);


            return entity.Id;

        }

        public async Task<IList<T>> GetByAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            if (includeProperties != null)
            {
                query = includeProperties(query);
            }
            return await query.ToListAsync();
        }

        public async Task<IList<T>> GetByOrderAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool descending, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
        {
            IQueryable<T> query;

            if (!descending)
            {
                query = _context.Set<T>().Where(predicate).OrderBy(order);
                if (includeProperties != null)
                {
                    query = includeProperties(query);
                }
            }
            else
            {
                query = _context.Set<T>().Where(predicate).OrderByDescending(order);
                if (includeProperties != null)
                {
                    query = includeProperties(query);
                }
            }


            return await query.ToListAsync();
        }

        public async Task<List<T>> FindPagedAsync<TKey>(int page, int pageSize, Expression<Func<T, TKey>> order, bool descending, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (descending)
            {
                query.OrderByDescending(order);
            }
            else
            {
                query.OrderBy(order);
            }

            if (includeProperties != null)
            {
                query = includeProperties(query);
            }
            var pag = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();


            return pag;
        }

        public async Task<IList<T>> FindPagedGetByAsync(int page, int pageSize, Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
        {
            var query = await GetByAsync(predicate, includeProperties);

            return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<IList<T>> FindPagedGetByOrderAsync<TKey>(int page, int pageSize, Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool descending, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            if (!descending)
            {
                query = _context.Set<T>().Where(predicate).OrderBy(order);
                if (includeProperties != null)
                {
                    query = includeProperties(query);
                }
            }
            else
            {
                query = _context.Set<T>().Where(predicate).OrderByDescending(order);
                if (includeProperties != null)
                {
                    query = includeProperties(query);
                }
            }

            var pag = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return pag;
        }
    }
}
