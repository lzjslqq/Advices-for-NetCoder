namespace _012.显式释放资源需继承接口IDisposable
{
	internal class Program
	{
		/// <summary>
		/// C#中的每一个类型都代表一种资源，资源分为两类：
		/// 1.托管资源：由CLR管理分配和释放的资源，即从CLR中new出来的对象
		/// 2.非托管资源：不受CLR管理的对象，如Windows内核对象，或者文件、数据库连接、套接字和COM对象等
		/// 如果我们的类型使用了非托管资源，或者需要显示地释放托管资源，那么就需要让类型继承接口IDisposable。
		/// 这相当于告诉调用者，类型资源是需要显示释放资源的，你需要调用类型的Dispose方法
		/// </summary>
		/// <param name="args"></param>
		private static void Main(string[] args)
		{
			// 继承IDisposable接口也为实现语法糖using带来了便利
			// 如果存在两个类型一致的对象，using还可以这样使用
			using (DemoClass c1 = new DemoClass(), c2 = new DemoClass())
			{
				//省略
			}
		}
	}
}