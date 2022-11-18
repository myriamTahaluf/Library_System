

using Library_System.Model;
using Library_System.Repositories.Generic.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace Library_System.Repositories.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        #region Fields
        protected readonly LibraryContext context;
        private readonly DbSet<TEntity> dbSet;
        #endregion
        #region CTOR
        public GenericRepository(LibraryContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        #endregion

        #region Methods

        public async Task<IEnumerable<TEntity>> All_async(Expression<Func<TEntity, bool>>? predicate = null)
        {

            IQueryable<TEntity> results = dbSet.AsNoTracking().AsQueryable();

            if (predicate != null)
                results = results.Where(predicate);

            return await results.ToListAsync();
        }

        public IEnumerable<TEntity> All(Expression<Func<TEntity, bool>>? predicate = null)
        {

            IQueryable<TEntity> results = dbSet.AsNoTracking().AsQueryable();

            if (predicate != null)
                results = results.Where(predicate);

            return results.ToList();
        }

        public async Task<IEnumerable<TEntity>> All(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> results = dbSet.AsNoTracking().AsQueryable();

            if (predicate != null)
                results = results.Where(predicate);

            if (includeProperties != null && includeProperties.Count() > 0)
                results = includeProperties.Aggregate(results, (current, includeProperty) => current.Include(includeProperty));

            return await results.ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> All(Expression<Func<TEntity, bool>>? predicate = null, params string[] includeProperties)
        {
            IQueryable<TEntity> results = dbSet.AsNoTracking().AsQueryable();
            if (predicate != null)
                results = results.Where(predicate);
            if (includeProperties != null && includeProperties.Count() > 0)
                results = includeProperties.Aggregate(results, (current, includeProperty) => current.Include(includeProperty));

            return await results.ToListAsync();

        }

        public async Task<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate,
            bool WithTracking = false,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;

            if (!WithTracking)
                query = query.AsNoTracking();
            else
                query = query.AsQueryable();

            if (includeProperties != null && includeProperties.Count() > 0)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            query = query.Where(predicate);


            return await query.FirstOrDefaultAsync();
        }


        public void ClearLocal()
        {
            dbSet.Local.Clear();
        }
        public async Task<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            bool WithTracking = false,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;

            if (!WithTracking)
                query = query.AsNoTracking();
            else
                query = query.AsQueryable();

            query = query.Where(predicate);

            if (includeProperties != null && includeProperties.Count() > 0)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            query = orderBy.Invoke(query);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate,
            string[] includeProperties,
            bool WithTracking = false)

        {
            IQueryable<TEntity> query = dbSet;

            if (!WithTracking)
                query = query.AsNoTracking();
            else
                query = query.AsQueryable();

            if (includeProperties != null && includeProperties.Count() > 0)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }
      

        public TEntity GetById(int id,
     bool withTrack = true,
     params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(withTrack, includeProperties);
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            if (withTrack)
                return query.FirstOrDefault(lambda) ?? query.First();

            return query.AsNoTracking().SingleOrDefaultAsync(lambda).Result ?? query.First();
        }
        public TEntity GetById(int id,
      bool withTrack = true, string[]? includeProperties = null)
        {
            var query = GetAllIncluding(withTrack, includeProperties);
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            if (withTrack)
                return query.SingleOrDefaultAsync(lambda).Result ?? query.Single();

            return query.AsNoTracking().SingleOrDefaultAsync(lambda).Result ?? query.Single();
        }
        public async Task<TEntity> GetByIdAsync(int id,
     bool withTrack = true, string[]? includeProperties = null)
        {
            var query = GetAllIncluding(withTrack, includeProperties);
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            if (withTrack)
                return await query.SingleOrDefaultAsync(lambda) ?? await query.SingleAsync();

            return await query.AsNoTracking().SingleOrDefaultAsync(lambda) ?? await query.SingleAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id,
     bool withTrack = true, string[]? includeProperties = null)
        {
            var query = GetAllIncluding(withTrack, includeProperties);
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            if (withTrack)
                return await query.SingleOrDefaultAsync(lambda) ?? await query.SingleAsync();

            return await query.AsNoTracking().SingleOrDefaultAsync(lambda) ?? await query.SingleAsync();
        }
        public TEntity GetById(Guid id,
     bool withTrack = true, string[]? includeProperties = null)
        {
            var query = GetAllIncluding(withTrack, includeProperties);
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            if (withTrack)
                return query.SingleOrDefaultAsync(lambda).Result ?? query.Single();

            return query.AsNoTracking().SingleOrDefaultAsync(lambda).Result ?? query.Single();
        }

        private IQueryable<TEntity> GetAllIncluding
           (bool withTrack, string[]? includeProperties = null)
        {
            IQueryable<TEntity>? queryable = null;

            if (withTrack)
                queryable = dbSet.AsQueryable();
            else
                queryable = dbSet.AsNoTracking();
            if (includeProperties == null)
            {
                return queryable;
            }
            return includeProperties.Aggregate
                (queryable, (current, includeProperty) => current.Include(includeProperty).AsNoTracking());
        }
        private IQueryable<TEntity> GetAllIncluding
           (bool withTrack, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity>? queryable = null;

            if (withTrack)
                queryable = dbSet.AsQueryable();
            else
                queryable = dbSet.AsNoTracking();
            if (includeProperties == null)
            {
                return queryable;
            }
            return includeProperties.Aggregate
                (queryable, (current, includeProperty) => current.Include(includeProperty));
        }

        public int Max(Expression<Func<TEntity, bool>> predicateWhere, Expression<Func<TEntity, int>> predicateMax)
        {
            var all = context.Set<TEntity>().Where(predicateWhere);
            if (all.Count() == 0)
            {
                return 1;
            }
            else
            {
                return all.Max(predicateMax) + 1;
            }
        }


        public async Task<decimal> MaxValue(Expression<Func<TEntity, bool>> predicateWhere, Expression<Func<TEntity, decimal>> predicateMax)
        {
            var all = context.Set<TEntity>().Where(predicateWhere);

            if (all.Count() == 0)
            {
                return 0;
            }
            else
            {
                return await all.MaxAsync(predicateMax);
            }
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await dbSet.AsQueryable().AnyAsync(predicate);
            return result;
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }
        public void Update(TEntity entity, List<Object>? otherEntities = null)
        {
            var isLocal = context.Set<TEntity>().Local.Contains(entity);

            if (entity.GetType().Namespace != "System.Data.Entity.DynamicProxies" && !isLocal)
                throw new Exception($"Are you trying to update an untrackable entity???? {nameof(entity)}");
            //if (!isLocal)
            //    dbSet.Attach(entity);

            if (otherEntities != null)
            {
                foreach (var e in otherEntities)
                {
                    //isLocal = context.Entry(e).State 
                    //if (!isLocal)
                    //    dbSet.Attach(e);
                    context.Entry(e).State = EntityState.Modified;
                }
            }
            context.Entry(entity).State = EntityState.Modified;
        }
        public void Update(TEntity entity)
        {
            //var isLocal = context.Set<TEntity>().Local.Contains(entity);
            //string _namespace = entity.GetType().Namespace;

            //if (_namespace != "System.Data.Entity.DynamicProxies" && !isLocal)
            //    throw new Exception($"Are you trying to update an untrackable entity???? {nameof(entity)}");

            context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                var isLocal = context.Set<TEntity>().Local.Contains(entity);
                //if (!isLocal)
                //    dbSet.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }
        }
        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = GetBy(predicate).GetAwaiter().GetResult();
            context.Set<TEntity>().Remove(entity);
        }


        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                var isLocal = context.Set<TEntity>().Local.Contains(entity);
                if (!isLocal)
                    dbSet.Attach(entity);
                //  context.Set<TEntity>().Remove(entity);
                //if (entity.GetType().Namespace != "System.Data.Entity.DynamicProxies" && !isLocal)
                //    throw new Exception($"Are you trying to delete an untrackable entity???? in {nameof(entities)}");
            }
            dbSet.RemoveRange(entities);
            //context.Set<TEntity>().RemoveRange(entities);
        }



        public async Task<IList<TEntity>> List(
      Expression<Func<TEntity, bool>>? filter = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
      string includeProperties = "")
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();

            }
        }

        public void Attach(TEntity entity)
        {
            context.Attach(entity);

        }

        public void Attach(List<TEntity> entity)
        {
            context.AttachRange(entity);

        }
        public IQueryable<TEntity> AllAsIQueryable(Expression<Func<TEntity, bool>>? predicate = null, params string[] includeProperties)
        {
            IQueryable<TEntity> results = dbSet.AsNoTracking().AsQueryable();
            if (predicate != null)
                results = results.Where(predicate);
            if (includeProperties != null && includeProperties.Count() > 0)
                results = includeProperties.Aggregate(results, (current, includeProperty) => current.Include(includeProperty));

            return results.AsNoTracking();
        }
        public async Task LoadEntities(Expression<Func<TEntity, bool>>? predicate = null, params string[] includeProperties)
        {
            IQueryable<TEntity> results = dbSet.AsNoTracking().AsQueryable();
            if (predicate != null)
                results = results.Where(predicate);
            if (includeProperties != null && includeProperties.Count() > 0)
                results = includeProperties.Aggregate(results, (current, includeProperty) => current.Include(includeProperty));

            await dbSet.LoadAsync();
        }

        public async Task<IEnumerable<TEntity>> AllWithTraking(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = false)
        {
            IQueryable<TEntity> results = dbSet.AsNoTracking().AsQueryable(); ;
            if (tracking)
                results = dbSet.AsTracking().AsQueryable();




            if (predicate != null)
                results = results.Where(predicate);

            return await results.ToListAsync();
        }

        public async Task<List<TEntity>> FromSqlRaw(string Storedprocedure, SqlParameter[] parameters)
        {
            var listParametersName = string.Join(',', parameters.Select(x => x.ParameterName).ToList());

            var retult = dbSet.FromSqlRaw(string.Format("exec {0} {1}", Storedprocedure, listParametersName), parameters).ToListAsync();

            return await retult;
        }

        public async Task<bool> CanConnectAsync()
        {
            return await context.Database.CanConnectAsync();
        }

        public async Task<bool> ExecuteSqlCommandAsync(string Storedprocedure, params object[] parameters)
        {
            var retult = await context.Database.ExecuteSqlRawAsync(Storedprocedure, parameters);

            return await Task.FromResult((retult > 0));
        }

        public bool ExecuteSqlCommand(string Storedprocedure, params SqlParameter[] parameters)
        {
            var retult = context.Database.ExecuteSqlRaw(Storedprocedure, parameters);

            return (retult > 0);
        }
   
        #endregion

        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy(string orderColumn, string orderType)
        {
            Type typeQueryable = typeof(IQueryable<TEntity>);
            ParameterExpression argQueryable = Expression.Parameter(typeQueryable, "p");
            var outerExpression = Expression.Lambda(argQueryable, argQueryable);
            string[] props = orderColumn.Split('.');
            IQueryable<TEntity> query = new List<TEntity>().AsQueryable<TEntity>();
            Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");

            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo? pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (pi != null)
                {
                    expr = Expression.Property(expr, pi);
                    type = pi.PropertyType;
                }
            }
            LambdaExpression lambda = Expression.Lambda(expr, arg);
            string methodName = orderType == "asc" ? "OrderBy" : "OrderByDescending";

            MethodCallExpression resultExp =
                Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TEntity), type }, outerExpression.Body, Expression.Quote(lambda));
            var finalLambda = Expression.Lambda(resultExp, argQueryable);
            return (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)finalLambda.Compile();
        }

     
    }



}
