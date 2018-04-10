using System;
using System.Security.Cryptography;
using System.Text;

namespace _031.MD5不再安全
{
	/// <summary>
	/// MD5不再安全不是就算法本身而言的。如果从可逆性的角度出发，MD5值不存在被破解的可能性
	/// 现在，已经有很多免费的商业的MD5字典库，存储了相当数量字符串的MD5值，只要提交一个MD5值进去，立刻就可以得到它的原文，只要这个原文不是非常复杂。所以，从这方面来说，MD5不再安全
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			// 在改进后的方法中，我们首先设计了一个足够复杂的密码hashKey，然后将它的MD5值和用户输入密码的MD5值相加，再求一次MD5值作为返回值。
			// 经过这个过程以后，密码的长度就够了，复杂度也够了，要想通过穷举法来得到真正的密码成本也就大大增加了
			string s = GetMd5Hash("wstclqqasdfjlk234");
			Console.WriteLine(s);

			Console.Read();
		}

		private static string GetMd5Hash(string input)
		{
			string hashKey = "Aa1@#$,.Klj+{>.45oP";
			using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
			{
				string hashCode = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(input))).Replace("-", "") + BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(hashKey))).Replace("-", "");
				return BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(hashCode))).Replace("-", "");
			}
		}
	}
}