namespace ChurchMiceServer.CQS.Exceptions;

public class UserNotPermittedxception : Exception
{
	public UserNotPermittedxception(string userId, string reason) 
		: base($"User with Id of {userId} is not permitted to {reason}") 
	{ }
}