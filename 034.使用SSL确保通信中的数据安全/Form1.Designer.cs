namespace _034.使用SSL确保通信中的数据安全
{
	partial class frmServer
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.btnStartServer = new System.Windows.Forms.Button();
			this.btnServerSend = new System.Windows.Forms.Button();
			this.listBoxServer = new System.Windows.Forms.ListBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.listBoxClient = new System.Windows.Forms.ListBox();
			this.btnClientSend = new System.Windows.Forms.Button();
			this.btnConnectAndReceive = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnStartServer
			// 
			this.btnStartServer.Location = new System.Drawing.Point(29, 23);
			this.btnStartServer.Name = "btnStartServer";
			this.btnStartServer.Size = new System.Drawing.Size(75, 23);
			this.btnStartServer.TabIndex = 0;
			this.btnStartServer.Text = "开启服务器";
			this.btnStartServer.UseVisualStyleBackColor = true;
			this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
			// 
			// btnServerSend
			// 
			this.btnServerSend.Location = new System.Drawing.Point(29, 208);
			this.btnServerSend.Name = "btnServerSend";
			this.btnServerSend.Size = new System.Drawing.Size(75, 23);
			this.btnServerSend.TabIndex = 1;
			this.btnServerSend.Text = "发送数据";
			this.btnServerSend.UseVisualStyleBackColor = true;
			this.btnServerSend.Click += new System.EventHandler(this.btnServerSend_Click);
			// 
			// listBoxServer
			// 
			this.listBoxServer.FormattingEnabled = true;
			this.listBoxServer.ItemHeight = 12;
			this.listBoxServer.Location = new System.Drawing.Point(133, 11);
			this.listBoxServer.Name = "listBoxServer";
			this.listBoxServer.Size = new System.Drawing.Size(354, 244);
			this.listBoxServer.TabIndex = 2;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnStartServer);
			this.panel1.Controls.Add(this.listBoxServer);
			this.panel1.Controls.Add(this.btnServerSend);
			this.panel1.Location = new System.Drawing.Point(30, 33);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(506, 265);
			this.panel1.TabIndex = 3;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.listBoxClient);
			this.panel2.Controls.Add(this.btnClientSend);
			this.panel2.Controls.Add(this.btnConnectAndReceive);
			this.panel2.Location = new System.Drawing.Point(30, 304);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(506, 293);
			this.panel2.TabIndex = 4;
			// 
			// listBoxClient
			// 
			this.listBoxClient.FormattingEnabled = true;
			this.listBoxClient.ItemHeight = 12;
			this.listBoxClient.Location = new System.Drawing.Point(133, 13);
			this.listBoxClient.Name = "listBoxClient";
			this.listBoxClient.Size = new System.Drawing.Size(354, 268);
			this.listBoxClient.TabIndex = 4;
			// 
			// btnClientSend
			// 
			this.btnClientSend.Location = new System.Drawing.Point(17, 234);
			this.btnClientSend.Name = "btnClientSend";
			this.btnClientSend.Size = new System.Drawing.Size(101, 23);
			this.btnClientSend.TabIndex = 3;
			this.btnClientSend.Text = "发送";
			this.btnClientSend.UseVisualStyleBackColor = true;
			this.btnClientSend.Click += new System.EventHandler(this.btnClientSend_Click);
			// 
			// btnConnectAndReceive
			// 
			this.btnConnectAndReceive.Location = new System.Drawing.Point(17, 25);
			this.btnConnectAndReceive.Name = "btnConnectAndReceive";
			this.btnConnectAndReceive.Size = new System.Drawing.Size(101, 23);
			this.btnConnectAndReceive.TabIndex = 2;
			this.btnConnectAndReceive.Text = "连接并接收消息";
			this.btnConnectAndReceive.UseVisualStyleBackColor = true;
			this.btnConnectAndReceive.Click += new System.EventHandler(this.btnConnectAndReceive_Click);
			// 
			// frmServer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(577, 622);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "frmServer";
			this.Text = "服务端";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnStartServer;
		private System.Windows.Forms.Button btnServerSend;
		private System.Windows.Forms.ListBox listBoxServer;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnClientSend;
		private System.Windows.Forms.Button btnConnectAndReceive;
		private System.Windows.Forms.ListBox listBoxClient;
	}
}

