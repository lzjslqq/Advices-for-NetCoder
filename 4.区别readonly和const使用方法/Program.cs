using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4.区别readonly和const使用方法
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// 区别：1.const是编译器常量，readonly是运行时常量  2.const只能修饰基元、枚举和字符串类型(引用类型只能赋值null)，readonly没有限制。
			// static const int i = 5; const天然就是static的
			const ContextStaticAttribute p = null;

			// readonly变量是运行时变量，赋值行为发生在运行时。它在运行时第一次被赋值后将不可改变：
			// 1.对于值类型，值本身不可改变
			// 2.对于引用类型，引用本身（相当于指针）不可改变
			Sample sample = new Sample(200);
			// sample.ReadOnlyValue = 30; 无法对只读的字段赋值
			Sample2 sample2 = new Sample2(new Student());
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

	internal class Student
	{
	}
}