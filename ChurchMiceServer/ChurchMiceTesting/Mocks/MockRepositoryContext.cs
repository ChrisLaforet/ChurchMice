using ChurchMiceServer.Domains;
using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;

namespace ChurchMiceTesting.Mocks;

public class MockRepositoryContext : IRepositoryContext
{
	private int savedChanges = 0;
	
	public MockRepositoryContext()
	{
		EmailQueues = new MockEmailQueueRepository();
		repositories.Add((IMockRepositoryMetrics)EmailQueues);

		Members = new MockMemberRepository();
		repositories.Add((IMockRepositoryMetrics)Members);
		
		Users = new MockUserRepository();
		repositories.Add((IMockRepositoryMetrics)Users);
		
		UserRoles = new MockUserRoleRepository();
		repositories.Add((IMockRepositoryMetrics)UserRoles);
		
		UserTokens = new MockUserTokenRepository();
		repositories.Add((IMockRepositoryMetrics)UserTokens);
	}
	
	public void Dispose()
	{
		// nothing to do in mock scenario
	}

	private List<IMockRepositoryMetrics> repositories = new List<IMockRepositoryMetrics>();
	
	public IEmailQueueRepository EmailQueues { get; private set; }
	public IMemberRepository Members { get; private set; }
	public IUserRepository Users { get; private set; }
	public IUserRoleRepository UserRoles { get; private set; }
	public IUserTokenRepository UserTokens { get; private set; }
	
	public int SaveChanges()
	{
		int totalChanges = repositories.Sum(repository => repository.GetChangeCount());
		int newChanges = totalChanges - savedChanges;
		savedChanges = totalChanges;
		return newChanges;
	}

	public void Remove(object toRemove)
	{
		if (toRemove is EmailQueue queue)
		{
			EmailQueues.Remove(queue);
		} 
		else if (toRemove is Member member)
		{
			Members.Remove(member);
		}
		else if (toRemove is User user)
		{
			Users.Remove(user);
		}
		else if (toRemove is UserRole userRole)
		{
			UserRoles.Remove(userRole);
		}
		else if (toRemove is UserToken userToken)
		{
			UserTokens.Remove(userToken);
		}
	}

	public int GetChangeCount()
	{
		return savedChanges;
	}
}