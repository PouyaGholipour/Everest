using DomainLayer.MainInterfaces;
using DomainLayer.ServiceResults;
using InfrastructureLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.MainServices
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly EverestDataBaseContext _dbContext;
        public IUnitOfWork _unitOfWork { get; }
        private DbSet<TEntity> dbSet
        {
            get
            {
                return _dbContext.Set<TEntity>();
            }
        }

        public Repository(EverestDataBaseContext dbContext,IUnitOfWork unitOfWork)
        {
            this._dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public List<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault();
        }

        public TEntity GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Create(TEntity entity)
        {
            dbSet.Add(entity);
            _unitOfWork.Commit();
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("موجودیت مورد نظر یافت نشد");
            dbSet.Update(entity);
            _unitOfWork.Commit();
        }

        public void DeleteById(int id)
        {
            var entity = GetById(id);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("موجودیت مورد نظر یافت نشد");
            dbSet.Remove(entity);
            _unitOfWork.Commit();
        }

        public void RemoveRange(List<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
            _unitOfWork.Commit();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IQueryable<TEntity>> GetAllAsyncQuery()
        {
            return dbSet.AsQueryable();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where)
        {
            return await dbSet.Where(where).FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task CreateAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if(entity == null)
                throw new ArgumentNullException("موجودیت مورد نظر یافت نشد");
            dbSet.Update(entity);

            await _unitOfWork.CommitAsync();
        }

        #region Dispose
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
