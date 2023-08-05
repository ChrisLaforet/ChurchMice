using System.Collections;
using System.Linq.Expressions;
using ChurchMiceServer.Domains;
using ChurchMiceServer.Domains.Interfaces;

namespace ChurchMiceTesting.Mocks;

public class MockRepository<T,K> : IRepository<T,K>, IMockRepositoryMetrics where T: IRepositoryIndex<K> 
{
	private List<T> records = new List<T>();
	private int recordsAddedCount = 0;
	private int changes;
	
	public T? GetById(K id)
	{
		return records.Where(record => record.GetIndex().Equals(id)).FirstOrDefault();
	}

	public IEnumerable<T> GetAll()
	{
		return records;
	}

	public IEnumerable<T> Where(Expression<Func<T, bool>> expression)
	{
		foreach (var record in records)
		{
			if (expression.Compile().Invoke(record))
			{
				yield return record;
			}
		}
	}

	public T? Find(params object[] keyValues)
	{
		if (keyValues.Count() == 0)
		{
			return default;
		}

		foreach (var keyValue in keyValues)
		{
			if (!(keyValue is K))
			{
				return default;
			}
		}

		return records.Where(record =>
			{
				foreach (var keyValue in keyValues)
				{
					if (record.GetIndex().Equals((K)keyValue))
					{
						return true;
					}
				}

				return false;
			}).FirstOrDefault();
	}

	public void Add(T entity)
	{
		if (entity.GetIndex() == null)
		{
			if (entity.GetIndex() is int)
			{
				entity.SetIndex(recordsAddedCount);
			}
			else if (entity.GetIndex() is string)
			{
				entity.SetIndex(recordsAddedCount.ToString());
			}
		}
		records.Add(entity);
		++recordsAddedCount;
		++changes;
	}

	public void AddRange(IEnumerable<T> entities)
	{
		foreach (var entity in entities)
		{
			Add(entity);
		}
	}

	public void Remove(T entity)
	{
		records.Remove(entity);
		++changes;
	}

	public void Update(T entity)
	{
		if (entity.GetIndex() != null)
		{
			T? match = GetById(entity.GetIndex());
			if (match != null)
			{
				Remove(match);
			}
		}
        Add(entity);
	}

	public void RemoveRange(IEnumerable<T> entities)
	{
		foreach (var entity in entities)
		{
			Remove(entity);
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		return records.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public int GetChangeCount()
	{
		return changes;
	}
}