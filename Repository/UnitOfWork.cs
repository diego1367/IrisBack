namespace Repository
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContextDb context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        private bool disposed = false;

        public UnitOfWork(DbContextOptions<ContextDb> options, string connectionString = null)
        {
            this.context = new ContextDb(options, connectionString);
        }

        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IGenericRepository<T>;
            }

            IGenericRepository<T> repo = new GenericRepository<T>(context);
            repositories.Add(typeof(T), repo);
            return repo;
        }

        public bool Save()
        {
            bool returnValue = true;
            using (var dbContextTransaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    this.context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    returnValue = false;
                    dbContextTransaction.Rollback();
                }
            }
            return returnValue;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.context.Dispose();
            }
            this.disposed = true;
        }
    }
}
