using DomainLayer.MainInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.MainServices
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        protected readonly DbContext dbContext;

        public UnitOfWork()
        {
            dbContext = new TContext();
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        #region Disposed

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
