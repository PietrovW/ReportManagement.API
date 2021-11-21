using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReportManagement.Domain.Repositorys;
using ReportManagement.Infrastructure.Data;

namespace ReportManagement.Infrastructure.Repositorys
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        //public BaseRepository(ApplicationDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}
        public T GetById(int id)
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
        public void Insert(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
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
