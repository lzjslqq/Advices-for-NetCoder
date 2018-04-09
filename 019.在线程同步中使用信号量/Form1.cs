using System;
using System.Threading;
using System.Windows.Forms;

namespace _019.在线程同步中使用信号量
{
	public partial class Form1 : Form
	{
		// 设置自己的默认阻滞状态是false。这意味着任何在它上面进行等待的线程都将被阻滞。
		private AutoResetEvent autoResetEvent = new AutoResetEvent(false);

		// AutoResetEvent和ManualResetEvent的区别是：前者在发送信号完毕后（即调用Set方法），会自动将自己的阻滞状态设置为false，而后者则需要进行手动设定
		private ManualResetEvent manualResetEvent = new ManualResetEvent(false);

		public Form1()
		{
			InitializeComponent();
		}

		private void UpdateControlText(Control control, object str)
		{
			if (control.InvokeRequired)
			{
				// 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
				Action<string> actionDelegate = (x) => { control.Text = x.ToString(); };
				// 或者
				// Action<string> actionDelegate = delegate(string txt) { this.label1.Text = txt; };
				control.Invoke(actionDelegate, str);
			}
			else
			{
				control.Text = str.ToString();
			}
		}

		private void btnStartAThread_Click(object sender, EventArgs e)
		{
			// 这个例子的本意是要让新起的两个工作线程tWork1和tWork2都阻滞，直到收到主线程的信号再继续工作。
			// 而程序运行的结果是，只有一个工作线程继续工作，另外一个工作线程则继续保持阻滞状态。
			// 原因是AutoResetEvent发送信号完毕就在内核中自动将自己的状态设置回false了，所以另外一个工作线程相当于根本没有收到主线程的信号。
			// 要修正这个问题，可以使用ManualResetEvent。
			//StartThread1();
			//StartThread2();

			// 一个需要用到线程同步的实际例子：模拟网络通信。客户端在运行过程中，服务器每隔一段的时间会给客户端发送心跳数据。
			// 实际工作中的服务器和客户端在网络中是两台不同的终端，不过在这个例子中将其进行了简化：
			// 工作线程tClient模拟客户端，主线程（UI线程）模拟服务器端。客户端每3秒检测是否收到服务器的心跳数据，如果没有心跳数据，则显示网络连接断开。
			Thread tClient = new Thread(() =>
			{
				while (true)
				{
					//等3秒，3秒没有信号，显示断开
					//有信号，则显示更新
					bool re = autoResetEvent.WaitOne(3000);
					if (re)
					{
						UpdateControlText(this.label1, string.Format("时间：{0}，{1}",
							DateTime.Now.ToString(), "保持连接状态"));
					}
					else
					{
						UpdateControlText(this.label1, string.Format("时间：{0}，{1}",
							DateTime.Now.ToString(), "断开，需要重启"));
					}
				}
			});

			tClient.IsBackground = true;
			tClient.Start();
		}

		private void btnSet_Click(object sender, EventArgs e)
		{
			// 主线程向在autoResetEvent上等待的线程tWork上下文发送信号，即将tWork的阻滞状态设置为true。tWork接收到这个信号后，开始继续工作。
			autoResetEvent.Set();
		}

		private void StartThread1()
		{
			Thread tWork1 = new Thread(() =>
			{
				UpdateControlText(this.label1, "线程1启动···" + Environment.NewLine);
				UpdateControlText(this.label1, "开始处理一些实际的工作" + Environment.NewLine);
				// 省略工作代码
				UpdateControlText(this.label1, "我开始等待别的线程给我信号，才愿意继续下去" + Environment.NewLine);
				// tWork开始在autoResetEvent上等待任何其他地方给它的信号。信号来了，则tWork开始继续工作，否则就一直等着（即阻滞）。
				autoResetEvent.WaitOne();
				UpdateControlText(this.label1, "我继续做一些工作，然后结束了！");
				// 省略工作代码
			});

			tWork1.IsBackground = true;
			tWork1.Start();
		}

		private void StartThread2()
		{
			Thread tWork2 = new Thread(() =>
			{
				UpdateControlText(this.label2, "线程2启动···" + Environment.NewLine);
				UpdateControlText(this.label2, "开始处理一些实际的工作" + Environment.NewLine);
				// 省略工作代码
				UpdateControlText(this.label2, "我开始等待别的线程给我信号，才愿意继续下去" + Environment.NewLine);
				// tWork开始在autoResetEvent上等待任何其他地方给它的信号。信号来了，则tWork开始继续工作，否则就一直等着（即阻滞）。
				autoResetEvent.WaitOne();
				UpdateControlText(this.label2, "我继续做一些工作，然后结束了！");
				// 省略工作代码
			});

			tWork2.IsBackground = true;
			tWork2.Start();
		}
	}
}