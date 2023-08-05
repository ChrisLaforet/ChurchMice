namespace ChurchMiceServer.Domains.Interfaces;

public interface IRepositoryIndex<K>
{
	K GetIndex();
	void SetIndex(object index);
}