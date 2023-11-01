namespace ChurchMiceServer.CQS.Exceptions;

public class UserNotPermittedException : Exception
{
	public UserNotPermittedException(string userId, string reason) 
		: base($"User with Id of {userId} is not permitted to {reason}") 
	{ }
}