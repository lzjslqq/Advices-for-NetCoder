using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace _8.确保集合的线程安全
{
	internal class Program
	{
		// 集合线程安全是指多个线程上添加或删除元素时，线程键必须保持同步
		private static List<Person> list = new List<Person>()
		{
			new Person() { Name = "Rose", Age = 19 },
            new Person() { Name = "Steve", Age = 45 },
            new Person() { Name = "Jessica", Age = 20 }
		};

		private static ArrayList list2 = new ArrayList()
        {
            new Person() { Name = "Rose2", Age = 22 },
            new Person() { Name = "Steve2", Age = 35 },
            new Person() { Name = "Jessica2", Age = 40 },
        };

		private static AutoResetEvent autoSet = new AutoResetEvent(false);
		private static object sycObj = new object();

		private static void Main(string[] args)
		{
			Thread t1 = new Thread(() =>
			{
				// 确保等t2开始后才运行以下代码
				autoSet.WaitOne();
				//lock (sycObj)
				//{
				foreach (var item in list)
				{
					Console.WriteLine("t1:" + item.Name);
					Thread.Sleep(1000);
				}
				//}
			});

			t1.Start();
			Thread t2 = new Thread(() =>
			{
				// 通知t1可以执行代码
				autoSet.Set();

				//沉睡1秒是为了确保删除操作在t1的迭代过程中
				Thread.Sleep(1000);
				//lock (sycObj)
				//{
				list.RemoveAt(2);
				Console.WriteLine("t2删除成功");
				//}
			});
			t2.Start();

			// 以上代码运行过程会抛出InvalidOperationException：“集合已修改，可能无法执行枚举。”

			// 早在泛型集合出现之前，非泛型集合一般提供一个SyncRoot属性，要保证非泛型集合的线程安全，可以通过锁定该属性来实现。
			// 如果上面的集合用ArrayList代替，保证其线程安全则应该在迭代和删除的时候都加上lock
			Thread t3 = new Thread(() =>
			{
				// 确保等t4开始后才运行以下代码
				autoSet.WaitOne();
				lock (list2.SyncRoot)
				{
					foreach (Person item in list2)
					{
						Console.WriteLine("t3:" + item.Name);
						Thread.Sleep(1000);
					}
				}
			});

			t3.Start();
			Thread t4 = new Thread(() =>
			{
				// 通知t3可以执行代码
				autoSet.Set();

				//沉睡1秒是为了确保删除操作在t3的迭代过程中
				Thread.Sleep(1000);
				lock (list2.SyncRoot)
				{
					list2.RemoveAt(2);
					Console.WriteLine("t4删除成功");
				}
			});
			t4.Start();
			// 以上代码不会抛出异常，因为锁定通过互斥的机制保证了同一时刻只能有一个线程操作集合元素。
			// 泛型集合没有这样的属性，必须要自己创建一个锁定对象来完成同步任务。可以通过new一个静态对象来进行锁定

			Console.Read();
		}
	}

	internal class Person
	{
		public string Name { get; set; }
		public int Age { get; set; }
	}
}