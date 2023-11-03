namespace ChurchMiceServer.Types.Base;


public abstract class IntegerIdType
{
    protected int value;

    protected IntegerIdType(int value)
    {
        if (value < 0)
        {
            throw new ArgumentException("An integer id cannot be negative");
        }
        
        this.value = value;
    }

    public int Id => value;

    public static implicit operator int(IntegerIdType that)
    {
        return that.Id;
    }
    
    public override string ToString()
    {
        return value.ToString();
    }
}