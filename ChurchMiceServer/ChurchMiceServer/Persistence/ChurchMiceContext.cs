using ChurchMiceServer.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace ChurchMiceServer.Persistence;

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
    
    public virtual DbSet<EmailQueue> EmailQueue { get; set; }
    
    public virtual DbSet<Member> Members { get; set; }
    
    public virtual DbSet<MemberEditor> MemberEditors { get; set; }
    
    public virtual DbSet<MemberImage> MemberImages { get; set; }
    
    public virtual DbSet<ChurchMiceServer.Domains.Models.Configuration> Configurations { get; set; }

	
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // empty for now - unless some config has to be done
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

            entity.Property(e => e.Fullname)
                .IsRequired()
                .HasColumnName("Fullname")
                .HasMaxLength(50)
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
                .HasMaxLength(50)
                .IsUnicode(false)
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
            entity.Property(e => e.Id)
                .HasColumnName("ID");
            
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
        
        modelBuilder.Entity<EmailQueue>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("EmailQueue");

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.EmailRecipient)
                .IsRequired()
                .HasColumnName("EmailRecipient")
                .HasMaxLength(255)
                .IsUnicode(false);
            
            entity.Property(e => e.EmailSender)
                .IsRequired()
                .HasColumnName("EmailSender")
                .HasMaxLength(255)
                .IsUnicode(false);
            
            entity.Property(e => e.EmailSubject)
                .HasColumnName("EmailSubject")
                .HasMaxLength(255)
                .IsUnicode(false);
            
            entity.Property(e => e.EmailBody)
                .IsRequired()
                .HasColumnName("EmailBody")
                .IsUnicode(false);
            
            entity.Property(e => e.SentDatetime)
                .HasColumnName("SentDatetime")
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.AttemptDatetime)
                .HasColumnName("AttemptDatetime")
                .HasColumnType("datetime");
            
            entity.Property(e => e.TotalAttempts)
                .HasColumnName("TotalAttempts");
            
            entity.Property(e => e.AttachmentFilename)
                .HasColumnName("AttachmentFilename")
                .IsUnicode(false);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(20)
                .IsRequired();
 
            entity.Property(e => e.Email)
                .HasColumnName("Email")
                .HasMaxLength(255);
 
            entity.Property(e => e.HomePhone)
                .HasColumnName("HomePhone")
                .HasMaxLength(20);
 
            entity.Property(e => e.MobilePhone)
                .HasColumnName("MobilePhone")
                .HasMaxLength(20);
 
            entity.Property(e => e.MailingAddress1)
                .HasColumnName("MailingAddress1")
                .HasMaxLength(100);
             
            entity.Property(e => e.MailingAddress2)
                .HasColumnName("MailingAddress2")
                .HasMaxLength(100);

            entity.Property(e => e.City)
                .HasColumnName("City")
                .HasMaxLength(50);

            entity.Property(e => e.State)
                .HasColumnName("State")
                .HasMaxLength(2);
            
            entity.Property(e => e.Zip)
                .HasColumnName("Zip")
                .HasMaxLength(10);
            
            entity.Property(e => e.Birthday)
                .HasColumnName("Birthday")
                .HasMaxLength(20);
            
            entity.Property(e => e.Anniversary)
                .HasColumnName("Anniversary")
                .HasMaxLength(20);

            entity.Property(e => e.MemberSince)
                .HasColumnName("MemberSince")
                .HasColumnType("datetime");
            
            entity.Property(e => e.Created)
                .HasColumnName("Created")
                .HasColumnType("datetime")
                .IsRequired();
  
            entity.Property(e => e.UserId)
                .HasColumnName("UserID")
                .HasMaxLength(50);
          });


        modelBuilder.Entity<MemberEditor>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("MemberEditor");
            
            entity.Property(e => e.MemberId)
                .HasColumnName("MemberID")
                .IsRequired();
              
            entity.Property(e => e.EditorId)
                .HasColumnName("EditorId")
                .HasMaxLength(50)
                .IsRequired();
        });

        modelBuilder.Entity<MemberImage>(entity =>
        {
            entity.HasKey(e => e.Id);
                
            entity.ToTable("MemberImage");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.MemberId)
                .HasColumnName("MemberID")
                .IsRequired();

            entity.Property(e => e.Image)
                .HasColumnName("Image")
                .IsRequired();

			entity.Property(e => e.ImageType)
	            .HasColumnName("ImageType")
                .HasMaxLength(50)
	            .IsRequired();

			entity.Property(e => e.UploadUserId)
	            .HasColumnName("UploadUserID")
				.HasMaxLength(50)
				.IsRequired();

			entity.Property(e => e.UploadDate)
                .HasColumnName("UploadDate")
                .HasColumnType("datetime")
                .IsRequired();
            
            entity.Property(e => e.ApproveDate)
                .HasColumnName("ApproveDate")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ChurchMiceServer.Domains.Models.Configuration>(entity =>
        {
            entity.HasKey(e => e.Keyword);

            entity.ToTable("Configuration");

            entity.Property(e => e.Keyword)
                .HasColumnName("Keyword")
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Value)
                .IsRequired()
                .HasColumnName("Value")
                .HasMaxLength(1024)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }
}
