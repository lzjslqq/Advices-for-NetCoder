using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10.小心闭包中的陷阱
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			List<Action> list = new List<Action>();
			for (int i = 0; i < 5; i++)
			{
				Action a = () =>
				{
					Console.WriteLine(i.ToString());
				};

				list.Add(a);
			}

			foreach (Action t in list)
			{
				t();
			}

			// 以上代码我们本意是想得到输出的效果为：0 1 2 3 4，然而实际输出却是：5 5 5 5 5。
			// 这段代码演示的就是闭包对象。如果匿名方法（lambda表达式）引用了某个局部变量，编译器就会自动将该引用提升到闭包对象中，即将for循环中的变量 i 修改成了引用闭包对象的公共变量 i 。这样，即使代码执行离开了原局部变量 i 的作用域（如for循环），包含该闭包对象的作用域还存在。
			// 以上代码和以下代码实际上一致：
			TempClass tempClass = new TempClass();
			for (tempClass.i = 0; tempClass.i < 5; tempClass.i++)
			{
				Action t = tempClass.TempFunc;
				list.Add(t);
			}
			foreach (Action t in list)
			{
				t();
			}
			// 要实现本建议开始时所预期的输出，可以将闭包对象的产生放在for循环内部：
			List<Action> list2 = new List<Action>();
			for (int i = 0; i < 5; i++)
			{
				int temp = i;
				Action a = () =>
				{
					Console.WriteLine(temp.ToString());
				};

				list2.Add(a);
			}

			foreach (Action t in list2)
			{
				t();
			}
			Console.Read();
		}
	}

	internal class TempClass
	{
		public int i;

		public void TempFunc()
		{
			Console.WriteLine(i.ToString());
		}
	}
}