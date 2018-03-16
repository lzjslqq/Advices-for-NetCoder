using System;

namespace _1.使用默认转型方法
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// 1.使用类型的转换运算符（隐式转换和显示转换）
			// 基元类型普遍提供了转换运算符
			int i = 0;
			float j = 0;
			j = i;			// 隐式转换
			i = (int)j;		// 显示转换

			// 用户自定义类型可以通过重载转换运算符的方式进行转换
			IP ip = "192.168.0.2";
			Console.WriteLine(ip.ToString());

			// 2.使用类型内置的Parse、TryParse和ToString、ToDouble、ToDateTime等方法。
			int k = int.Parse("66");
			Console.WriteLine(k);

			// 3.使用帮助类提供的方法如System.Convert、System.BitConverter类
			double d = Convert.ToDouble("52.36");
			Console.WriteLine(d);

			Console.Read();
		}
	}
}