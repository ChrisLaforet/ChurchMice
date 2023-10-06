namespace ChurchMiceServer.CQS.Exceptions;

public class UserNotFoundException : Exception
{
	public UserNotFoundException(string userId) 
		: base($"Unable to find a user with Id of {userId}") 
	{ }
}