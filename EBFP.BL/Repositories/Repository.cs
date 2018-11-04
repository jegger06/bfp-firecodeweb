namespace Queries.Core.Repositories
{
    using AutoMapper;
    using EBFP.BL.Helper;
    using EBFP.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    public class Repository<TEntity, T> : IRepository<TEntity, T> where TEntity : class
    {
       

        protected readonly DbContext Context;
        private DbSet<TEntity> dbSet;
        private IQueryable<TEntity> dbSetQueryable;

        public EBFPEntities BFPContext
        {
            get { return Context as EBFPEntities; }
        }

        public Repository(DbContext context)
        {
            Context = context;
            dbSet = Context.Set<TEntity>();
            dbSetQueryable = dbSet;
            Mapper.CreateMap<TEntity, T>().ReverseMap();
            Mapper.CreateMap<List<TEntity>, List<T>>().ReverseMap();
        }

        public TEntity Get(int id)
        {
            return dbSet.Find(id);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> wherePredicate)
        {
            var item = await dbSet.SingleOrDefaultAsync(wherePredicate);
            T ret = Mapper.Map<TEntity, T>(item);
            return ret;
        }

        public T SingleOrDefault(Expression<Func<TEntity, bool>> wherePredicate)
        {
            var item = dbSet.SingleOrDefault(wherePredicate);
            T ret = Mapper.Map<TEntity, T>(item);
            return ret;
        }

        public T SingleOrDefault(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            return SingleOrDefault(null, orderBy);
        }

        public T SingleOrDefault(Expression<Func<TEntity, bool>> wherePredicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {

            if (wherePredicate != null)
                dbSetQueryable = dbSetQueryable.Where(wherePredicate);

            if (orderBy != null)
                dbSetQueryable = orderBy(dbSetQueryable);

            var item = dbSetQueryable.FirstOrDefault();

            T ret = Mapper.Map<TEntity, T>(item);
            return ret;
        }


        public IEnumerable<T> GetAll()
        {
            var ret = new List<T>();
            var items = dbSet.ToList();
            foreach (var item in items)
                ret.Add(Mapper.Map<TEntity, T>(item));

            return ret; 
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }
         
        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Add(T entity, Expression<Func<TEntity, bool>> deletePredicate = null)
        {
            if (deletePredicate != null)
            {
                var items = dbSet.Where(deletePredicate);
                RemoveRange(items);
            }

            var item = Mapper.Map<T, TEntity>(entity);
            dbSet.Add(item);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            var item = Mapper.Map<T, TEntity>(entity);
            dbSet.Attach(item);
            Context.Entry(item).State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void RemoveRange(Expression<Func<TEntity, bool>> deletePredicate)
        {
            var entities = dbSet.Where(deletePredicate);
            dbSet.RemoveRange(entities);
        }

        ////can pass multiple orderBy see below
        ////You can also add desc or descending to order by descending. 
        ////var x = _usersService.GetAll().OrderBy("LastName desc,FirstName desc,UserId")
        //public async Task<List<T>> GetList(Expression<Func<TEntity, bool>> wherePredicate = null, string orderBy = "")
        //{
        //    var ret = new List<T>();
        //    var items = dbSet.Where(wherePredicate).OrderBy(orderBy);
        //    foreach (var item in await items.ToListAsync())
        //        ret.Add(Mapper.Map<TEntity, T>(item));

        //    return ret;
        //}

        //TO DO : IMPROVE ORDER BY, CREATE EXTENSION INSTEAD OF PASSING AS PARAMETER
        public async Task<List<T>> GetListAsync(Expression<Func<TEntity, bool>> wherePredicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            //foreach (Expression<Func<TEntity, object>> include in includes)
            //    query = query.Include(include);

            if (wherePredicate != null)
                dbSetQueryable = dbSetQueryable.Where(wherePredicate);

            if (orderBy != null)
                dbSetQueryable = orderBy(dbSetQueryable);

            var ret = new List<T>();

            foreach (var item in await dbSetQueryable.ToListAsync())
                ret.Add(Mapper.Map<TEntity, T>(item));

            return ret;
        }

        public List<T> GetList(Expression<Func<TEntity, bool>> wherePredicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            //foreach (Expression<Func<TEntity, object>> include in includes)
            //    query = query.Include(include);

            if (wherePredicate != null)
                dbSetQueryable = dbSetQueryable.Where(wherePredicate);

            if (orderBy != null)
                dbSetQueryable = orderBy(dbSetQueryable);

            var ret = new List<T>();

            foreach (var item in dbSetQueryable.ToList())
                ret.Add(Mapper.Map<TEntity, T>(item));

            return ret;
        }

        public virtual void InsertBulk(List<T> model, Expression<Func<TEntity, bool>> deletePredicate = null)
        {
            if (deletePredicate != null)
            {
                var items = dbSet.Where(deletePredicate);
                RemoveRange(items);
            }

            if (model != null)
            {
                foreach (var item in model)
                {
                    TEntity mappedItem = Mapper.DynamicMap<TEntity>(item);
                    Add(mappedItem);
                }
            }
        }

        //public virtual Task<T> GetListResult(GridInfo gridInfo)
        //{
        //    throw new NotImplementedException();
        //}
    }
}