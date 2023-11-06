using ChurchMiceServer.Types.Base;

namespace ChurchMiceServer.Types;

public class MemberId : IntegerIdType
{
    public static MemberId From(int memberId) => new MemberId(memberId);
    
    public MemberId(int id) : base(id) {}
    
    public MemberId(MemberId that) : base(that.Id) {}
}