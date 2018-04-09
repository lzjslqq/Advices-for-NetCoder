using System;
using System.Threading;
using System.Threading.Tasks;

namespace _021.用Task代替ThreadPool
{
	/// <summary>
	/// ThreadPool相对于Thread来说具有很多优势，但是ThreadPool在使用上却存在一定的不方便。比如：
	/// ThreadPool不支持线程的取消、完成、失败通知等交互性操作。
	/// ThreadPool不支持线程执行的先后次序
	/// Task在线程池的基础上进行了优化，并提供了更多的API
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			Task t = new Task(() =>
			{
				Console.WriteLine("任务开始工作···");
				// 模拟工作过程
				Thread.Sleep(3000);
			});
			//t.Start();

			// 任务并没有提供回调事件来通知完成（像BackgroundWorker一样），它是通过启用一个新任务的方式来完成类似的功能。
			// ContinueWith方法可以在一个任务完成的时候发起一个新任务，这种方式天然就支持了任务的完成通知：我们可以在新任务中获取原任务的结果值。
			//t.ContinueWith((task) =>
			//{
			//	Console.WriteLine("任务完成，完成时候的状态为：");
			//	Console.WriteLine("IsCanceled={0}\tIsCompleted={1}\tIsFaulted={2}", task.IsCanceled, task.IsCompleted, task.IsFaulted);
			//});

			CancellationTokenSource cts = new CancellationTokenSource();
			Task<int> t1 = new Task<int>(() => AddCancleByThrow(cts.Token), cts.Token);
			t1.Start();
			t1.ContinueWith(TaskEndedByCatch);

			TaskFactory taskFactory = new TaskFactory();
			Task[] tasks = new Task[]
			{
				taskFactory.StartNew(() => Add(cts.Token)),
				taskFactory.StartNew(() => Add(cts.Token)),
				taskFactory.StartNew(() => Add(cts.Token))
			};
			// CancellationToken.None指示TasksEnded不能被取消
			taskFactory.ContinueWhenAll(tasks, TasksEnded, CancellationToken.None);
			// 等待按任意键取消任务
			Console.ReadKey();
			cts.Cancel();
			Console.ReadKey();
		}

		private static void TasksEnded(Task[] tasks)
		{
			Console.WriteLine("所有任务已完成！");
		}

		private static void TaskEnded(Task<int> task)
		{
			Console.WriteLine("任务完成，完成时候的状态为：");
			Console.WriteLine("IsCanceled={0}\tIsCompleted={1}\tIsFaulted={2}", task.IsCanceled, task.IsCompleted, task.IsFaulted);
			Console.WriteLine("任务的返回值为：{0}", task.Result);
		}

		private static int Add(CancellationToken ct)
		{
			Console.WriteLine("任务开始……");
			int result = 0;
			while (!ct.IsCancellationRequested)
			{
				result++;
				Thread.Sleep(1000);
			}
			return result;
		}

		private static void TaskEndedByCatch(Task<int> task)
		{
			Console.WriteLine("任务完成，完成时候的状态为：");
			Console.WriteLine("IsCanceled={0}\tIsCompleted={1}\tIsFaulted={2}", task.IsCanceled, task.IsCompleted, task.IsFaulted);
			try
			{
				Console.WriteLine("任务的返回值为：{0}", task.Result);
			}
			catch (AggregateException e)
			{
				// 任务是通过ThrowIfCancellation Requested方法结束的，对任务求结果值将会抛出异常OperationCanceledException，而不是得到抛出异常前的结果值。这意味着任务是通过异常的方式被取消掉的，所以可以注意到上面代码的输出中，状态IsCanceled为True
				e.Handle((err) => err is OperationCanceledException);
			}
		}

		private static int AddCancleByThrow(CancellationToken ct)
		{
			Console.WriteLine("任务开始……");
			int result = 0;
			while (true)
			{
				//ct.ThrowIfCancellationRequested();
				if (result == 5)
				{
					throw new Exception("error");
				}
				result++;
				Thread.Sleep(1000);
			}
			return result;
		}
	}
}