using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace _027.Parallel中的异常处理
{
	/// <summary>
	/// 由于Task的Start方法是异步启动的，所以我们需要额外的技术来完成异常处理。
	/// Parallel相对来说就要简单很多，因为Parallel的调用者线程会等到所有的任务全部完成后，再继续自己的工作。简单来说，它具有同步的特性.
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			// 用下面的这段代码就可以实现将并发异常包装到主线程中:
			try
			{
				var parallelExceptions = new ConcurrentQueue<Exception>();
				Parallel.For(0, 2, (i) =>
				{
					try
					{
						throw new InvalidOperationException("并行任务中出现的异常");
					}
					catch (Exception e)
					{
						parallelExceptions.Enqueue(e);
					}

					if (parallelExceptions.Count > 0)
						throw new AggregateException(parallelExceptions);
				});
			}
			catch (AggregateException err)
			{
				foreach (Exception item in err.InnerExceptions)
				{
					Console.WriteLine("异常类型：{0}{1}来自：{2}{3}异常内容：{4}", item.InnerException.GetType(), Environment.NewLine, item.InnerException.Source, Environment.NewLine, item.InnerException.Message);
				}
			}

			Console.WriteLine("主线程马上结束");
			Console.ReadKey();
		}
	}
}