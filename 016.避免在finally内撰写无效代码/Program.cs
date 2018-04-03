using System;

namespace _016.避免在finally内撰写无效代码
{
	/// <summary>
	/// 是否存在一种打破try-finally执行顺序的情况，答案是：不存在（除非应用程序本身因为某些很少出现的特殊情况在try块中退出）。
	/// 应该始终认为finally内的代码会在方法return之前执行，哪怕return在try块中。
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			int result1 = TestIntReturnBelowFinally();
			Console.WriteLine(result1);

			int result2 = TestIntReturnInTry();
			Console.WriteLine(result2);

			var user = TestUserReturnInTry();
			Console.WriteLine(user.Name);
			Console.WriteLine(user.BirthDay);

			var u2 = TestUserReturnInTry2();
			Console.WriteLine(u2.Name);
			Console.Read();
		}

		private static int TestIntReturnBelowFinally()
		{
			int i;
			try
			{
				i = 1;
			}
			finally
			{
				i = 2;
				Console.WriteLine("\t TestIntReturnBelowFinally将int结果改为2，finally执行完毕");
			}
			return i;
		}

		private static int TestIntReturnInTry()
		{
			int i;
			try
			{
				return i = 1;
			}
			finally
			{
				i = 2;
				Console.WriteLine("\t TestIntReturnInTry将int结果改为2，finally执行完毕");
			}
		}

		private static User TestUserReturnInTry()
		{
			User user = new User { Name = "Mike", BirthDay = new DateTime(2010, 1, 1) };
			try
			{
				// User是引用类型,在finally中 user.Name = "Rose"时return的user的Name也会变为"Rose"
				return user;
			}
			finally
			{
				user.Name = "Rose";
				user.BirthDay = new DateTime(2010, 2, 2);
				Console.WriteLine("\t TestUserReturnInTry将user.Name改为Rose");
			}
		}

		private static User TestUserReturnInTry2()
		{
			// User CS$1$0000;  release版本中C#代码
			User user = new User() { Name = "Mike", BirthDay = new DateTime(2010, 1, 1) };
			try
			{
				// CS$1$0000 = user; release版本中C#代码
				return user;
			}
			finally
			{
				user.Name = "Rose";
				user.BirthDay = new DateTime(2010, 2, 2);
				// CS$1$0000和user指向的是同一个对象，当在finally中 user=null 时，只是user指向为null了，CS$1$0000指向的对象并没有变。
				user = null;
				Console.WriteLine("\t TestUserReturnInTry2将user置为null");
			}

			// return CS$1$0000; release版本中C#代码
		}
	}

	internal class User
	{
		public string Name { get; set; }

		public DateTime BirthDay { get; set; }
	}
}