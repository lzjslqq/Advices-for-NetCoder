using System;
using System.Runtime.InteropServices;

namespace _012.显式释放资源需继承接口IDisposable
{
	public class DerivedDemoClass : DemoClass
	{
		// 子类的非托管资源
		private IntPtr derivedNativeResource = Marshal.AllocHGlobal(100);

		// 子类的托管资源
		private AnotherResource derivedManagedResource = new AnotherResource();

		// 定义自己的是否释放的标识变量
		private bool disposed = false;

		/// <summary>
		/// 重写父类的Dispose方法
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				// 清理子类托管资源
				if (derivedManagedResource != null)
				{
					derivedManagedResource.Dispose();
					derivedManagedResource = null;
				}
			}

			// 清理子类非托管资源
			if (derivedNativeResource != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(derivedNativeResource);
				derivedNativeResource = IntPtr.Zero;
			}

			// 调用父类的清理代码
			base.Dispose(disposing);

			//让类型知道自己已经被释放
			disposed = true;
		}
	}
}