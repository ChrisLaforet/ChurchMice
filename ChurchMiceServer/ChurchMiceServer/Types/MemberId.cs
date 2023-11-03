using ChurchMiceServer.Types.Base;

namespace ChurchMiceServer.Types;

public class MemberId : IntegerIdType
{
    public MemberId(int id) : base(id) {}
    
    public MemberId(MemberId that) : base(that.Id) {}

}