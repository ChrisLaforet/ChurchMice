using System.Linq.Expressions;
using ChurchMiceServer.Domains;

namespace ChurchMiceTesting.Mocks;

public class MockRepository<T> : IRepository<T> where T: class
{
	private List<T> records = new List<T>();
	
	public T GetById(int id)
	{
		// How to determine which field is an ID??
		// What about an ID that is not an int!
		throw new NotImplementedException();
	}

	public IEnumerable<T> GetAll()
	{
		return records;
	}

	public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
	{
		// how to transform my expression to work with List.Find()?
		//return records.Find(expression);
		throw new NotImplementedException();
	}

	public void Add(T entity)
	{
		records.Add(entity);
	}

	public void AddRange(IEnumerable<T> entities)
	{
		records.AddRange(entities);
	}

	public void Remove(T entity)
	{
		records.Remove(entity);
	}

	public void RemoveRange(IEnumerable<T> entities)
	{
		throw new NotImplementedException();
	}
}