using ChurchMiceServer.Domains.Interfaces;

namespace ChurchMiceServer.Domains;

public interface IRepositoryContext : IDisposable
{
        IEmailQueueRepository EmailQueues { get; }
        IMemberRepository Members { get; }
        IUserRepository Users { get; }
        IUserRoleRepository UserRoles { get; }
        IUserTokenRepository UserTokens { get; }
        int SaveChanges();

}
