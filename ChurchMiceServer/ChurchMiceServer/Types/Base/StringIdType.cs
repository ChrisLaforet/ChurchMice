namespace ChurchMiceServer.Types.Base;

public abstract class StringIdType
{
    protected string value;

    protected StringIdType(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("A string id cannot be null or empty");
        }
        
        this.value = value;
    }

    public string Id => value;
    
    public static implicit operator string(StringIdType that)
    {
        return that.Id;
    }
    
    public override string ToString()
    {
        return value;
    }
}