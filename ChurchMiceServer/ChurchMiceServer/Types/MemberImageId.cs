using ChurchMiceServer.Types.Base;

namespace ChurchMiceServer.Types;

public class MemberImageId : IntegerIdType
{
    public MemberImageId(int id) : base(id) {}
    
    public MemberImageId(MemberId that) : base(that.Id) {}

}