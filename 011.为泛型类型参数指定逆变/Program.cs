using System;

namespace _011.为泛型类型参数指定逆变
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// 逆变是指方法的参数可以是委托或者泛型接口的参数类型的基类。FCL4.0中支持逆变的常用委托有：
			// Func<int T,out TResult>、 Predicate<in T>
			// 常用委托有： IComparer<in T>

			Programmer p = new Programmer { Name = "Mike" };
			Manager m = new Manager { Name = "Steve" };
			// 由于逆变，IMyComparable<Employee>可以转化为IMyComparable<Manager>（本质上还是使用IMyComparable<Employee>）
			Test(p, m);

			Console.Read();
		}

		private static void Test<T>(IMyComparable<T> t1, T t2)
		{
			Console.WriteLine(t1.Compare(t2));
		}
	}

	public interface IMyComparable<in T>
	{
		int Compare(T other);
	}

	public class Employee : IMyComparable<Employee>
	{
		public string Name { get; set; }

		public int Compare(Employee other)
		{
			Console.WriteLine("Employee's method");
			return Name.CompareTo(other.Name);
		}
	}

	public class Programmer : Employee, IMyComparable<Programmer>
	{
		public int Compare(Programmer other)
		{
			Console.WriteLine("Programmer's method");
			return Name.CompareTo(other.Name);
		}
	}

	public class Manager : Employee
	{
	}
}