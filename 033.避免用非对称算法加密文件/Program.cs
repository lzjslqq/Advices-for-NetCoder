using System;
using System.IO;
using System.Security.Cryptography;

namespace _033.避免用非对称算法加密文件
{
	/// <summary>
	///		在非对称算法中，首先应该有一个密钥对，这个密钥对含有两部分内容，分别称作公钥（PK）和私钥（SK），公钥通常用来加密，私钥则用来解密。
	///		在对称算法中，也可以有两个密钥（即加密密钥和解密密钥）。但是，对称算法中的加密密钥和解密密钥可以相互转换，而在非对称算法中，则不
	/// 能通过公钥推算出私钥。所以，我们完全可以将公钥公开到任何地方。密文可以被截获，但是由于截获者只有公钥，没有私钥，因此仍然不能解密。
	///		对称算法和非对称算法各有优缺点。非对称算法的突出优点是用于解密的密钥（私钥）永远不需要传递给对方。但是，它的缺点也很突出：非对称算法
	///	复杂，导致加密、解密速度慢，因此只适用于数据量小的场合。而对称加密加密、解密的的优点是效率高，系统开销小，适合进行大数据量的加密、解密。
	///	如果文件较大，那么最适合的加密方式就是对称加密
	/// </summary>
	internal class Program
	{
		//缓冲区大小
		private static int bufferSize = 128 * 1024;

		//密钥salt
		private static byte[] salt = { 134, 216, 7, 36, 88, 164, 91, 227, 174, 76, 191, 197, 192, 154, 200, 248 };

		//初始化向量
		private static byte[] iv = { 134, 216, 7, 36, 88, 164, 91, 227, 174, 76, 191, 197, 192, 154, 200, 248 };

		private static void Main(string[] args)
		{
			EncryptFile(@"C:\Users\Administrator\Desktop\temp.txt", @"C:\Users\Administrator\Desktop\tempcm.txt", "123");
			Console.WriteLine("加密成功！");
			DecryptFile(@"C:\Users\Administrator\Desktop\tempcm.txt", @"C:\Users\Administrator\Desktop\tempm.txt", "123");
			Console.WriteLine("解密成功！");

			Console.Read();
		}

		//初始化并返回对称加密算法
		private static SymmetricAlgorithm CreateRijndael(string password, byte[] salt)
		{
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt, "SHA256", 1000);
			SymmetricAlgorithm sma = Rijndael.Create();
			sma.KeySize = 256;
			sma.Key = pdb.GetBytes(32);
			sma.Padding = PaddingMode.PKCS7;
			return sma;
		}

		private static void EncryptFile(string inFile, string outFile, string password)
		{
			using (FileStream inFileStream = File.OpenRead(inFile), outFileStream = File.Open(outFile, FileMode.OpenOrCreate))
			using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
			{
				algorithm.IV = iv;
				using (CryptoStream cryptoStream = new CryptoStream(outFileStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
				{
					byte[] bytes = new byte[bufferSize];
					int readSize = -1;
					while ((readSize = inFileStream.Read(bytes, 0, bytes.Length)) != 0)
					{
						cryptoStream.Write(bytes, 0, readSize);
					}
					cryptoStream.Flush();
				}
			}
		}

		private static void DecryptFile(string inFile, string outFile, string password)
		{
			using (FileStream inFileStream = File.OpenRead(inFile), outFileStream = File.OpenWrite(outFile))
			using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
			{
				algorithm.IV = iv;
				using (CryptoStream cryptoStream = new CryptoStream(inFileStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read))
				{
					byte[] bytes = new byte[bufferSize];
					int readSize = -1;
					int numReads = (int)(inFileStream.Length / bufferSize);
					int slack = (int)(inFileStream.Length % bufferSize);
					for (int i = 0; i < numReads; ++i)
					{
						readSize = cryptoStream.Read(bytes, 0, bytes.Length);
						outFileStream.Write(bytes, 0, readSize);
					}
					if (slack > 0)
					{
						readSize = cryptoStream.Read(bytes, 0, (int)slack);
						outFileStream.Write(bytes, 0, readSize);
					}
					outFileStream.Flush();
				}
			}
		}
	}
}