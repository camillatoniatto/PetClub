using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Domain.Interfaces.Base
{
    public interface IRepositoryBase<T> where T : class
    {
        Task AddAsync(T entity);
        Task<string> AddReturnIdAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> UpdateAsync(T entity);

        Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null);
        Task<IList<T>> GetByAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null);
        Task<IList<T>> GetByOrderAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool descending, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null);
        Task<T> GetFirstWithOrderTypeAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool descending, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null);

        Task<List<T>> FindPagedAsync<TKey>(int page, int pageSize, Expression<Func<T, TKey>> order, bool descending, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null);
        Task<IList<T>> FindPagedGetByAsync(int page, int pageSize, Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null);
        Task<IList<T>> FindPagedGetByOrderAsync<TKey>(int page, int pageSize, Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool descending, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null);
    }
}
