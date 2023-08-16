using ChurchMiceServer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchMiceTesting
{
	public class UtilityTests
	{
		private const string STRING_TO_COMPRESS = "When developing applications you will often need to deal with strings. And because string objects are costly in terms of performance, you will often want to compress your string content, i.e., the data inside your string objects, to reduce the payload.";
		
		[Fact]
		public void GivenAnUncompressedString_WhenCompressedAndDecompressed_ThenStringValueIsTheSame()
		{
			var compressed = Compression.CompressToBase64(STRING_TO_COMPRESS);
			Assert.Equal(STRING_TO_COMPRESS, Compression.DecompressFromBase64(compressed));
		}
	}
}
