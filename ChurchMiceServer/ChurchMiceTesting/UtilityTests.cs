using System.Text;
using ChurchMiceServer.Utility;

namespace ChurchMiceTesting;

public class UtilityTests
{
	private const string STRING_TO_COMPRESS = "When developing applications you will often need to deal with strings. And because string objects are costly in terms of performance, you will often want to compress your string content, i.e., the data inside your string objects, to reduce the payload.";
	
	[Fact]
	public void GivenAnUncompressedString_WhenCompressedAndDecompressed_ThenStringValueIsTheSame()
	{
		var compressed = Compression.CompressToBase64(STRING_TO_COMPRESS);
		var decompressed = Compression.DecompressFromBase64(compressed);
		Assert.Equal(STRING_TO_COMPRESS, Encoding.UTF8.GetString(decompressed));
	}

	[Theory]
	// [InlineData("user@[IPv6:2001:db8:1ff::a0b:dbd0]")]  -- IPv6 is legitimately the domain part
	[InlineData("chris@laforet.name")]
	// Seeded below with examples from cjaude: https://gist.github.com/cjaoude/fd9910626629b53c4d25
	[InlineData("email@example.com")]
	[InlineData("firstname.lastname@example.com")]
	[InlineData("email@subdomain.example.com")]
	[InlineData("firstname+lastname@example.com")]
	[InlineData("email@123.123.123.123")]
	[InlineData("email@[123.123.123.123]")]
	[InlineData("\"email\"@example.com")]
	[InlineData("1234567890@example.com")]
	[InlineData("email@example-one.com")]
	[InlineData("_______@example.com")]
	[InlineData("email@example.name")]
	[InlineData("email@example.museum")]
	[InlineData("email@example.co.jp")]
	[InlineData("firstname-lastname@example.com")]
	// These are actually valid by the spec for local-part
	[InlineData("あいうえお@example.com")]
	[InlineData(".email@example.com")]
	[InlineData("Abc..123@example.com")]
	[InlineData("email..email@example.com")]
	[InlineData("email@111.222.111.222")]
	[InlineData("email@example.web")]
	// These are very unusual examples
	// [InlineData("much.”more\\ unusual”@example.com")]
	// [InlineData("very.unusual.”@”.unusual.com@example.com")]
	// [InlineData("very.”(),:;<>[]”.VERY.”very@\\ \"very”.unusual@strange.example.com")]
	public void GivenProperlyFormedAddress_WhenValidated_ThenReturnsTrue(string email)
	{
		Assert.True(EmailAddressValidation.IsValidEmail(email));
	}
	
	[Theory]
	// [InlineData("email@spammy_7.example.com")] -- underscores should not be permitted in domain part
	// Seeded below with examples from cjaude: https://gist.github.com/cjaoude/fd9910626629b53c4d25
	[InlineData("plainaddress")]
	[InlineData("#@%^%#$@#$@#.com")]
	[InlineData("@example.com")]
	[InlineData("Joe Smith <email@example.com>")]
	[InlineData("email@example@example.com")]
	// [InlineData("email.@example.com")]  -- cannot end local-part with .
	[InlineData("email@example.com (Joe Smith)")]
	[InlineData("email@example")]
	[InlineData("email@-example.com")]
	//[InlineData("email@111.222.333.44444")]
	[InlineData("email@example..com")]
	[InlineData("email.example.com")]
	// [InlineData("”(),:;<>[\\]@example.com")]
	// [InlineData("just”not”right@example.com")]
	[InlineData("this\\ is\"really\"not\\allowed@example.com")]
	public void GivenMalformedAddress_WhenValidated_ThenReturnsFalse(string email)
	{
		Assert.False(EmailAddressValidation.IsValidEmail(email));
	}
}

