using ChurchMiceServer.Domains.Interfaces;

namespace ChurchMiceServer.Domains.Models;

public partial class EmailQueue : IRepositoryIndex<string>
{
	public string GetIndex()
	{
		return Id;
	}
}