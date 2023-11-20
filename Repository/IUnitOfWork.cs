namespace Repository
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class;
        public bool Save();
    }
}
