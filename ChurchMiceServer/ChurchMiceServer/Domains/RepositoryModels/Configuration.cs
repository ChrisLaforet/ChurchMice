using ChurchMiceServer.Domains.Interfaces;

namespace ChurchMiceServer.Domains.Models;

public partial class Configuration : IRepositoryIndex<string>
{
	public string GetIndex()
	{
		return Keyword;
	}

	public void SetIndex(object index)
	{
		this.Keyword = (string)index;
	}
}