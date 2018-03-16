using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3.TryParse比Parse好
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// 除string以外所有的基元类型都有两个将字符串转型为本身的方法：Parse和TryParse。
			//	   以double为例，两者最大的区别是如果字符串不满足转换要求，Parse会引发异常；TryParse不会引发异常，并返回false，
			// 同时将result置为0。

			double re;
			long ticks;
			// Parse转换成功
			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < 1000; i++)
			{
				try
				{
					re = double.Parse("123");
				}
				catch
				{
					re = 0;
				}
			}
			sw.Stop();
			ticks = sw.ElapsedTicks;
			Console.WriteLine("Parse转换成功耗时：{0}", ticks);

			// TryParse转换成功
			sw = Stopwatch.StartNew();
			for (int i = 0; i < 1000; i++)
			{
				if (!double.TryParse("123", out re))
				{
					re = 0;
				}
			}
			sw.Stop();
			ticks = sw.ElapsedTicks;
			Console.WriteLine("TryParse转换成功耗时：{0}", ticks);

			// Parse转换失败
			sw = Stopwatch.StartNew();
			for (int i = 0; i < 1000; i++)
			{
				try
				{
					re = double.Parse("aaa");
				}
				catch
				{
					re = 0;
				}
			}
			sw.Stop();
			ticks = sw.ElapsedTicks;
			Console.WriteLine("Parse转换失败耗时：{0}", ticks);

			// TryParse转换失败
			sw = Stopwatch.StartNew();
			for (int i = 0; i < 1000; i++)
			{
				if (!double.TryParse("aaa", out re))
				{
					re = 0;
				}
			}
			sw.Stop();
			ticks = sw.ElapsedTicks;
			Console.WriteLine("TryParse转换失败耗时：{0}", ticks);

			Console.Read();

			// 如果转换成功，二者效率在一个量级上，甚至TryParse还要高效一些。若转换失败，Parse执行效率则远不如TryParse。
		}
	}
}