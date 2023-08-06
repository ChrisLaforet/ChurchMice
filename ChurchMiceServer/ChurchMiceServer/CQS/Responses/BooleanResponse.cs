namespace ChurchMiceServer.CQS.Responses;

public class BooleanResponse
{
    public bool Value { get; private set; }

    public BooleanResponse(bool value)
    {
        this.Value = value;
    }
    
}