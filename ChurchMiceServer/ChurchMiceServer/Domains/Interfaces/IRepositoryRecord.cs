namespace ChurchMiceServer.Domains.Interfaces;

public interface IRepositoryIndex<out T> where T: class
{
	T GetId();
}