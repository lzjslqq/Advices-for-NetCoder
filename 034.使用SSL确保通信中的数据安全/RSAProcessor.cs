using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace _034.使用SSL确保通信中的数据安全
{
	/// <summary>
	/// 封装非对称加密算法
	/// </summary>
	public class RSAProcessor
	{
		/// <summary>
		/// 生成公钥和私钥
		/// </summary>
		/// <param name="publicKey"></param>
		/// <param name="pfxKey"></param>
		public static void CreateRSAKey(ref string publicKey, ref string pfxKey)
		{
			RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
			pfxKey = provider.ToXmlString(true);
			publicKey = provider.ToXmlString(false);
		}

		/// <summary>
		/// 加密
		/// </summary>
		/// <param name="xmlPublicKey">公钥</param>
		/// <param name="m_strEncryptString">要加密的字符串</param>
		/// <returns></returns>
		public static string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
		{
			byte[] btEncryptedSecret = Encoding.UTF8.GetBytes(m_strEncryptString);
			btEncryptedSecret = CRSAWrap.EncryptBuffer(xmlPublicKey, btEncryptedSecret);
			return Convert.ToBase64String(btEncryptedSecret);
		}

		/// <summary>
		/// 解密
		/// </summary>
		/// <param name="xmlPrivateKey">私钥</param>
		/// <param name="m_strDecryptString">加密串</param>
		/// <returns></returns>
		public static string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
		{
			byte[] btDecryptedSecred = Convert.FromBase64String(m_strDecryptString);
			btDecryptedSecred = CRSAWrap.DecryptBuffer(xmlPrivateKey, btDecryptedSecred);
			return Encoding.UTF8.GetString(btDecryptedSecred);
		}

		private class CRSAWrap
		{
			public static byte[] EncryptBuffer(string rsaKeyString, byte[] btSecret)
			{
				int keySize = 0;
				int blockSize = 0;
				int lastblockSize = 0;
				int counter = 0;
				int iterations = 0;
				int index = 0;
				byte[] btPlaintextToken;
				byte[] btEncryptedToken;
				byte[] btEncryptedSecret;
				RSACryptoServiceProvider rsaSender = new RSACryptoServiceProvider();
				rsaSender.FromXmlString(rsaKeyString);
				keySize = rsaSender.KeySize / 8;
				blockSize = keySize - 11;

				if ((btSecret.Length % blockSize) != 0)
				{
					iterations = btSecret.Length / blockSize + 1;
				}
				else
				{
					iterations = btSecret.Length / blockSize;
				}
				btPlaintextToken = new byte[blockSize];
				btEncryptedSecret = new byte[iterations * keySize];
				for (index = 0, counter = 0; counter < iterations; counter++, index += blockSize)
				{
					if (counter == (iterations - 1))
					{
						lastblockSize = btSecret.Length % blockSize;
						btPlaintextToken = new byte[lastblockSize];
						Array.Copy(btSecret, index, btPlaintextToken, 0, lastblockSize);
					}
					else
					{
						Array.Copy(btSecret, index, btPlaintextToken, 0, blockSize);
					}
					btEncryptedToken = rsaSender.Encrypt(btPlaintextToken, false);
					Array.Copy(btEncryptedToken, 0, btEncryptedSecret, counter * keySize, keySize);
				}
				return btEncryptedSecret;
			}

			public static byte[] DecryptBuffer(string rsaKeyString, byte[] btEncryptedSecret)
			{
				int keySize = 0;
				int blockSize = 0;
				int counter = 0;
				int iterations = 0;
				int index = 0;
				int byteCount = 0;
				byte[] btPlaintextToken;
				byte[] btEncryptedToken;
				byte[] btDecryptedSecret;
				RSACryptoServiceProvider rsaReceiver = new RSACryptoServiceProvider();
				rsaReceiver.FromXmlString(rsaKeyString);
				keySize = rsaReceiver.KeySize / 8;
				blockSize = keySize - 11;
				if ((btEncryptedSecret.Length % keySize) != 0)
				{
					return null;
				}
				iterations = btEncryptedSecret.Length / keySize;
				btEncryptedToken = new byte[keySize];
				Queue<byte[]> tokenQueue = new Queue<byte[]>();
				for (index = 0, counter = 0; counter < iterations; index += blockSize, counter++)
				{
					Array.Copy(btEncryptedSecret, counter * keySize, btEncryptedToken, 0, keySize);
					btPlaintextToken = rsaReceiver.Decrypt(btEncryptedToken, false);
					tokenQueue.Enqueue(btPlaintextToken);
				}
				byteCount = 0;
				foreach (var PlaintextToken in tokenQueue)
				{
					byteCount += PlaintextToken.Length;
				}
				counter = 0;
				btDecryptedSecret = new byte[byteCount];
				foreach (var PlaintextToken in tokenQueue)
				{
					if (counter == (iterations - 1))
					{
						Array.Copy(PlaintextToken, 0, btDecryptedSecret, btDecryptedSecret.Length - PlaintextToken.Length, PlaintextToken.Length);
					}
					else
					{
						Array.Copy(PlaintextToken, 0, btDecryptedSecret, counter * blockSize, blockSize);
					}
					counter++;
				}
				return btDecryptedSecret;
			}
		}
	}
}