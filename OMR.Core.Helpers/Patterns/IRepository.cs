using System;
using System.Linq;
using System.Linq.Expressions;

namespace OMR.Core.Helpers.Patterns
{
    public interface IRepository<T> : IDisposable where T : class
    {
        T Create(T entity);
        void Insert(T entity);
        void Update(T entity);
        T Get(object id);
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
        void Delete(T entity);
        void Validate(T entity);
    }
}
