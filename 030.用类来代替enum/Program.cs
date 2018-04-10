using System;

namespace _030.用类来代替enum
{
	/// <summary>
	/// 枚举最大的优点在于它的类型是值类型。相比较引用类型来说，它可以在关键算法中提升性能，因为它不需要创建在“堆”中。
	/// 但是，如果不考虑这方面的因素，我们不妨让类（引用类型）来代替枚举。
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine(Week.Monday);
			Console.WriteLine(Week.Sunday);
			Console.WriteLine(Week.Saturday);

			Console.Read();
			//相比枚举而言，类能赋予类型更多的行为。当然，如果应用场合满足如下特性，我们就应该更多的考虑使用枚举：
			//效率。这源于枚举是值类型。
			//类型用于内部，不需要增加更多的行为属性。
			//类型元素不需要提供附加的特性。
		}
	}

	/// <summary>
	/// 类Week相比枚举Week的优点在于，它能够添加方法或重写基类方法，以便提供丰富的功能。
	/// 以星期为例，如果要提供更有意义的字符串，如指定Monday是星期一，对于枚举来说，这并不是天然支持的
	/// </summary>
	internal class Week
	{
		public static readonly Week Monday = new Week(0);
		public static readonly Week Tuesday = new Week(1);
		public static readonly Week Wednesday = new Week(2);
		public static readonly Week Thursday = new Week(3);
		public static readonly Week Friday = new Week(4);
		public static readonly Week Saturday = new Week(5);
		public static readonly Week Sunday = new Week(6);
		private int _infoType;

		/// <summary>
		/// 将类型Week的构造方法实现为private，这有效阻止了类型在外部生成类的实例，使它的行为更接近于枚举
		/// </summary>
		/// <param name="infoType"></param>
		private Week(int infoType)
		{
			_infoType = infoType;
		}

		public override string ToString()
		{
			switch (_infoType)
			{
				case 0:
					return "星期一";

				case 1:
					return "星期二";

				case 2:
					return "星期三";

				case 3:
					return "星期四";

				case 4:
					return "星期五";

				case 5:
					return "星期六";

				case 6:
					return "星期日";

				default:
					throw new Exception("不正确的星期信息！");
			}
		}
	}
}