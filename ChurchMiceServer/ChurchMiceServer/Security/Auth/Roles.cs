namespace ChurchMiceServer.Security.Auth;

public class Roles
{
	private static readonly Role[] roles = { Role.GetNoAccess(), Role.GetAttender(), Role.GetMember(), Role.GetAdministrator() };
	
	public static Role[] GetRoles() {
	    return roles;
	}
	
	public static Role GetRoleByLevel(int level)
	{
		Role foundRole = Role.GetNoAccess();
	    foreach (var role in roles) {
	        if (role.Level <= level && foundRole.Level < role.Level) {
	            foundRole = role;
	        }
	    }
	    return foundRole;
	}

	public static List<Role> GetAllRolesWithinLevel(int level)
	{
		var allRoles = new List<Role>();
		foreach (var role in GetRoles())
		{
			if (role.IsNoAccess)
			{
				continue;
			}

			if (role.Level <= level)
			{
				allRoles.Add(role);
			}
		}

		return allRoles;
	}

	public static Role? GetRoleByLevelCode(string roleLevelCode)
	{
		foreach (var role in GetRoles())
		{
			if (role.Code.Equals(roleLevelCode, StringComparison.CurrentCultureIgnoreCase))
			{
				return role;
			}
		}

		return null;
	}
}