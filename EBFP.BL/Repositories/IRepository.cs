

namespace Queries.Core.Repositories
{
    using EBFP.BL.Helper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<TEntity, T> where TEntity : class
    {
        TEntity Get(int id);
        Task<T> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> wherePredicate);
        T SingleOrDefault(Expression<Func<TEntity, bool>> wherePredicate);
        T SingleOrDefault(Expression<Func<TEntity, bool>> wherePredicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        IEnumerable<T> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        // This method was not in the videos, but I thought it would be useful to add.
        //TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void Add(T entity, Expression<Func<TEntity, bool>> deletePredicate = null);
        void Update(T entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        void RemoveRange(Expression<Func<TEntity, bool>> deletePredicate);
        //Task<List<T>> GetList(Expression<Func<TEntity, bool>> wherePredicate = null, string orderBy = "");
        Task<List<T>> GetListAsync(Expression<Func<TEntity, bool>> wherePredicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        List<T> GetList(Expression<Func<TEntity, bool>> wherePredicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        void InsertBulk(List<T> model, Expression<Func<TEntity, bool>> deletePredicate = null);
        //TResult GetListResult<TResult>(GridInfo gridInfo);
    }
}