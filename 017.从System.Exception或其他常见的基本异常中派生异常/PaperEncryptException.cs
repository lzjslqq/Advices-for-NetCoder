using System;
using System.Runtime.Serialization;

namespace _017.从System.Exception或其他常见的基本异常中派生异常
{
	/// <summary>
	///		一般来说，从Exception或其他常见的基本异常中派生异常已经满足你对于自定义异常的普通需求，但是另一个需求是，你也许会想要格式化异常的Message。
	///	比如，为一个考试系统设计一个自定义的加密异常
	///		PaperEncryptException与MyException相比，有两个明显的不同：
	///		1）实现了接口ISerializable。
	///		2）重写了方法GetObjectData。
	///		因为我们给PaperEncryptException定义了一个新的字段_paperInfo。为了确保新定义的字段也能被序列化，必须要让异常类型实现ISerializable接口，
	///	并且需要将字段加入到GetObjectData方法的SerializationInfo参数中
	/// </summary>
	[Serializable]
	public class PaperEncryptException : Exception, ISerializable
	{
		private readonly string _paperInfo;

		public PaperEncryptException()
		{
		}

		public PaperEncryptException(string message)
			: base(message)
		{
		}

		public PaperEncryptException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public PaperEncryptException(string message, string paperInfo)
			: base(message)
		{
			_paperInfo = paperInfo;
		}

		public PaperEncryptException(string message, string paperInfo, Exception inner)
			: base(message, inner)
		{
			_paperInfo = paperInfo;
		}

		protected PaperEncryptException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public override string Message
		{
			get
			{
				return base.Message + " " + _paperInfo;
			}
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Args", _paperInfo);
			base.GetObjectData(info, context);
		}
	}
}