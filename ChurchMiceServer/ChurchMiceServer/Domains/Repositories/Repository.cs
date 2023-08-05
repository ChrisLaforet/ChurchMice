using System.Linq.Expressions;
using ChurchMiceServer.Persistence;

namespace ChurchMiceServer.Domains.Repositories;

public class Repository<T> : IRepository<T> where T: class
{
    protected readonly ChurchMiceContext context;
    
    public Repository(ChurchMiceContext context)
    {
        this.context = context;
    }
    
    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }
    
    public void AddRange(IEnumerable<T> entities)
    {
        context.Set<T>().AddRange(entities);
    }
    
    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return context.Set<T>().Where(expression);
    }
    
    public IEnumerable<T> GetAll()
    {
        return context.Set<T>().ToList();
    }
    
    public T GetById(int id)
    {
        return context.Set<T>().Find(id);
    }
    
    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }
    
    public void RemoveRange(IEnumerable<T> entities)
    {
        context.Set<T>().RemoveRange(entities);
    }
}