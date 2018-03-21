using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _9.避免将泛型List作为自定义集合类的基类
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// 如果要实现一个自定义的集合类，不应该以一个FCL集合类为基类，反而应扩展相应的泛型接口。
			// FCL结合类应该以组合的形式包含至自定义的集合类，需要扩展的泛型接口通常是IEnumerable<T>和ICollection<T>（或ICollection<T>的子接口，
			// 如IList<T>），前者规范了集合类的迭代功能，后者规范了一个集合通常会有的操作。
			Employees1 employees1 = new Employees1()
            {
                new Employee(){ Name = "Mike" },
                new Employee(){ Name = "Rose" }
            };
			IList<Employee> employees = employees1;
			employees.Add(new Employee { Name = "Steve" });
			foreach (var item in employees)
			{
				Console.WriteLine(item.Name);
			}
			// 以上代码的实际输出会偏离集合类设计者的设想,输出为：Mike Changed! Rose Changed! Steve

			Employees2 employees2 = new Employees2()
            {
                new Employee(){ Name = "Mike" },
                new Employee(){ Name = "Rose" }
            };
			ICollection<Employee> employees3 = employees2;
			employees3.Add(new Employee { Name = "Steve" });
			foreach (var item in employees3)
			{
				Console.WriteLine(item.Name);
			}
			// 以上代码可以得到正确的结果
			Console.Read();
		}
	}

	internal class Employee
	{
		public string Name { get; set; }
	}

	/// <summary>
	/// 以Employees1为例，如果要在Add方法中加入某些需求方面的变化，比如，为名字添加一个后缀“Changed!"
	/// </summary>
	internal class Employees1 : List<Employee>
	{
		public new void Add(Employee item)
		{
			item.Name += "Changed！";
			base.Add(item);
		}
	}

	internal class Employees2 : ICollection<Employee>, IEnumerable<Employee>
	{
		private List<Employee> list = new List<Employee>();

		public void Add(Employee item)
		{
			item.Name += " Changed!";
			list.Add(item);
		}

		public IEnumerator<Employee> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		#region 其他方法实现省略

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(Employee item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(Employee[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { throw new NotImplementedException(); }
		}

		public bool IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		public bool Remove(Employee item)
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion 其他方法实现省略
	}
}