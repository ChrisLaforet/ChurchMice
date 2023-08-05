using ChurchMiceServer.Domains.Interfaces;

namespace ChurchMiceServer.Domains.Models;

public partial class User : IRepositoryIndex<string>
{
	public string GetIndex()
	{
		return Id;
	}
}