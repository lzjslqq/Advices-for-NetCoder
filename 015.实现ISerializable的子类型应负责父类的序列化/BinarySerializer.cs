using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace _015.实现ISerializable的子类型应负责父类的序列化
{
	public class BinarySerializer
	{
		/// <summary>
		/// 将类型序列化为字符串
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <returns></returns>
		public static string Serialize<T>(T t)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, t);
				return Encoding.UTF8.GetString(stream.ToArray());
			}
		}

		/// <summary>
		///
		/// 将字符串反序列化为对象
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="s"></param>
		/// <returns></returns>
		public static TResult Deserialize<TResult>(string s) where TResult : class
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				return formatter.Deserialize(stream) as TResult;
			}
		}

		/// <summary>
		/// 将对象序列化到文件中
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <param name="path"></param>
		/// <param name="fullName"></param>
		public static void SerializeToFile<T>(T t, string path, string fullName)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			string fullPath = Path.Combine(path, fullName);

			using (FileStream stream = new FileStream(fullPath, FileMode.OpenOrCreate))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, t);
				stream.Flush();
			}
		}

		/// <summary>
		/// 将文件反序列化为对象
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="path"></param>
		/// <returns></returns>
		public static TResult DeserializeFromFile<TResult>(string path) where TResult : class
		{
			using (FileStream stream = new FileStream(path, FileMode.Open))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				return formatter.Deserialize(stream) as TResult;
			}
		}
	}
}