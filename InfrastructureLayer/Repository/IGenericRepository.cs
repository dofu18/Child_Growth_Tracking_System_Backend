using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Constants;

namespace InfrastructureLayer.Repository
{
    public interface IGenericRepository<T>
    {
        Task<T> CreateAsync(T entity);
        Task CreateRangeAsync(IEnumerable<T> entities);
        Task<List<T>> ListAsync(params string[] navigationProperties);
        Task<T> FindByIdAsync(Guid id, params string[] navigationProperties);
        Task<T> FoundOrThrowAsync(Guid id, string message = Constants.Errors.NOT_EXIST_ERROR, params string[] navigationProperties);
        Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
        Task<T> UpdateAsync(T updated);
        Task<T> DeleteAsync(Guid id);
        Task DeleteAsync(T _entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties);

        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task<long> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> sumExpression);
        Task<int> CountAsync();
        //add
        Task<List<T>> WhereAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? page = null,
        int? pageSize = null,
        params string[] navigationProperties);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
    }
}
