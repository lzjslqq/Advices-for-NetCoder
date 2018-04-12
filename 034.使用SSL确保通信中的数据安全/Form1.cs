using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace _034.使用SSL确保通信中的数据安全
{
	/// <summary>
	/// 非对称加密中：
	/// 秘钥分为两部分：公钥PK和私钥SK。
	/// 公钥用于加密数据用，私钥用于解密。
	/// 公钥可公开而且应该公开，私钥只属于创建者。
	/// 经过公钥加密的数据只有证书创建者才能解密。这是构成SSL通道所有理论的依据。
	/// 假定服务器端是数字证书的创建者，它保存好自己的私钥，同时公布了自己的公钥给所有的客户端。
	/// 首先，客户端随机生成一个字符串作为密钥K，然后用公钥PK对这个密钥加密，并将加密后密钥发送给服务器端。
	/// 如果客户端曾经在服务器端注册过自己的信息，则还可以在这个密钥上加上自己的身份信息，从而向服务器端汇报自己的唯一性，但在本例中略去这一步。
	///	服务器端用私钥解密消息，获取了客户端的K，并确认了客户端的身份（不可抵赖性），SSL通道建立。
	///	服务器端和客户端现在可以进行安全通信。过程是：发送方使用密钥K对要传输的消息进行对称加密，接受方则使用K进行解密。这就是传输过程中的不可篡改性。
	/// </summary>
	public partial class frmServer : Form
	{
		#region 服务端

		//用于保存非对称加密（数字证书）的公钥
		private string publicKey = string.Empty;

		//用于保存非对称加密（数字证书）的私钥
		private string pfxKey = string.Empty;

		///用于跟客户端通信的socket
		private Socket serverCommunicateSocket;

		///定义接受缓存块的大小
		private static int serverBufferSize = 1024;

		///缓存块
		private byte[] bytesReceivedFromClient = new byte[serverBufferSize];

		///密钥K
		private string key = string.Empty;

		private StringBuilder messageFromClient = new StringBuilder();

		public frmServer()
		{
			InitializeComponent();
		}

		private void btnStartServer_Click(object sender, EventArgs e)
		{
			RSAKeyInit();
			StartListen();
		}

		private void btnServerSend_Click(object sender, EventArgs e)
		{
			//加密消息体
			string msg = string.Format("{0}{1}", RijndaelProcessor.EncryptString(DateTime.Now.ToString(), key), "<EOF>");
			RijndaelProcessor.DencryptString(msg.Substring(0, msg.Length - 5), key);
			byte[] msgBytes = UTF32Encoding.Default.GetBytes(msg);
			serverCommunicateSocket.BeginSend(msgBytes, 0, msgBytes.Length, SocketFlags.None, null, null);
			ListBoxServerShow(string.Format("发送：{0}", msg));
		}

		/// <summary>
		/// 先生成数字证书（模拟，即非对称密钥对）
		/// </summary>
		private void RSAKeyInit()
		{
			RSAProcessor.CreateRSAKey(ref publicKey, ref pfxKey);
		}

		/// <summary>
		/// 负责侦听
		/// </summary>
		private void StartListen()
		{
			IPEndPoint iep = new IPEndPoint(IPAddress.Parse("192.168.44.1"), 8000);
			Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			listenSocket.Bind(iep);
			listenSocket.Listen(50);
			listenSocket.BeginAccept(new AsyncCallback(this.Accepted), listenSocket);
			ListBoxServerShow("开始侦听。。。");
			btnStartServer.Enabled = false;
		}

		private void Accepted(IAsyncResult result)
		{
			Socket listenSocket = result.AsyncState as Socket;
			//初始化和客户端进行通信的socket
			serverCommunicateSocket = listenSocket.EndAccept(result);
			ListBoxServerShow("有客户端连接到。。。");
			serverCommunicateSocket.BeginReceive(bytesReceivedFromClient, 0, serverBufferSize, SocketFlags.None, new AsyncCallback(this.ReceivedFromClient), null);
		}

		/// <summary>
		/// 负责处理接受自客户端的数据
		/// </summary>
		/// <param name="result"></param>

		private void ReceivedFromClient(IAsyncResult result)
		{
			int read = serverCommunicateSocket.EndReceive(result);
			if (read > 0)
			{
				messageFromClient.Append(UTF32Encoding.Default.GetString(bytesReceivedFromClient, 0, read));
				//处理并显示数据
				ProcessAndShowInServer();
				serverCommunicateSocket.BeginReceive(bytesReceivedFromClient, 0, serverBufferSize, 0, new AsyncCallback(ReceivedFromClient), null);
			}
		}

		private void ProcessAndShowInServer()
		{
			string msg = messageFromClient.ToString();
			//如果接收到<EOF>则表示完成完成一次，否则继续将自己置于接收状态
			if (msg.IndexOf("<EOF>") > -1)
			{
				//如果客户端发送key，则负责初始化key
				if (msg.IndexOf("<KEY>") > -1)
				{
					//用私钥解密发送过来的Key信息
					key = RSAProcessor.RSADecrypt(pfxKey, msg.Substring(0, msg.Length - 10));
					ListBoxServerShow(string.Format("接收到客户端密钥：{0}", key));
				}
				else
				{
					//解密SSL通道中发送过来的密文并显式
					ListBoxServerShow(string.Format("接收到客户端消息：{0}", RijndaelProcessor.DencryptString(msg.Substring(0, msg.Length - 5), key)));
				}
				messageFromClient.Clear();
			}
		}

		private void ListBoxServerShow(string msg)
		{
			listBoxServer.BeginInvoke(new Action(() =>
			{
				listBoxServer.Items.Add(msg);
			}));
		}

		#endregion 服务端

		#region 客户端

		///用于跟服务器通信的socket
		private Socket clientCommunicateSocket;

		///用于暂存接收到的字符串
		private StringBuilder messageFromServer = new StringBuilder();

		///定义接受缓存块的大小
		private static int clientBufferSize = 1024;

		///缓存块
		private byte[] bytesReceivedFromServer = new byte[clientBufferSize];

		//随机生成的key，在这里硬编码为key123
		private string keyCreateRandom = "key123";

		private void btnConnectAndReceive_Click(object sender, EventArgs e)
		{
			IPEndPoint iep = new IPEndPoint(IPAddress.Parse("192.168.44.1"), 8000);
			Socket connectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			connectSocket.BeginConnect(iep, new AsyncCallback(this.Connected), connectSocket);
			btnConnectAndReceive.Enabled = false;
		}

		private void btnClientSend_Click(object sender, EventArgs e)
		{
			//加密消息体
			string msg = string.Format("{0}{1}", RijndaelProcessor.EncryptString(DateTime.Now.ToString(), keyCreateRandom), "<EOF>");
			byte[] msgBytes = UTF32Encoding.Default.GetBytes(msg);
			clientCommunicateSocket.BeginSend(msgBytes, 0, msgBytes.Length, SocketFlags.None, null, null);
			ListBoxClientShow(string.Format("发送：{0}", msg));
		}

		private void Connected(IAsyncResult result)
		{
			clientCommunicateSocket = result.AsyncState as Socket;
			clientCommunicateSocket.EndConnect(result);
			clientCommunicateSocket.BeginReceive(bytesReceivedFromServer, 0, clientBufferSize, SocketFlags.None, new AsyncCallback(this.ReceivedFromServer), null);
			ListBoxClientShow("客户端连接上服务器。。。");
			//连接成功便发送密钥K给服务器
			SendKey();
		}

		private void ReceivedFromServer(IAsyncResult result)
		{
			int read = clientCommunicateSocket.EndReceive(result);
			if (read > 0)
			{
				messageFromServer.Append(UTF32Encoding.Default.GetString(bytesReceivedFromServer, 0, read));
				//处理并显示客户端数据
				ProcessAndShowInClient();
				clientCommunicateSocket.BeginReceive(bytesReceivedFromServer, 0, clientBufferSize, 0, new AsyncCallback(ReceivedFromServer), null);
			}
		}

		private void ProcessAndShowInClient()
		{
			//如果接收到<EOF>则表示完成一次接收，否则继续将自己置于接收状态
			if (messageFromServer.ToString().IndexOf("<EOF>") > -1)
			{
				//解密消息体并呈现出来
				ListBoxClientShow(string.Format("接收到服务器消息：{0}", RijndaelProcessor.DencryptString(messageFromServer.ToString().Substring(0, messageFromServer.ToString().Length - 5), keyCreateRandom)));
				messageFromServer.Clear();
			}
		}

		private void SendKey()
		{
			string msg = RSAProcessor.RSAEncrypt(publicKey, keyCreateRandom) + "<KEY><EOF>";
			byte[] msgBytes = UTF32Encoding.Default.GetBytes(msg);
			clientCommunicateSocket.BeginSend(msgBytes, 0, msgBytes.Length, SocketFlags.None, null, null);
			ListBoxClientShow(string.Format("发送：{0}", keyCreateRandom));
		}

		private void ListBoxClientShow(string msg)
		{
			listBoxClient.BeginInvoke(new Action(() =>
			{
				listBoxClient.Items.Add(msg);
			}));
		}

		#endregion 客户端
	}
}