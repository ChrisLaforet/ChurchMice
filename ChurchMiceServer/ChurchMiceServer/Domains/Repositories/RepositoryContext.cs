using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Persistence;

namespace ChurchMiceServer.Domains.Repositories;


public class RepositoryContext : IRepositoryContext
{
	private readonly ChurchMiceContext context;

	public RepositoryContext(ChurchMiceContext context)
	{
		this.context = context;
		this.Configurations = new ConfigurationRepository(context);
		this.EmailQueues = new EmailQueueRepository(context);
		this.Members = new MemberRepository(context);
		this.Users = new UserRepository(context);
		this.UserRoles = new UserRoleRepository(context);
		this.UserTokens = new UserTokenRepository(context);
		this.MemberImages = new MemberImageRepository(context);
	}
	
	public void Dispose()
	{
		context.Dispose();
	}

	public IConfigurationRepository Configurations { get; set; }
	
	public IEmailQueueRepository EmailQueues { get; private set; }
	
	public IMemberRepository Members { get; private set; }
	
	public IUserRepository Users { get; private set; }
	
	public IUserRoleRepository UserRoles { get; private set; }
	
	public IUserTokenRepository UserTokens { get; private set; }
	
	public IMemberImageRepository MemberImages { get; private set; }
	
	
	public int SaveChanges()
	{
		return context.SaveChanges();
	}

	public void Remove(object toRemove)
	{
		context.Remove(toRemove);
	}
}