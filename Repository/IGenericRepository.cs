namespace Repository
{
    using System.Linq.Expressions;
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
             Expression<Func<TEntity, bool>> filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             string includeProperties = "");
        TEntity FindSingleBy(
          Expression<Func<TEntity, bool>> filter = null);
        IEnumerable<TEntity> GetAllBy(Expression<Func<TEntity, bool>> filter = null);
        TEntity GetByID(object id);
        void Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
    }
}
