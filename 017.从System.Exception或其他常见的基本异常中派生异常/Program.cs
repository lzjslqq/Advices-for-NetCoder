using System;

namespace _017.从System.Exception或其他常见的基本异常中派生异常
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			try
			{
				throw new PaperEncryptException("试卷加密失败", "学生ID：123456");
			}
			catch (PaperEncryptException err)
			{
				Console.WriteLine(err.Message);
			}

			Console.Read();
		}
	}
}