using System;

namespace _029.避免在构造方法中调用虚成员
{
	internal class Program
	{
		/// <summary>
		/// 调用者代码中，我们需要创建一个American的实例对象american。由于发现实例还存在一个基类Person，所以运行时会首先调用基类的构造方法。
		/// 在构造方法中Person调用了虚方法InitSkin。由于是虚方法，所以会在运行时调用子类的InitSkin方法。子类的InitSkin方法中，需要打印出名字。
		/// 而这个时候，方法的调用堆栈还一直在基类的构造方法内，也就是在子类的构造方法中的代码还完全没有执行：Race = new Race() { Name = "White" };
		/// </summary>
		/// <param name="args"></param>
		private static void Main(string[] args)
		{
			American american = new American();
			Console.ReadKey();
		}
	}

	internal class Person
	{
		public Person()
		{
			InitSkin();
		}

		protected virtual void InitSkin()
		{
			//省略
		}
	}

	internal class American : Person
	{
		private Race Race;

		public American()
			: base()
		{
			Race = new Race() { Name = "White" };
		}

		protected override void InitSkin()
		{
			Console.WriteLine(Race.Name);
		}
	}

	internal class Race
	{
		public string Name { get; set; }
	}
}