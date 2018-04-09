using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _018.应使用finally避免资源泄漏
{
	public class ClassShouldDisposeBase : IDisposable
	{
		private bool disposed = false;
		private string _methodName;

		public ClassShouldDisposeBase(string methodName)
		{
			_methodName = methodName;
		}

		public void Dispose()
		{
			// 必须指定为true
			Dispose(true);
			// 通知垃圾回收机制不再调用终结器（析构器）
			GC.SuppressFinalize(this);
			Console.WriteLine("在方法：" + _methodName + "中被释放！");
		}

		public void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				// 清理托管资源
			}

			// 清理非托管资源

			// 让类型知道自己已经被释放
			disposed = true;
		}

		~ClassShouldDisposeBase()
		{
			Dispose(false);
		}
	}
}