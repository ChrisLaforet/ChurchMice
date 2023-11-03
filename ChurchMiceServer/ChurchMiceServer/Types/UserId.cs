using ChurchMiceServer.Types.Base;

namespace ChurchMiceServer.Types;

public class UserId : StringIdType
{
    public UserId(string id) : base(id) {}
    
    public UserId(UserId that) : base(that.value) {}
}