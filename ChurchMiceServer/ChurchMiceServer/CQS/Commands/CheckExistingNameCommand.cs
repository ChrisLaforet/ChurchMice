using ChurchMiceServer.Controllers.Models;

namespace ChurchMiceServer.CQS.Commands;

public class CheckExistingNameCommand: ICommand
{
    public string CheckField { get; private set; }
    public string CheckValue { get; private set; }

    public CheckExistingNameCommand(string checkField, string checkValue)
    {
        this.CheckField = checkField;
        this.CheckValue = checkValue;
    }
}