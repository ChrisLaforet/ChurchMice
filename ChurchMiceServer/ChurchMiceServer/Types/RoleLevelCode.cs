namespace ChurchMiceServer.Types;

public class RoleLevelCode
{
	public string Code { get; private set; }

	public static RoleLevelCode From(string roleLevelCode) => new RoleLevelCode(roleLevelCode);

	public RoleLevelCode(string roleLevelCode)
	{
		if (string.IsNullOrEmpty(roleLevelCode))
		{
			throw new ArgumentException("A role level code cannot be null or empty");
		}
        
		this.Code = roleLevelCode;
	}

	public RoleLevelCode(RoleLevelCode that)
	{
		this.Code = that.Code;
	}
	
	public static implicit operator string(RoleLevelCode that)
	{
		return that.Code;
	}
    
	public override string ToString()
	{
		return Code;
	}
}