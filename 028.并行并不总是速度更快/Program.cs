using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace _028.并行并不总是速度更快
{
	/// <summary>
	/// 并行所带来的后台任务及任务的管理，都会带来一定的开销，如果一项工作本来就能很快完成，或者说循环体很小，那么并行的速度也许会比非并行要慢。
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			Stopwatch watch = new Stopwatch();
			watch.Start();
			DoInFor();
			watch.Stop();

			Console.WriteLine("同步耗时：{0}", watch.Elapsed);

			watch.Restart();
			DoInParalleFor();
			watch.Stop();
			Console.WriteLine("并行耗时：{0}", watch.Elapsed);

			// 将循环改为10000000次，循环体需要做更多工作的时候，我们发现，同步需要比并行慢完成了工作。

			object syncObj = new object();
			SampleClass sample = new SampleClass();
			Parallel.For(0, 10000000, (i) =>
			{
				lock (syncObj)
				{
					sample.SimpleAdd();
				}
			});
			Console.WriteLine(sample.SomeCount);

			Console.ReadKey();
		}

		private static void DoInFor()
		{
			for (int i = 0; i < 10000000; i++)
			{
				DoSomething();
			}
		}

		private static void DoSomething()
		{
			for (int i = 0; i < 10; i++)
			{
				i++;
			}
		}

		private static void DoInParalleFor()
		{
			Parallel.For(0, 10000000, (i) =>
			{
				DoSomething();
			});
		}
	}

	internal class SampleClass
	{
		public long SomeCount { get; private set; }

		public void SimpleAdd()
		{
			SomeCount++;
		}
	}
}