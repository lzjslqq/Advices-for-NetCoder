//#define ONLINE
#define OFFLINE

using System;
using System.Diagnostics;

namespace _038.利用特性为应用程序提供多个版本
{
	/// <summary>
	/// 基于如下理由，需要为应用程序提供多个版本：
	/// 应用程序有体验版和完整功能版。
	/// 应用程序在迭代过程中需要屏蔽一些不成熟的功能
	/// </summary>
	internal class Program
	{
		/// <summary>
		/// 假设我们的应用程序共有两类功能：第一类功能属于单机版，而第二类的完整版还提供了在线功能。
		/// 那么，在功能上，需要定制两个属性“ONLINE”和“OFFLINE”。在体验版中，我们只开放“OFFLINE”功能。
		/// 要实现此目的，不应该提供两套应用程序，而应该通过最小设置，为一个应用程序输出两个发布版本，可以通过.NET中的特性（Attribute）来实现
		/// </summary>
		/// <param name="args"></param>
		private static void Main(string[] args)
		{
			MyService service = new MyService();
			service.Testing();
			service.GetInfoFromNet();

			Console.Read();
		}
	}

	/// <summary>
	/// 要实现两个不同的功能，需要在程序入口这个文件最开头定义#define ONLINE、#define OFFLINE
	/// 这条编译符号一定要在文件的最开头。同时，该定义只对本文件有效。如果要想定义全局编译符号，则必须在项目属性中定义
	/// </summary>
	internal class MyService
	{
		[Conditional("ONLINE")]
		public void Testing()
		{
			Console.WriteLine("完整功能版");
		}

		[Conditional("OFFLINE")]
		[Conditional("ONLINE")]
		public void GetInfoFromNet()
		{
			Console.WriteLine("单机功能版");
		}
	}
}