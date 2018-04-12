using System;

namespace _037.使用表驱动法避免过长的if和switch分支
{
	/// <summary>
	/// 在实际场景中随着代码变得复杂，我们很容易被过长的if和switch分支困扰
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine(GetChineseWeek(Week.Saturday));

			SampleClass sample = new SampleClass();
			var addMethod = typeof(SampleClass).GetMethod(ActionInTable(Week.Monday));
			addMethod.Invoke(sample, null);

			Console.Read();
		}

		// 如果要把Week的元素值用中文输出，简单而丑陋的方法也许是封装一个GetChineseWeekTest方法:
		// 1）分支太长了，而且出现了重复代码。
		// 2）不利于扩展
		// 一种解决方案是使用多态，它很好的符合了“开闭”原则。如果增加条件分支，不必修改源代码，直接增加子类就可以了。
		private static string GetChineseWeekTest(Week week)
		{
			switch (week)
			{
				case Week.Monday:
					return "星期一";

				case Week.Tuesday:
					return "星期二";

				case Week.Wednesday:
					return "星期三";

				case Week.Thursday:
					return "星期四";

				case Week.Friday:
					return "星期五";

				case Week.Saturday:
					return "星期六";

				case Week.Sunday:
					return "星期日";

				default:
					throw new ArgumentOutOfRangeException("week", "星期值超出范围");
			}
		}

		/// <summary>
		/// 本建议要采用的是“表驱动法”,可以把表驱动简单理解为查字典.
		/// 这是一种按照索引值驱动的表驱动法。枚举元素代表的整型值，很容易和字符串数组索引结合起来，用两行语句就解决了GetChineseWeek方法
		/// 这种方法有局限性，如果需要换成：星期一Mike打扫卫生、星期二Rose清理衣柜、星期三Mike和Rose没事可以吵吵架、星期四Rose要去Shopping，也就是说需求由静态属性变成了动态行为，那么事情就会变得复杂
		/// </summary>
		/// <param name="week"></param>
		/// <returns></returns>
		private static string GetChineseWeek(Week week)
		{
			string[] chineseWeek = { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" };
			return chineseWeek[(int)week];
		}

		/// <summary>
		/// 除了多态外，使用表驱动法加上一点反射来实现这类动态的行为
		/// </summary>
		/// <param name="week"></param>
		/// <returns></returns>
		private static string ActionInTable(Week week)
		{
			string[] methods = { "Cleaning", "CleanCloset", "Quarrel", "Shopping", "Temp", "Temp", "Temp" };
			return methods[(int)week];
		}
	}

	internal enum Week
	{
		Monday,
		Tuesday,
		Wednesday,
		Thursday,
		Friday,
		Saturday,
		Sunday
	}

	internal class SampleClass
	{
		public void Cleaning()
		{
			Console.WriteLine("打扫");
		}

		public void CleanCloset()
		{
			Console.WriteLine("整理衣橱");
		}

		public void Quarrel()
		{
			Console.WriteLine("吵架");
		}

		public void Shopping()
		{
			Console.WriteLine("购物");
		}

		public void Temp()
		{
			Console.WriteLine("临时安排");
		}
	}
}