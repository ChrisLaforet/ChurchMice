namespace ChurchMiceServer.Security.Auth;

public class Role
{
	public const int NO_ACCESS_LEVEL = 0;
	public const int ATTENDER_LEVEL = 100;
	public const int MEMBER_LEVEL = 1000;
	public const int ADMINISTRATOR_LEVEL = 10000;
	
	public int Level { get; private set; }
	public string Name { get; private set; }
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
			Name = "NoAccess",
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
			Name = "Attender",
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
			Name = "Member",
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
			Name = "Administrator",
			HasAccess = true,
			IsMember = true,
			IsAdministrator = true
		};
	}
}