namespace ChurchMiceServer.Types;

public class RoleLevel
{
	public int Level { get; private set; }


	public static RoleLevel From(int roleLevel) => new RoleLevel(roleLevel);
    
	public RoleLevel(int roleLevel)
	{
		if (roleLevel < 0)
		{
			throw new ArgumentException("An role level cannot be negative");
		}
        
		this.Level = roleLevel;	
	}
    
	public RoleLevel(RoleLevel that)
	{
		this.Level = that.Level;
	}
	
	public static implicit operator int(RoleLevel that)
	{
		return that.Level;
	}
	
	public override string ToString()
	{
		return Level.ToString();
	}
}