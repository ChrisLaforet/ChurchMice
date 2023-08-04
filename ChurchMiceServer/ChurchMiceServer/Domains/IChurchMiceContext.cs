using ChurchMiceServer.Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChurchMiceServer.Domains;

public interface IChurchMiceContext
{
    DbSet<User> Users { get; set; }
    DbSet<UserRole> UserRoles { get; set; }
    DbSet<UserToken> UserTokens { get; set; }
    DbSet<EmailQueue> EmailQueue { get; set; }
    DbSet<Member> Members { get; set; }

    int SaveChanges();
    EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
}