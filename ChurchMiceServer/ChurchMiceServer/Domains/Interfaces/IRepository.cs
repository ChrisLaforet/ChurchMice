using System.Linq.Expressions;
using ChurchMiceServer.Domains.Interfaces;

namespace ChurchMiceServer.Domains;

// Based on: https://codewithmukesh.com/blog/repository-pattern-in-aspnet-core/

public interface IRepository<T, K> where T: IRepositoryIndex<K>
{
    T GetById(K id);
    IEnumerable<T> GetAll();
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}