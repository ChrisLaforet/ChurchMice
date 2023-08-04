using ChurchMiceServer.Domains;
using ChurchMiceServer.Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChurchMiceTesting;

public class MockContext
{
    private readonly Mock<DbSet<User>> mockUsers = new Mock<DbSet<User>>();
    private readonly Mock<DbSet<UserRole>> mockUserRoles = new Mock<DbSet<UserRole>>();
    private readonly Mock<DbSet<UserToken>> mockUserTokens = new Mock<DbSet<UserToken>>();
    private readonly Mock<DbSet<EmailQueue>> mockEmailQueue = new Mock<DbSet<EmailQueue>>();
    private readonly Mock<DbSet<Member>> mockMembers = new Mock<DbSet<Member>>();
        
    private readonly Mock<ChurchMiceContext> mockContext = new Mock<ChurchMiceContext>();
    
    public MockContext()
    {
        mockContext.Setup(m => m.Users).Returns(mockUsers.Object);
        mockContext.Setup(m => m.UserRoles).Returns(mockUserRoles.Object);
        mockContext.Setup(m => m.UserTokens).Returns(mockUserTokens.Object);
        mockContext.Setup(m => m.EmailQueue).Returns(mockEmailQueue.Object);
        mockContext.Setup(m => m.Members).Returns(mockMembers.Object);
    }

    public IChurchMiceContext Get()
    {
        return mockContext.Object;
    }

    public void VerifySaveChanges(int expectedCount)
    {
        mockContext.Verify(m => m.SaveChanges(), Times.Exactly(expectedCount));
    }

    public void VerifyRemoveUserToken(int expectedCount)
    {
        mockContext.Verify(m => m.Remove(It.IsAny<UserToken>()), Times.Exactly(expectedCount));
    }
}