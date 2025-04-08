using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Entities;
using DomainLayer.Exceptions;
using InfrastructureLayer.Database;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace InfrastructureLayer.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ChildGrowthDbContext _context;
        protected DbSet<T> dbSet;
        public GenericRepository(ChildGrowthDbContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }

        public async Task<int> CountAsync()
        {
            return await dbSet.AsNoTracking().CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task CreateRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
            }
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<T> DeleteAsync(Guid id)
        {
            T _entity = await FindByIdAsync(id);
            if (_entity == null)
            {
                return null;
            }
            dbSet.Remove(_entity);
            await _context.SaveChangesAsync();
            return _entity;
        }

        public virtual async Task DeleteAsync(T _entity)
        {
            dbSet.Remove(_entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<T> FindByIdAsync(Guid id, params string[] navigationProperties)
        {
            var query = ApplyNavigation(navigationProperties);
            T entity = await query.AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id));
            return entity;
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
        {
            var query = dbSet.AsQueryable();

            // Áp dụng Include cho tất cả các navigation properties được truyền vào
            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T> FoundOrThrowAsync(Guid id, string message = "not exist", params string[] navigationProperties)
        {
            var query = ApplyNavigation(navigationProperties);
            T entity = await query.AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id));
            if (entity is null)
            {
                throw new NotFoundException(message);
            }
            return entity;
        }

        public virtual async Task<List<T>> ListAsync(params string[] navigationProperties)
        {
            var query = ApplyNavigation(navigationProperties);
            return await query.AsNoTracking().ToListAsync();
        }

        public virtual async Task<long> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> sumExpression)
        {
            return await dbSet.AsQueryable().AsNoTracking().Where(predicate).SumAsync(sumExpression);
        }

        public virtual async Task<T> UpdateAsync(T updated)
        {
            updated.UpdatedAt = DateTime.UtcNow;
            _context.Attach(updated).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return updated;
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
        {
            List<T> list;
            var query = dbSet.AsQueryable();
            foreach (string navigationProperty in navigationProperties)
                query = query.Include(navigationProperty);//got to reaffect it.

            list = await query.Where(predicate).ToListAsync<T>();
            return list;
        }

        public virtual async Task<List<T>> WhereAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? page = null, int? pageSize = null, params string[] navigationProperties)
        {
            IQueryable<T> query = ApplyNavigation(navigationProperties);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip(page.Value * pageSize.Value).Take(pageSize.Value);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        private IQueryable<T> ApplyNavigation(params string[] navigationProperties)
        {
            var query = dbSet.AsQueryable();
            foreach (string navigationProperty in navigationProperties)
                query = query.Include(navigationProperty);
            return query;
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate = null, params string[] navigationProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }

            return predicate == null
                ? await query.ToListAsync()
                : await query.Where(predicate).ToListAsync();
        }

        public async Task<T> CreateWithoutCreatedAtAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
