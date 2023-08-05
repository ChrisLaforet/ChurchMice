using ChurchMiceServer.Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChurchMiceServer.Persistence;


// TODO: CML - Remove this completely once repository is in place

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