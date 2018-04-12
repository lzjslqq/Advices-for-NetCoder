using System;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace _036.为应用程序设定运行权限
{
	/// <summary>
	/// 在某些情况下，可能存在这样的需求：只有系统管理员才能访问某应用程序的若干功能。
	/// 可以结合.NET中提供的代码访问安全性（Code Access Security）和基于角色（Role-Based Security）的安全性去实现。
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			// 如果是以User用户组的用户登录系统的，则会抛出异常System.Security.SecurityException：对主体权限的请求失败
			AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
			SampleClass sample = new SampleClass();
			sample.SampleMethod();
			sample.SampleMethodSecond();
			Console.WriteLine("有权限，代码成功运行1");

			GenericIdentity examIdentity = new GenericIdentity("ExamUser");
			// 配置了教师和学生用户组，所以可以修改成功
			String[] Users = { "teacher", "Student" };
			GenericPrincipal testPrincipal = new GenericPrincipal(examIdentity, Users);
			Thread.CurrentPrincipal = testPrincipal;
			ScoreProcessor processor = new ScoreProcessor();
			processor.Update();
			Console.WriteLine("有权限，代码成功运行2");

			Console.Read();
		}
	}

	//[PrincipalPermission(SecurityAction.Demand, Role = @"Administrator")]
	[PrincipalPermission(SecurityAction.Demand, Role = @"Users")]
	internal class SampleClass
	{
		public void SampleMethod()
		{
			Console.WriteLine("执行方法SampleMethod");
		}

		[PrincipalPermission(SecurityAction.Demand, Role = @"Administrator")]
		//[PrincipalPermission(SecurityAction.Demand, Role = @"Users")]
		public void SampleMethodSecond()
		{
			Console.WriteLine("执行方法SampleMethodSecond");
		}
	}

	/// <summary>
	/// 只有教师才能修改成绩，而考生只能具备浏览功能
	/// </summary>
	internal class ScoreProcessor
	{
		public void Update()
		{
			try
			{
				PrincipalPermission MyPermission = new PrincipalPermission("ExamUser", "Teacher");
				MyPermission.Demand();
				//省略
				Console.WriteLine("修改成绩成功");
			}
			catch (SecurityException e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}