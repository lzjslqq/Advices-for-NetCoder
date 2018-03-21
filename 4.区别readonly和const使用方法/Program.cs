using System;

namespace _4.区别readonly和const使用方法
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// 区别：1.const是编译器常量，readonly是运行时常量  2.const只能修饰基元、枚举和字符串类型(引用类型只能赋值null)，readonly没有限制。
			// static const int i = 5; const天然就是static的
			const Sample p = null;

			// readonly变量是运行时变量，赋值行为发生在运行时。它在运行时第一次被赋值后将不可改变：
			// 1.对于值类型，值本身不可改变
			// 2.对于引用类型，引用本身（相当于指针）不可改变
			Sample sample = new Sample(200);
			// sample.ReadOnlyValue = 30; 无法对只读的字段赋值
			Sample2 sample2 = new Sample2(new Student() { Age = 10 });
			// sample2.ReadOnlyValue = new Student(){ Age = 18 }; 报错
			// 引用不可改变，引用所指向的实例的值可以改变
			sample2.ReadOnlyValue.Age = 20;

			// readonly不能被重新赋值的说法是错误的
			Sample3 sample3 = new Sample3(200);
			Console.WriteLine(sample3.ReadOnlyValue);

			Console.Read();
		}
	}

	internal class Sample
	{
		public readonly int ReadOnlyValue;

		public Sample(int value)
		{
			ReadOnlyValue = value;
		}
	}

	internal class Sample2
	{
		public readonly Student ReadOnlyValue;

		public Sample2(Student value)
		{
			ReadOnlyValue = value;
		}
	}

	internal class Sample3
	{
		// 事实上应把初始化器理解为构造方法的一部分，其实是个语法糖。在构造方法内可以对readonly变量多次赋值。
		public readonly int ReadOnlyValue = 100;

		public Sample3(int value)
		{
			ReadOnlyValue = value;
		}
	}

	internal class Student
	{
		public int Age { get; set; }
	}
}