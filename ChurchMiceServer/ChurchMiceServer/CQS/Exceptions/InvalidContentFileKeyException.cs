namespace ChurchMiceServer.CQS.Exceptions;


public class InvalidContentFileKeyException : Exception
{
	public InvalidContentFileKeyException(string key) 
		: base($"Invalid key {key} does not correspond to valid user content file type") { }
}