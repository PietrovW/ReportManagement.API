
using ReportManagement.Domain.Models;
using System.Linq.Expressions;

namespace ReportManagement.Domain.Repositorys
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        T GetById(Guid id);
        IEnumerable<T> List();
        IEnumerable<T> List(Expression<Func<T, bool>> predicate);
        Guid Insert(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
