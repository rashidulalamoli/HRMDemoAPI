using DataAccess.GenericRepositoryAndUnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CustomRepository
{
    public interface IRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
    }
    public class RepositoryBase<TEntity> : Repository<TEntity>, IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly DbContext _context;

        public RepositoryBase(DbContext context) : base(context)
        {
            this._context = context;
        }

    }
}
