namespace ChurchMiceServer.Domains.Exceptions;

public class NotConfiguredException : Exception
{
	public NotConfiguredException(string keyword) : base($"{keyword} is not configured") { }
}