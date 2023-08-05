using ChurchMiceServer.Domains.Interfaces;

namespace ChurchMiceServer.Domains.Models;

public partial class Member : IRepositoryIndex<int>
{
	public int GetIndex()
	{
		return Id;
	}
}