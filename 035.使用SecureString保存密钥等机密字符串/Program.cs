using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace _035.使用SecureString保存密钥等机密字符串
{
	internal class Program
	{
		private static SecureString secureString = new SecureString();

		private static void Main(string[] args)
		{
			// 在此处打上断点,在即时窗口中相继运行命令：&str可查看str地址和内容。
			// 这就出现了一个问题，如果有人恶意扫描你的内存，程序中所保存的机密信息将泄露。
			Method1();
			Method2();
			// 当然，没有绝对的安全，因为即便如此，让关键字符串在内存中像流星一样一闪而过，它也存在被捕获的可能性。
			// 但是通过这种方法降低了数据被破解的概率
			Method3();
			Console.ReadKey();
		}

		private static void Method1()
		{
			string str = "test_string";
			Console.WriteLine(str);
		}

		private static void Method2()
		{
			// 进入Method2后，已经找不到对应的字符串"test_string2"了
			secureString.AppendChar('t');
			secureString.AppendChar('e');
			secureString.AppendChar('s');
			secureString.AppendChar('t');
			secureString.AppendChar('_');
			secureString.AppendChar('s');
			secureString.AppendChar('t');
			secureString.AppendChar('r');
			secureString.AppendChar('i');
			secureString.AppendChar('n');
			secureString.AppendChar('g');
			secureString.AppendChar('2');
		}

		private static void Method3()
		{
			secureString.AppendChar('t');
			secureString.AppendChar('e');
			secureString.AppendChar('s');
			secureString.AppendChar('t');
			secureString.AppendChar('_');
			secureString.AppendChar('s');
			secureString.AppendChar('t');
			secureString.AppendChar('r');
			secureString.AppendChar('i');
			secureString.AppendChar('n');
			secureString.AppendChar('g');
			secureString.AppendChar('3');

			// 把机密文本从SecureString取出来，临时赋值给字符串temp。
			// 这里存在两个问题：第一行实际调用的是非托管代码，它在内存中也会存储一个“liming”；第二行代码会在托管内存中存储一个“liming”。
			// 这两段文本的释放方式是不一样的
			IntPtr addr = Marshal.SecureStringToBSTR(secureString);
			string temp = Marshal.PtrToStringBSTR(addr);

			//使用该机密文本做一些事情
			Console.WriteLine(temp);
			///=======开始清理内存
			//清理掉非托管代码中对应的内存的值
			Marshal.ZeroFreeBSTR(addr);
			//清理托管代码对应的内存的值（采用重写的方法,重写成无意义的“xxxxxx”）
			int id = GetProcessID();
			byte[] writeBytes = Encoding.Unicode.GetBytes("xxxxxx");
			IntPtr intPtr = Open(id);
			unsafe
			{
				fixed (char* c = temp)
				{
					WriteMemory((IntPtr)c, writeBytes, writeBytes.Length);
				}
			}
			///=======清理完毕
		}

		private static PROCESS_INFORMATION processInfo = new PROCESS_INFORMATION();

		public static int GetProcessID()
		{
			Process p = Process.GetCurrentProcess();
			return p.Id;
		}

		public static IntPtr Open(int processId)
		{
			IntPtr hProcess = IntPtr.Zero;
			hProcess = ProcessAPIHelper.OpenProcess(ProcessAccessFlags.All, false, processId);
			if (hProcess == IntPtr.Zero)
				throw new Exception("OpenProcess失败");
			processInfo.hProcess = hProcess;
			processInfo.dwProcessId = processId;
			return hProcess;
		}

		private static int WriteMemory(IntPtr addressBase, byte[] writeBytes, int writeLength)
		{
			int reallyWriteLength = 0;
			if (!ProcessAPIHelper.WriteProcessMemory(processInfo.hProcess, addressBase, writeBytes, writeLength, out reallyWriteLength))
			{
				throw new Exception();
			}
			return reallyWriteLength;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct PROCESS_INFORMATION
		{
			public IntPtr hProcess;
			public IntPtr hThread;
			public int dwProcessId;
			public int dwThreadId;
		}

		[Flags]
		private enum ProcessAccessFlags : uint
		{
			All = 0x001F0FFF,
			Terminate = 0x00000001,
			CreateThread = 0x00000002,
			VMOperation = 0x00000008,
			VMRead = 0x00000010,
			VMWrite = 0x00000020,
			DupHandle = 0x00000040,
			SetInformation = 0x00000200,
			QueryInformation = 0x00000400,
			Synchronize = 0x00100000
		}

		private static class ProcessAPIHelper
		{
			[DllImport("kernel32.dll")]
			public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);

			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out uint lpNumberOfBytesRead);

			[DllImport("kernel32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CloseHandle(IntPtr hObject);
		}
	}
}