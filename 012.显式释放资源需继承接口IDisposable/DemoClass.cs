using System;
using System.Runtime.InteropServices;

namespace _012.显式释放资源需继承接口IDisposable
{
	/// <summary>
	/// 一个标准的继承了IDisposable接口的类型应该像下面这样去实现，这种实现我们称为Dispose模式
	///   为什么要区别对待托管资源和非托管资源呢？在这个问题前，我们首先要弄明白：托管资源需要手工清理吗？不妨将C#中的类型分为两类，一类继承了IDisposable接口，一类则没有继承。前者，暂时称为非普通类型，后者称为普通类型。非普通类型因为包含非托管资源，所以它需要继承IDisposable接口，但是，这里包含非托管资源的类型本身，它是一个托管资源。所以，托管资源中的普通类型不需要手动清理，而非普通类型是需要手工清理的（即调用Dispose方法）。
	///   Dispose模式设计的思路是：如果调用者显式调用了Dispose方法，那么类型就应该按部就班地将自己的资源全部释放。如果调用者忘记调用Dispose方法，那么类型就假设自己的所有托管资源（哪怕是那些非普通类型）会全部都交给垃圾回收器回收，所以不进行手工清理。所以在Dispose方法中，虚方法传入参数true，在终结器中，虚方法传入参数false。
	/// </summary>
	public class DemoClass : IDisposable
	{
		// 演示创建一个非托管资源
		private IntPtr nativeResource = Marshal.AllocHGlobal(100);

		// 演示创建一个托管资源
		private AnotherResource managedResource = new AnotherResource();

		private bool disposed = false;

		/// <summary>
		/// 实现IDisposable中的Dispose方法
		/// </summary>
		public void Dispose()
		{
			// 必须指定为true
			Dispose(true);
			// 通知垃圾回收机制不再调用终结器（析构器）
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 不是必要的，提供一个Close方法仅仅是为了更符合其他语言（如
		/// C++）的规范
		/// </summary>
		public void Close()
		{
			Dispose();
		}

		/// <summary>
		/// 必须，防止程序员忘记了显式调用Dispose方法
		/// </summary>
		~DemoClass()
		{
			// 必须为false：隐式清理时，只要处理非托管资源就可以了
			//	  这个方法叫做类型的终结器。提供类型终结器的意义在于，我们不能奢望类型的调用者肯定会主动调用Dispose方法，基于终结器会被垃圾回收这个特  点，它被  用作资源释放的补救措施。
			//	  在.NET中每次使用new操作符创建对象时，CLR都会为该对象在堆上分配内存。对于没有继承IDisposable接口的类型对象，垃圾回收器则会直接释放对象所占用的内存
			//    而对于实现了Dispose模式的类型，每次创建对象的时候，CLR都会将该对象的一个指针放到终结列表中，垃圾回收器在回收该对象的内存前，首先将终结列表中的指针放到一个freachable队列中。同时，CLR还会分配专门的线程读取freachable队列，并调用对象的终结器，只有这个时候对象才会真正被识别为垃圾，并且在下一次进行垃圾回收时释放对象所占的内存
			//    可见，实现了Dispose模式的类型对象，起码要经过两次垃圾回收才能真正地被回收掉，应为垃圾回收机制会安排CLR调用终结器。基于这个特点，如果我们的类型提供了显式释放的方法来减少一次垃圾回收，同时也可以在终结器中提供隐式清理，以避免调用者忘记调用该方法而带来的资源泄漏。
			Dispose(false);
		}

		/// <summary>
		/// 非密封类修饰用protected virtual
		/// 密封类修饰用private
		/// 之所以提供这样一个受保护的虚方法，是因为考虑了这个类型会被其他类型继承的情况。
		/// 如果类型存在一个子类，子类也许会实现自己的Dispose模式。受保护的虚方法用来提醒子类：必须在自己的清理方法时注意到父类的清理工作，即子类需要在自己的释放方法中调用base.Dispose方法
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				// 清理托管资源
				if (managedResource != null)
				{
					managedResource.Dispose();
					managedResource = null;
				}
			}

			// 清理非托管资源
			if (nativeResource != IntPtr.Zero)
			{
				// 演示清理非托管资源代码
				Marshal.FreeHGlobal(nativeResource);
				nativeResource = IntPtr.Zero;
			}

			// 让类型知道自己已经被释放
			disposed = true;
		}

		/// <summary>
		/// 对象被调用过Dispose方法，并不表示该对象被置为null，且被垃圾回收机制回收过内存，已经彻底不存在了。
		/// 事实上，对象的引用可能还在。但是，对象被Dispose过，说明对象的正常状态已经不存在了，此时如果调用对象的公开的方法，应该会抛出一个ObjectDisposedException
		/// </summary>
		public void SamplePublicMethod()
		{
			if (disposed)
			{
				throw new ObjectDisposedException("DemoClass", "DemoClass is disposed");
			}
			//省略
		}
	}
}