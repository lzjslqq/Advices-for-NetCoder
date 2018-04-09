using System;
using System.Threading;
using System.Threading.Tasks;

namespace _024.小心Parallel中的陷阱
{
	/// <summary>
	///		Parallel的For和ForEach方法还支持一些相对复杂的应用。在这些应用中，它允许我们在每个任务启动时执行一些初始化操作，在每个任务结束后，
	/// 又执行一些后续工作，同时，还允许我们监视任务的状态。
	///		但是，记住上面这句话“允许我们监视任务的状态”是错误的：应该把其中的“任务”改成“线程”。这，就是陷阱所在。
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			int[] nums = new int[] { 1, 2, 3, 4 };
			int total = 0;
			Parallel.For(0, nums.Length, () =>
			{
				// localInit的作用是如果Parallel为我们新起了一个线程,它会将任务体中的subtotal这个值初始化为1
				return 1;
			}, (i, loopState, subtotal) =>
			{
				subtotal += nums[i];
				// subtotal为单个任务的返回值
				return subtotal;
			}, (x) => Interlocked.Add(ref total, x));

			Console.WriteLine("total={0}", total);

			// 这段代码有可能输出11，较少的情况下输出12，虽然理论上有可能输出13和14，但是我们应该很少有机会观察到
			// Parallel一共启动了4个任务，但是我们不能确定Parallel到底为我们启动了多少个线程，那是运行时根据自己的调度算法决定的。
			// 如果所有的并发任务只用了一个线程，则输出为11；如果用了两个线程，那么根据程序的逻辑来看，输出就是12了。

			string[] stringArr = new string[] { "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh" };
			string result = string.Empty;
			Parallel.For<string>(0, stringArr.Length, () => "-", (i, loopState, subResult) =>
			{
				return subResult += stringArr[i];
			}, (threadEndString) =>
			{
				result += threadEndString;
				Console.WriteLine("Inner:" + threadEndString);
			});
			Console.WriteLine(result);

			Console.Read();
		}
	}
}