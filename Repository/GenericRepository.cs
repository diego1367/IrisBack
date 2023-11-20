namespace Repository
{
    using Data;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq.Expressions;
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ContextDb context;
        internal DbSet<TEntity> dbSet;


        protected readonly ContextDb _context;
        public GenericRepository(ContextDb context)
        {
            this._context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
             Expression<Func<TEntity, bool>> filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             string includeProperties = "")
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

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
                    return orderBy(query).ToList();
                }
                else
                {
                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public virtual TEntity FindSingleBy(
         Expression<Func<TEntity, bool>> filter = null)
        {
            TEntity query = null;
            if (filter != null)
            {
                return query = context.Set<TEntity>().Where(filter).SingleOrDefault();
            }
            else
            {
                throw new ArgumentNullException("Predicate value must be passed to FindSingleBy<T>.");
            }
        }

        public virtual IEnumerable<TEntity> GetAllBy(
         Expression<Func<TEntity, bool>> filter = null)
        {
            IEnumerable<TEntity> query = null;
            if (filter != null)
            {
                return query = context.Set<TEntity>().Where(filter);
            }
            else
            {
                throw new ArgumentNullException("Predicate value must be passed to FindSingleBy<T>.");
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
            _context.SaveChanges();

        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Update(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
