namespace ChurchMiceServer.CQS.Queries;

public class CheckExistingNameQuery: IQuery
{
    public const string CHECK_USERNAME = "USERNAME";
    public const string CHECK_EMAIL = "EMAIL";
    
    public string CheckField { get; private set; }
    public string CheckValue { get; private set; }

    public CheckExistingNameQuery(string checkField, string checkValue)
    {
        this.CheckField = checkField;
        this.CheckValue = checkValue;
    }
}