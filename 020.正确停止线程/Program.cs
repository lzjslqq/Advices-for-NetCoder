using System;
using System.Threading;

namespace _020.正确停止线程
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			int num = 0;
			CancellationTokenSource cts = new CancellationTokenSource();
			// 在线程停止的时候被回调
			cts.Token.Register(() =>
			{
				Console.WriteLine("工作线程被终止了。");
			});

			Thread t = new Thread(() =>
			{
				while (true)
				{
					if (cts.Token.IsCancellationRequested)
					{
						Console.WriteLine("线程被终止！");
						break;
					}
					Console.WriteLine(DateTime.Now.ToString());
					Thread.Sleep(1000);
					num++;
					if (num == 10)
					{
						cts.Cancel();
					}
				}
			});

			t.Start();
			Console.ReadLine();
		}
	}
}