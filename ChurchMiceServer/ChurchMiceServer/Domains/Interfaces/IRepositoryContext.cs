using ChurchMiceServer.Domains.Interfaces;

namespace ChurchMiceServer.Domains;

public interface IRepositoryContext : IDisposable
{
        IConfigurationRepository Configurations { get; }
        IEmailQueueRepository EmailQueues { get; }
        IMemberRepository Members { get; }
        IUserRepository Users { get; }
        IUserRoleRepository UserRoles { get; }
        IUserTokenRepository UserTokens { get; }
        IMemberImageRepository MemberImages { get; }
        int SaveChanges();
        void Remove(object toRemove);
}
