using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportManagement.Domain.Models;
using ReportManagement.Domain.Repositorys;
using ReportManagement.Infrastructure.Data;

namespace ReportManagement.Infrastructure.Repositorys
{
    public  class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        private readonly ApplicationDbContext _dbContext;
        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public T GetById(Guid id)
        {
            return _dbContext.Set<T>().Find(id);
        }
        public IEnumerable<T> List()
        {
            return _dbContext.Set<T>().AsEnumerable();
        }
        public IEnumerable<T> List(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
                   .Where(predicate)
                   .AsEnumerable();
        }
        public Guid Insert(T entity)
        {
            EntityEntry<T>  model = _dbContext.Set<T>().Add(entity);//TEntity
            _dbContext.SaveChanges();
            return model.Entity.Id;
        }
        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
