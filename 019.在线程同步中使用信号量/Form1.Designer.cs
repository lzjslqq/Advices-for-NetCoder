namespace _019.在线程同步中使用信号量
{
	partial class Form1
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
			this.btnStartAThread = new System.Windows.Forms.Button();
			this.btnSet = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnStartAThread
			// 
			this.btnStartAThread.Location = new System.Drawing.Point(69, 59);
			this.btnStartAThread.Name = "btnStartAThread";
			this.btnStartAThread.Size = new System.Drawing.Size(93, 23);
			this.btnStartAThread.TabIndex = 0;
			this.btnStartAThread.Text = "开启一个线程";
			this.btnStartAThread.UseVisualStyleBackColor = true;
			this.btnStartAThread.Click += new System.EventHandler(this.btnStartAThread_Click);
			// 
			// btnSet
			// 
			this.btnSet.Location = new System.Drawing.Point(69, 120);
			this.btnSet.Name = "btnSet";
			this.btnSet.Size = new System.Drawing.Size(100, 23);
			this.btnSet.TabIndex = 1;
			this.btnSet.Text = "给线程发送信号";
			this.btnSet.UseVisualStyleBackColor = true;
			this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(198, 69);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "label1";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(197, 130);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "label2";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(519, 306);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnSet);
			this.Controls.Add(this.btnStartAThread);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStartAThread;
		private System.Windows.Forms.Button btnSet;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}

