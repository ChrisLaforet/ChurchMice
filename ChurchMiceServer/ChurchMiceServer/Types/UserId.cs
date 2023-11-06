using ChurchMiceServer.Types.Base;

namespace ChurchMiceServer.Types;

public class UserId : StringIdType
{
    public static UserId From(string id) => new UserId(id);
    
    public UserId(string id) : base(id) {}
    
    public UserId(UserId that) : base(that.value) {}
}