using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.StaticData;

namespace DataAccess.GenericRepositoryAndUnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        void Save();
        Task<int> SaveAsync();
        IDisposable BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private bool disposed;
        private IDbContextTransaction transaction;
        private Dictionary<string, object> repositories;
        private DbContext context;

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }
            var type = typeof(T).Name;
            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance =
                    Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
                repositories.Add(type, repositoryInstance);
            }
            return (Repository<T>)repositories[type];
        }

        public virtual void Save()
        {

            context.SaveChanges();
        }

        public async virtual Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }

        public virtual IDisposable BeginTransaction()
        {
            if (transaction != null) throw new Exception(StaticData.TRAN_IN_PROGRESS);
            transaction = context.Database.BeginTransaction();
            return transaction;
        }

        public virtual void CommitTransaction()
        {
            transaction.Commit();
            transaction = null;
        }

        public virtual void RollbackTransaction()
        {
            transaction.Rollback();
            transaction = null;
        }
    }
}
