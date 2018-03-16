using System;
using System.Net;

namespace _1.使用默认转型方法
{
	public class IP //:IConvertible
	{
		private IPAddress value;

		public IP(string ip)
		{
			value = IPAddress.Parse(ip);
		}

		public static implicit operator IP(string ip)
		{
			IP temp = new IP(ip);
			return temp;
		}

		public override string ToString()
		{
			return value.ToString();
		}
	}
}