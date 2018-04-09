using System;
using System.Threading.Tasks;

namespace _026.Task中的异常处理
{
	/// <summary>
	/// 在任务并行库中，如果对任务运行Wait、WaitAny、WaitAll等方法，或者求Result属性，都能捕获到AggregateException异常。
	/// 可以将AggregateException异常看做是任务并行库编程中最上层的异常。在任务中捕获的异常，最终都应该包装到AggregateException中。
	/// </summary>
	internal class Program
	{
		private static event EventHandler<AggregateExceptionArgs> AggregateExceptionCatched;

		private static void Main(string[] args)
		{
			// 一个任务并行库异常的简单处理示例如下:

			#region 方法1

			// 新任务只完成了处理异常，这意味着新任务不会延续较长时间，所以，在这个新任务上维持等待对于调用者来说，是可以忍受的。
			// 所以，我们可以采用这个方法将异常包装到主线程
			Task t = new Task(() =>
				{
					throw new Exception("任务并行编码中产生的未知异常");
				});
			t.Start();

			Task tEnd = t.ContinueWith((task) =>
			{
				foreach (var item in task.Exception.InnerExceptions)
				{
					Console.WriteLine("异常类型：{0}{1}来自：{2}{3}异常内容：{4}", item.GetType(), Environment.NewLine, item.Source, Environment.NewLine, item.Message);
				}
			}, TaskContinuationOptions.OnlyOnFaulted);

			try
			{
				tEnd.Wait();
			}
			catch (AggregateException e)
			{
				foreach (var item in e.InnerExceptions)
				{
					Console.WriteLine("异常类型：{0}{1}来自：{2}{3}异常内容：{4}", item.GetType(), Environment.NewLine, item.Source, Environment.NewLine, item.Message);
				}
			}

			#endregion 方法1

			#region 方法2

			// 对线程调用Wait方法（或者求Result）不是最好的办法，因为它会阻滞主线程，并且CLR在后台会新起线程池线程来完成额外的工作。
			// 如果要包装异常到主线程，另外一个方法就是使用事件通知的方式
			AggregateExceptionCatched += new EventHandler<AggregateExceptionArgs>(Program_AggregateExceptionCatched);
			Task t2 = new Task(() =>
			{
				try
				{
					throw new InvalidOperationException("任务并行编码2中产生的未知异常");
				}
				catch (Exception err)
				{
					AggregateExceptionArgs errArgs = new AggregateExceptionArgs() { AggregateException = new AggregateException(err) };
					AggregateExceptionCatched(null, errArgs);
				}
			});

			t2.Start();

			#endregion 方法2

			Console.WriteLine("主线程马上结束");

			Console.ReadKey();
		}

		private static void Program_AggregateExceptionCatched(object sender, AggregateExceptionArgs e)
		{
			foreach (var item in e.AggregateException.InnerExceptions)
			{
				Console.WriteLine("异常类型：{0}{1}来自：{2}{3}异常内容：{4}", item.GetType(), Environment.NewLine, item.Source, Environment.NewLine, item.Message);
			}
		}
	}

	public class AggregateExceptionArgs : EventArgs
	{
		public AggregateException AggregateException { get; set; }
	}
}