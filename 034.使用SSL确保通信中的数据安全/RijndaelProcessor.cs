using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace _034.使用SSL确保通信中的数据安全
{
	public class RijndaelProcessor
	{
		private static int bufferSize = 128 * 1024;
		private static byte[] salt = { 134, 216, 7, 36, 88, 164, 91, 227, 174, 76, 191, 197, 192, 154, 200, 248 };
		private static byte[] iv = { 134, 216, 7, 36, 88, 164, 91, 227, 174, 76, 191, 197, 192, 154, 200, 248 };

		private static SymmetricAlgorithm CreateRijndael(string password, byte[] salt)
		{
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt, "SHA256", 1000);
			SymmetricAlgorithm sma = Rijndael.Create();
			sma.KeySize = 256;
			sma.Key = pdb.GetBytes(32);
			sma.Padding = PaddingMode.PKCS7;
			return sma;
		}

		public static string EncryptString(string input, string password)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
			{
				algorithm.IV = iv;
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
				{
					byte[] bytes = UTF32Encoding.Default.GetBytes(input);
					cryptoStream.Write(bytes, 0, bytes.Length);
					cryptoStream.Flush();
				}
				return Convert.ToBase64String(memoryStream.ToArray());
			}
		}

		public static string DencryptString(string input, string password)
		{
			using (MemoryStream inputMemoryStream = new MemoryStream(Convert.FromBase64String(input)))
			using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
			{
				algorithm.IV = iv;
				using (CryptoStream cryptoStream = new CryptoStream(inputMemoryStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read))
				{
					StreamReader sr = new StreamReader(cryptoStream);
					return sr.ReadToEnd();
				}
			}
		}
	}
}