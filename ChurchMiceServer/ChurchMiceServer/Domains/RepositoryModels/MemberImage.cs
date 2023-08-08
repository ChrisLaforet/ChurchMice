using ChurchMiceServer.Domains.Interfaces;

namespace ChurchMiceServer.Domains.Models;

public partial class MemberImage : IRepositoryIndex<int>
{
	public int GetIndex()
	{
		return Id;
	}

	public void SetIndex(object index)
	{
		this.Id = (int)index;
	}
}