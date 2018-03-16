namespace _2.区别对待强制转型as和is
{
	public class FirstType
	{
		public string Name { get; set; }
	}

	public class SecondType
	{
		public string Name { get; set; }

		public static explicit operator SecondType(FirstType firstType)
		{
			return new SecondType { Name = "转型自：" + firstType.Name };
		}
	}
}