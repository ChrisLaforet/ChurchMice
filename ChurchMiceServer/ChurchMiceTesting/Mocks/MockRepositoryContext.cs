using ChurchMiceServer.Domains;
using ChurchMiceServer.Domains.Interfaces;

namespace ChurchMiceTesting.Mocks;

public class MockRepositoryContext : IRepositoryContext
{
	public void Dispose()
	{
		// nothing to do in mock scenario
	}

	public IEmailQueueRepository EmailQueues { get; }
	public IMemberRepository Members { get; }
	public IUserRepository Users { get; private set; }
	public IUserRoleRepository UserRoles { get; }
	public IUserTokenRepository UserTokens { get; }
	
	public int SaveChanges()
	{
		// nothing to do in mock scenario (for now)
		return 0;
	}
}