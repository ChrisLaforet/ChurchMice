using ChurchMiceServer.Domains.Interfaces;

namespace ChurchMiceServer.Domains.Models;

public partial class UserToken : IRepositoryIndex<string>
{
	public string GetIndex()
	{
		return Id;
	}
}