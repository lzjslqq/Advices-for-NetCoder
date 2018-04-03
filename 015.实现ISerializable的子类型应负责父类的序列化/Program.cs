using System;
using System.Runtime.Serialization;

namespace _015.实现ISerializable的子类型应负责父类的序列化
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Employee liming = new Employee() { Name = "liming", Salary = 2000 };
			BinarySerializer.SerializeToFile(liming, @"C:\Users\Administrator\Desktop", "LiMing.txt");
			Employee limingCopy = BinarySerializer.DeserializeFromFile<Employee>(@"C:\Users\Administrator\Desktop\LiMing.txt");
			Console.WriteLine(string.Format("姓名：{0}", limingCopy.Name));
			Console.WriteLine(string.Format("薪水：{0}", limingCopy.Salary));

			Employee2 liming2 = new Employee2() { Name = "liming2", Salary = 20002 };
			BinarySerializer.SerializeToFile(liming2, @"C:\Users\Administrator\Desktop", "LiMing2.txt");
			Employee2 limingCopy2 = BinarySerializer.DeserializeFromFile<Employee2>(@"C:\Users\Administrator\Desktop\LiMing2.txt");
			Console.WriteLine(string.Format("姓名：{0}", limingCopy2.Name));
			Console.WriteLine(string.Format("薪水：{0}", limingCopy2.Salary));

			Console.Read();
		}
	}

	// 父类未实现ISerializable接口
	public class Person
	{
		public string Name { get; set; }
	}

	// 父类已实现ISerializable接口
	[Serializable]
	public class Person2 : ISerializable
	{
		public string Name { get; set; }

		public Person2()
		{
		}

		protected Person2(SerializationInfo info, StreamingContext context)
		{
			Name = info.GetString("Name");
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Name", Name);
		}
	}

	[Serializable]
	public class Employee : Person, ISerializable
	{
		public int Salary { get; set; }

		public Employee()
		{
		}

		protected Employee(SerializationInfo info, StreamingContext context)
		{
			Name = info.GetString("Name");
			Salary = info.GetInt32("Salary");
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// 父类字段Name的处理
			info.AddValue("Name", Name);
			info.AddValue("Salary", Salary);
		}
	}

	[Serializable]
	public class Employee2 : Person2, ISerializable
	{
		public int Salary { get; set; }

		public Employee2()
		{
		}

		protected Employee2(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Name = info.GetString("Name");
			Salary = info.GetInt32("Salary");
		}

		// Person类已经实现了ISerializable接口，在子类Employee中，只需要调用父类受保护的构造方法和GetObjectData方法就可以了
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// 父类字段Name的处理
			info.AddValue("Name", Name);
			info.AddValue("Salary", Salary);
		}
	}
}