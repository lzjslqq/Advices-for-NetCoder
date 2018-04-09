using System;

namespace _018.应使用finally避免资源泄漏
{
	internal class Program
	{
		/// <summary>
		///	除非发生让应用程序中断的异常，否则finally总是会先于return执行。finally的这个语言特性决定了资源释放的最佳位置就是在finally块中；
		/// </summary>
		/// <param name="args"></param>
		private static void Main(string[] args)
		{
			// 资源释放会随着调用堆栈由下往上执行。下面的代码验证了这一点
			Method1();

			// finally不会因为调用堆栈中存在的异常而被终止，CLR会先执行catch块，然后再执行finally块。如下：
			Method3();

			Console.Read();
		}

		private static void Method1()
		{
			ClassShouldDisposeBase c = null;
			try
			{
				c = new ClassShouldDisposeBase("Method1");
				Method2();
			}
			finally
			{
				c.Dispose();
			}
		}

		private static void Method2()
		{
			ClassShouldDisposeBase c = null;
			try
			{
				c = new ClassShouldDisposeBase("Method2");
			}
			finally
			{
				c.Dispose();
			}
		}

		private static void Method3()
		{
			ClassShouldDisposeBase c = null;
			try
			{
				c = new ClassShouldDisposeBase("Method3");
				Method4();
			}
			catch
			{
				Console.WriteLine("在Method3中捕获了异常。");
			}
			finally
			{
				c.Dispose();
			}
		}

		private static void Method4()
		{
			ClassShouldDisposeBase c = null;
			try
			{
				c = new ClassShouldDisposeBase("Method4");
				throw new Exception();
			}
			catch
			{
				Console.WriteLine("在Method4中捕获了异常。");
				throw;
			}
			finally
			{
				c.Dispose();
			}
		}
	}
}