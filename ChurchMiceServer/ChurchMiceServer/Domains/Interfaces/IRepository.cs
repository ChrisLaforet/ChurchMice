using System.Linq.Expressions;

namespace ChurchMiceServer.Domains;

// Based on: https://codewithmukesh.com/blog/repository-pattern-in-aspnet-core/

public interface IRepository<T> where T: class
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}