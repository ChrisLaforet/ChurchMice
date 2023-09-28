namespace ChurchMiceServer.CQS.Exceptions;

public class InvalidFieldException : Exception
{
	public InvalidFieldException(string fieldName) 
		: base($"Invalid value for {fieldName} does not correspond to minimum requirements") { }
}