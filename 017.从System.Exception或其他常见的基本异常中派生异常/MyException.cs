using System;
using System.Runtime.Serialization;

namespace _017.从System.Exception或其他常见的基本异常中派生异常
{
	/// <summary>
	/// 这是一个标准的自定义异常，它同时告诉你，你所创建的异常必须是可序列化的，因为你必须保证异常是可以穿越AppDomain边界的
	/// </summary>
	[Serializable]
	public class MyException : Exception
	{
		public MyException()
		{
		}

		public MyException(string message)
			: base(message)
		{
		}

		public MyException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected MyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}