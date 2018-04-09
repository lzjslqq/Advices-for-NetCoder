using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _023.Parallel简化但不等同于Task默认行为
{
	/// <summary>
	/// 在同步状态下简化了Task的使用。也就是说，在运行Parallel中的For、ForEach方法时，调用者线程（在示例中就是主线程）是被阻滞的。
	/// Parallel虽然将任务交给Task去处理，即交给CLR线程池去处理，不过调用者会一直等到线程池中的相关工作全部完成。
	/// 表示并行的静态类Parallel甚至只提供了Invoke方法，而没有同时提供一个BeginInvoke方法，这也从一定程度上说明了这个问题。
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			// 在使用Task时，我们最常使用的是Start方法（Task也提供了RunSynchronously），它不会阻滞调用者线程
			//Task t = new Task(() =>
			//{
			//	while (true)
			//	{
			//	}
			//});
			//t.Start();

			//在这里也可以使用Invoke方法,使用Parallel执行相近的功能，主线程被阻滞,永远不会有输出
			Parallel.For(0, 1, (i) =>
			{
				while (true)
				{
				}
			});
			Console.WriteLine("主线程即将结束");
			Console.ReadKey();
		}
	}
}