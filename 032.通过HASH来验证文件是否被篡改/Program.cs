using System;
using System.IO;
using System.Security.Cryptography;

namespace _032.通过HASH来验证文件是否被篡改
{
	/// <summary>
	/// MD5算法作为一种最通用的HASH算法，也被广泛用于文件完整性的验证上。
	/// 文件通过MD5-HASH算法求值，总能得到一个固定长度的MD5值。
	/// 虽说MD5是一种压缩算法，以致可能存在多个样本空间会得到相同目标字符串的情况，但是这种概率很小。
	/// 一个1GB的文件，哪怕只改动1字节的内容，得到的MD5值也会完全不同
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			string hash = GetFileHash(@"C:\Users\Administrator\Desktop\nginx命令.txt");
			Console.WriteLine(hash);

			Console.Read();
		}

		private static string GetFileHash(string filePath)
		{
			using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
			using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return BitConverter.ToString(md5.ComputeHash(fs)).Replace("-", "");
			}
		}
	}
}