namespace ChurchMiceServer.Security.Auth;

public class Roles
{
	private readonly Role[] roles = { Role.GetNoAccess(), Role.GetAttender(), Role.GetMember(), Role.GetAdministrator() };
	
	public Role[] GetRoles() {
	    return roles;
	}
	
	public Role GetRoleByLevel(int level) {
	    foreach (var role in roles) {
	        if (role.Level == level) {
	            return role;
	        }
	    }
	    return Role.GetNoAccess();
	}
}