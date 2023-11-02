using ChurchMiceServer.Domains.Models;

namespace ChurchMiceServer.CQS.Responses;

public class MemberImagesResponse
{
    public List<MemberImageResponse> Images { get; private set; }

    public MemberImagesResponse()
    {
        Images = new List<MemberImageResponse>();
    }
    
    public void Add(MemberImageResponse response)
    {
        Images.Add(response);
    }

    public void Add(MemberImage memberImage)
    {
        Images.Add(new MemberImageResponse(memberImage));
    }
}