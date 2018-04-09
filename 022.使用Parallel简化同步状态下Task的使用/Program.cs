using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _022.使用Parallel简化同步状态下Task的使用
{
	/// <summary>
	/// 静态类Parallel简化了在同步状态下的Task的操作。Parallel主要提供3个有用的方法：For、ForEach、Invoke
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			// 1.For方法主要用于处理针对数组元素的并行操作:
			int[] nums = new int[] { 1, 2, 3, 4, 5 };
			Parallel.For(0, nums.Length, (i) =>
			{
				Console.WriteLine("针对数组索引{0}对应的那个元素{1}的一些工作代码……", i, nums[i]);
			});

			// 2.ForEach方法主要用于处理泛型集合元素的并行操作:
			List<int> list = new List<int> { 1, 2, 3, 4, 5 };
			Parallel.ForEach(list, (item) =>
			{
				Console.WriteLine("针对集合元素{0}的一些工作代码……", item);
			});

			// 3.Parallel的Invoke方法为我们简化了启动一组并行操作，它隐式启动的就是Task
			Parallel.Invoke(() =>
			{
				Console.WriteLine("任务1……");
			}, () =>
			{
				Console.WriteLine("任务2……");
			}, () =>
			{
				Console.WriteLine("任务3……");
			});

			Console.Read();
		}
	}
}