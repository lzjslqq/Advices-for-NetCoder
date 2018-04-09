using System;
using System.Collections.Generic;
using System.Linq;

namespace _025.使用PLINQ
{
	/// <summary>
	/// 传统的LINQ计算是单线程的，PLINQ则是并发的、多线程的
	/// </summary>
	internal class Program
	{
		// 建议在对集合中的元素项进行操作的时候使用PLINQ代替LINQ。但是要记住，不是所有并行查询的速度都会比顺序查询快，
		// 在对集合执行某些方法时，顺序查询的速度会更快一点，如方法ElementAt等。在开发中，我们应该仔细辨别这方面的需求，以便找到最佳的解决方案。
		private static void Main(string[] args)
		{
			List<int> intList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var query = from p in intList select p;
			Console.WriteLine("以下是LINQ顺序输出：");
			foreach (int item in query)
			{
				Console.WriteLine(item.ToString());
			}

			Console.WriteLine("以下是PLINQ并行输出：");
			var queryParallel = from p in intList.AsParallel() select p;
			// 在ForAll方法中，它所完成的输出仍是无序的
			queryParallel.ForAll((item) =>
			{
				Console.WriteLine(item.ToString());
			});

			Console.Read();
		}
	}
}