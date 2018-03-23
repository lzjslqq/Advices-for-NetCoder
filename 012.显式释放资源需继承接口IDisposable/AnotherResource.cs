using System;

namespace _012.显式释放资源需继承接口IDisposable
{
	/// <summary>
	/// 自定义类型示例（托管资源），实现了IDisposable称为非普通类型
	/// </summary>
	public class AnotherResource : IDisposable
	{
		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// 在标准的Dispose模式中，我们对非普通类型举了一个例子：一个非普通类型AnotherResource。
	/// 由于AnotherResource是一个非普通类型，所以如果有一个类型，它组合了AnotherResource，那么他就应该继承IDisposable接口
	/// 类型AnotherSampleClass虽然没有包含任何显式的非托管资源，但是由于它本身包含了一个非普通类型，所以我们仍旧必须为它实现一个标准的Dispose模式
	/// </summary>
	internal class AnotherSampleClass : IDisposable
	{
		private AnotherResource managedResource = new AnotherResource();
		private bool disposed = false;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~AnotherSampleClass()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}
			if (disposing)
			{
				// 清理托管资源
				if (managedResource != null)
				{
					managedResource.Dispose();
					managedResource = null;
				}
			}
			disposed = true;
		}

		public void SamplePublicMethod()
		{
			if (disposed)
			{
				throw new ObjectDisposedException("AnotherSampleClass", "AnotherSampleClass is disposed");
			}
			//省略
		}
	}
}