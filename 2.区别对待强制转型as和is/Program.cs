using System;

namespace _2.区别对待强制转型as和is
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// 1.FirstType与SecondType依靠转换操作符进行转型时
			FirstType firstType = new FirstType { Name = "firstType's Name" };
			SecondType secondType = (SecondType)firstType;	// 转换成功
			// secondType = firstType as SecondType; 编译器转型失败，编译不通过

			// 2.BaseType与SubType之间有继承关系
			SubType subType = new SubType { Name = "BaseType's name" };
			BaseType baseType = subType as BaseType;
			BaseType baseType2 = (BaseType)subType;

			// 3.as运算符不能操作基元类型，如果涉及到基元类型的算法需要使用is运算符进行判断
			//int a = subType as int;

			// as运算符不会抛出异常，如果类型不匹配（既不能操作符转换也不是继承关系），或者转型的源对象是null，那么转型后的值也是null。建议使用
			Console.WriteLine(secondType.Name);
			Console.WriteLine(baseType2.Name);
			Console.Read();
		}
	}
}