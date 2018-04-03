using System;
using System.Runtime.Serialization;

namespace _013.为无用字段标注不可序列化
{
	internal class Program
	{
		/// <summary>
		/// 序列化是指这样一种技术：把对象转变成流。相反的过程，我们称为反序列化。在很多场合都需要用到这项技术:
		/// 1.把对象保存到本地，在下次运行程序的时候，恢复这个对象
		/// 2.把对象传到网络中的另外一台终端上，然后在此终端还原这个对象
		/// 3.其他场合，如：把对象赋值到系统的粘贴板中，然后用快捷键Ctrl+V恢复这个对象
		/// </summary>
		/// <param name="args"></param>
		private static void Main(string[] args)
		{
			Person person = new Person { Age = 20, Name = "Mike", Department = new Department { No = 123, Name = "dep1" } };
			person.NameChanged += person_NameChanged;
			BinarySerializer.SerializeToFile(person, @"C:\Users\Administrator\Desktop", "person.txt");
			var p = BinarySerializer.DeserializeFromFile<Person>(@"C:\Users\Administrator\Desktop\person.txt");
			Console.WriteLine(p.Name);

			p.Name = "new Name";
			Console.WriteLine(p.Name);
			// Console.WriteLine(p.Department.Name);
			Console.WriteLine(p.Age.ToString());

			Department department = new Department { No = 1212, CompanyName = "MyCompany", Name = "Financial Department" };
			BinarySerializer.SerializeToFile(department, @"C:\Users\Administrator\Desktop", "department.txt");
			var d = BinarySerializer.DeserializeFromFile<Department>(@"C:\Users\Administrator\Desktop\department.txt");
			Console.WriteLine(d.ToString());

			TestA a = new TestA { No_A = 1, Testb = new TestB { No_B = 2 } };

			Console.Read();
		}

		private static void person_NameChanged(object sender, EventArgs e)
		{
			Console.WriteLine("Name Changed");
		}
	}

	[Serializable]
	internal class Person
	{
		private string name;

		public string Name
		{
			get { return name; }
			set
			{
				if (NameChanged != null)
				{
					NameChanged(this, null);
				}

				name = value;
			}
		}

		public int Age { get; set; }

		[NonSerialized]
		private Department department;

		public Department Department
		{
			get { return department; }
			set { department = value; }
		}

		[field: NonSerialized]
		public event EventHandler NameChanged;
	}

	[Serializable]
	internal class Department
	{
		public int No { get; set; }
		public string Name { get; set; }
		public string CompanyName { get; set; }

		[field: NonSerialized]
		public string CustomName { get; set; }

		/// <summary>
		/// 特性（attribute）可以声明式地为代码中的目标元素添加注释。运行时可以通过查询这些托管块中的元数据信息，达到改变目标元素运行时行为的目的
		/// OnDeserializedAttribute、OnDeserializingAttribute、OnSerializedAttribute、OnSerializingAttribute
		/// 利用这些特性，可以更加灵活地处理序列化和反序列化。例如，我们可以利用这一点，进一步减少某些可序列化的字段
		/// </summary>
		/// <param name="context"></param>
		[OnDeserializedAttribute]
		private void OnSerialized(StreamingContext context)
		{
			CustomName = string.Format("CompanyName:{0}'s {1}", CompanyName, Name);
		}

		public override string ToString()
		{
			return string.Format("==={0} {1} {2} {3}===", No, Name, CompanyName, CustomName);
		}
	}

	[Serializable]
	internal class TestA
	{
		public int No_A { get; set; }
		public TestB Testb { get; set; }
	}

	[Serializable]
	internal class TestB
	{
		public int No_B { get; set; }
		public TestA TestA { get; set; }
	}
}