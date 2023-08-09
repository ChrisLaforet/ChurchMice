using System.IO.Compression;
using System.Text;

namespace ChurchMiceServer.Utility
{
	// borrowed from https://www.infoworld.com/article/3660629/how-to-compress-and-decompress-strings-in-c-sharp.html

	public class Compression
	{

		public static string CompressToBase64(string toCompress)
		{
			return Convert.ToBase64String(CompressBytes(Encoding.UTF8.GetBytes(toCompress)));
		}

		public static byte[] CompressBytes(byte[] bytes)
		{
			using (var memoryStream = new MemoryStream())
			{
				using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
				{
					gzipStream.Write(bytes, 0, bytes.Length);
				}
				return memoryStream.ToArray();
			}
		}

		public static string DecompressFromBase64(string toDecompress)
		{
			return Encoding.UTF8.GetString(DecompressBytes(Convert.FromBase64String(toDecompress)));
		}

		public static byte[] DecompressBytes(byte[] bytes)
		{
			using (var memoryStream = new MemoryStream(bytes))
			{
				using (var outputStream = new MemoryStream())
				{
					using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
					{
						decompressStream.CopyTo(outputStream);
					}
					return outputStream.ToArray();
				}
			}
		}
	}
}
