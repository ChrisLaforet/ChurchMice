namespace ChurchMiceServer.Security.Auth;

public class Role
{
	// Note: ChurchMice uses the concept of Cascading Roles, that is, higher roles include all lower roles.
	// This approach is not necessarily valid for other implementations where roles are discrete grants.
	public const int NO_ACCESS_LEVEL = 0;
	public const int ATTENDER_LEVEL = 100;
	public const int MEMBER_LEVEL = 1000;
	public const int ADMINISTRATOR_LEVEL = 10000;
	
	public int Level { get; private set; }
	public string Code { get; private set; }
	public bool HasAccess { get; private set; }
	public bool IsMember { get; private set; }
	public bool IsAdministrator { get; private set; }

	public bool IsNoAccess
	{
		get
		{
			return Level == NO_ACCESS_LEVEL;
		}
	}

	public static Role GetNoAccess()
	{
		return new Role()
		{
			Level = NO_ACCESS_LEVEL,
			Code = "NoAccess",
			HasAccess = false,
			IsMember = false,
			IsAdministrator = false
		};
	}
	
	public static Role GetAttender()
	{
		return new Role()
		{
			Level = ATTENDER_LEVEL,
			Code = "Attender",
			HasAccess = true,
			IsMember = false,
			IsAdministrator = false
		};
	}
	
	public static Role GetMember()
	{
		return new Role()
		{
			Level = MEMBER_LEVEL,
			Code = "Member",
			HasAccess = true,
			IsMember = true,
			IsAdministrator = false
		};
	}
	
	public static Role GetAdministrator()
	{
		return new Role()
		{
			Level = ADMINISTRATOR_LEVEL,
			Code = "Administrator",
			HasAccess = true,
			IsMember = true,
			IsAdministrator = true
		};
	}
}