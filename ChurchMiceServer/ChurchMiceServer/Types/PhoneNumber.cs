using System.Text;
using Microsoft.EntityFrameworkCore.Internal;

namespace ChurchMiceServer.Types;

public class PhoneNumber
{
	private string number;
	
	public PhoneNumber(string value)
	{
		var compressedNumber = CompressPhoneNumber(value);
		if (string.IsNullOrEmpty(compressedNumber))
		{
			throw new ArgumentException("A phone number cannot be null or empty");
		}

		number = compressedNumber;
	}

	public string GetCompressed()
	{
		return number;
	}

	public string GetFormatted()
	{
		var possiblePlus1 = number.Length == 11 && number.StartsWith('1');
		
		// currently only formats numbers with North American Numbering Plan
		if (number.Length == 10 || possiblePlus1)
		{
			var toFormat = possiblePlus1 ? number.Substring(1) : number;
			var prefix = possiblePlus1 ? "+1 " : "";
			return $"{prefix}({toFormat.Substring(0, 3)}) {toFormat.Substring(3, 3)}-{toFormat.Substring(6)}";
		}

		return number;
	}
	
	public static string CompressPhoneNumber(string value)
	{
		var number = new StringBuilder();
		if (value != null)
		{
			foreach (var ch in value)
			{
				if (char.IsDigit(ch))
				{
					number.Append(ch);
				}
			}
		}
		return number.ToString();
	}
    
	public override string ToString()
	{
		return GetFormatted();
	}
}