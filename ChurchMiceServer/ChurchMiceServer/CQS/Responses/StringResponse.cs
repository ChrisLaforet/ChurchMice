namespace ChurchMiceServer.CQS.Responses;

public class StringResponse
{
    public string Value { get; private set; }

    public StringResponse(string value)
    {
        this.Value = value;
    }
}