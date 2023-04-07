using ChurchMiceServer.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace ChurchMiceServer.Domains;

public partial class ChurchMiceContext : Microsoft.EntityFrameworkCore.DbContext
{
	public ChurchMiceContext() : base()
	{
	}

	public ChurchMiceContext(DbContextOptions<ChurchMiceContext> options)
		: base(options)
	{
	}
    
	public virtual DbSet<User> Users { get; set; }
    
    public virtual DbSet<UserRole> UserRoles { get; set; }
    
    public virtual DbSet<UserToken> UserTokens { get; set; }

	
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("User");

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Username)
                .IsRequired()
                .HasColumnName("Username")
                .HasMaxLength(20)
                .IsUnicode(false);
            
            entity.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("Email")
                .HasMaxLength(100)
                .IsUnicode(false);
            
            entity.Property(e => e.CreateDate)
                .HasColumnName("CreateDate")
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.PasswordHash)
                .HasColumnName("PasswordHash")
                .HasMaxLength(1000)
                .IsUnicode(false);
            
            entity.Property(e => e.LastLoginDatetime)
                .HasColumnName("LastLoginDatetime")
                .HasColumnType("datetime");
            
            entity.Property(e => e.ResetExpirationDatetime)
                .HasColumnName("ResetExpirationDatetime")
                .HasColumnType("datetime");
            
            entity.Property(e => e.ResetKey)
                .HasColumnName("ResetKey")
                .HasMaxLength(50)
                .IsUnicode(false);
        });
        
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id);
                
            entity.ToTable("UserRole");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.UserId)
                .HasColumnName("UserID")
                .IsRequired();
                
            entity.Property(e => e.RoleLevel)
                .HasColumnName("RoleLevel")
                .IsRequired();
                
            // entity.HasOne(d => d.UserId)
            //     .WithMany(p => p.Id)
            //     .HasForeignKey(d => d.UserId)
            //     .HasConstraintName("FK_UserRole_User");
        });
        
        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable("UserToken");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
				
            entity.Property(e => e.UserId)
                .HasColumnName("UserID")
                .IsRequired();

            entity.Property(e => e.TokenKey)
                .HasColumnName("TokenKey")
                .HasMaxLength(255)                
                .IsRequired();

            entity.Property(e => e.Created)
                .HasColumnName("Created")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(e => e.Expired)
                .HasColumnName("Expired")
                .HasColumnType("datetime");
        });
        
        OnModelCreatingPartial(modelBuilder);
    }
}