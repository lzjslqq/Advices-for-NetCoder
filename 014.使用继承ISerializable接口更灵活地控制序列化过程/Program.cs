using System;
using System.Runtime.Serialization;

namespace _014.使用继承ISerializable接口更灵活地控制序列化过程
{
	/// <summary>
	/// 接口ISerializable的意义在于，如果特性Serializable，以及与其像配套的OnDeserializedAttribute、OnDeserializingAttribute、OnSerializedAttribute、OnSerializingAttribute、NoSerializable等特性不能完全满足自定义序列化的要求，那就需要继承ISerializable了
	/// </summary>
	internal class Program
	{
		/// <summary>
		/// 格式化器的工作流程：如果格式化器在序列化一个对象的时候，发现对象继承了ISerializable接口，那它就会忽略掉类型所有的序列化特性，转而调用类型的GetObjectData方法来构造一个SerializationInfo对象，方法内部负责向这个对象添加所有需要序列化的字段（“添加”这个词可能不太恰当，因为我们在添加前可以随意处置这个字段）
		/// </summary>
		/// <param name="args"></param>
		private static void Main(string[] args)
		{
			Person liming = new Person() { FirstName = "Ming", LastName = "Li" };
			BinarySerializer.SerializeToFile(liming, @"C:\Users\Administrator\Desktop", "LiMing.txt");
			Person p = BinarySerializer.DeserializeFromFile<Person>(@"C:\Users\Administrator\Desktop\LiMing.txt");
			Console.WriteLine(p.FirstName);
			Console.WriteLine(p.LastName);
			Console.WriteLine(p.ChineseName);

			People people = new People() { FirstName = "Hong", LastName = "Li" };
			BinarySerializer.SerializeToFile(people, @"C:\Users\Administrator\Desktop", "LiHong.txt");
			AnotherPeople another = BinarySerializer.DeserializeFromFile<AnotherPeople>(@"C:\Users\Administrator\Desktop\LiHong.txt");
			Console.WriteLine(another.Name);

			Console.Read();
		}
	}

	[Serializable]
	internal class Person : ISerializable
	{
		public string FirstName;
		public string LastName;
		public string ChineseName;

		public Person()
		{
		}

		/// <summary>
		/// 带参数的构造方法用于处理反序列化。虽然在接口中没有地方指出需要这样一个构造器，但这确实是需要的，否则我们序列化后无法把它反序列化回来。
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public Person(SerializationInfo info, StreamingContext context)
		{
			FirstName = info.GetString("firstname");
			LastName = info.GetString("LastName");
			ChineseName = string.Format("{0} {1}", LastName, FirstName);
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("firstname", FirstName);
			info.AddValue("LastName", LastName);
		}
	}

	[Serializable]
	internal class People : ISerializable
	{
		public string FirstName;
		public string LastName;

		public People()
		{
		}

		protected People(SerializationInfo info, StreamingContext context)
		{
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// 负责告诉序列化器：我要被反序列化为PersonAnother
			info.SetType(typeof(AnotherPeople));
			info.AddValue("Name", string.Format("{0} {1}", LastName, FirstName));
		}
	}

	[Serializable]
	internal class AnotherPeople : ISerializable
	{
		public string Name { get; set; }

		protected AnotherPeople(SerializationInfo info, StreamingContext context)
		{
			Name = info.GetString("Name");
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}
	}
}