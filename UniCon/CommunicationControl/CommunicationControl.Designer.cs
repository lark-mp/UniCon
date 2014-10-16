namespace UniCon.CommunicationControl
{
	partial class CommunicationControl
	{
		/// <summary> 
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region コンポーネント デザイナーで生成されたコード

		/// <summary> 
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.comTCPRBtn = new System.Windows.Forms.RadioButton();
			this.comSerialRBtn = new System.Windows.Forms.RadioButton();
			this.comSerialLabel = new System.Windows.Forms.Label();
			this.comTCPPortLabael = new System.Windows.Forms.Label();
			this.comTCPRecvPortText = new System.Windows.Forms.TextBox();
			this.comSerialCBox = new System.Windows.Forms.ComboBox();
			this.comTCPSendPortText = new System.Windows.Forms.TextBox();
			this.comTCPIPLabel = new System.Windows.Forms.Label();
			this.comTCPIPText = new System.Windows.Forms.TextBox();
			this.comSerialPort = new System.IO.Ports.SerialPort(this.components);
			this.comConnectCBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// comTCPRBtn
			// 
			this.comTCPRBtn.AutoSize = true;
			this.comTCPRBtn.Location = new System.Drawing.Point(12, 40);
			this.comTCPRBtn.Name = "comTCPRBtn";
			this.comTCPRBtn.Size = new System.Drawing.Size(61, 16);
			this.comTCPRBtn.TabIndex = 4;
			this.comTCPRBtn.TabStop = true;
			this.comTCPRBtn.Text = "TCP/IP";
			this.comTCPRBtn.UseVisualStyleBackColor = true;
			this.comTCPRBtn.CheckedChanged += new System.EventHandler(this.comConTCPRButton_CheckedChanged);
			// 
			// comSerialRBtn
			// 
			this.comSerialRBtn.AutoSize = true;
			this.comSerialRBtn.Location = new System.Drawing.Point(12, 4);
			this.comSerialRBtn.Name = "comSerialRBtn";
			this.comSerialRBtn.Size = new System.Drawing.Size(77, 16);
			this.comSerialRBtn.TabIndex = 3;
			this.comSerialRBtn.TabStop = true;
			this.comSerialRBtn.Text = "Serial Port";
			this.comSerialRBtn.UseVisualStyleBackColor = true;
			this.comSerialRBtn.CheckedChanged += new System.EventHandler(this.comConSerialRButton_CheckedChanged);
			// 
			// comSerialLabel
			// 
			this.comSerialLabel.AutoSize = true;
			this.comSerialLabel.Location = new System.Drawing.Point(122, 6);
			this.comSerialLabel.Name = "comSerialLabel";
			this.comSerialLabel.Size = new System.Drawing.Size(28, 12);
			this.comSerialLabel.TabIndex = 1;
			this.comSerialLabel.Text = "Port:";
			// 
			// comTCPPortLabael
			// 
			this.comTCPPortLabael.AutoSize = true;
			this.comTCPPortLabael.Location = new System.Drawing.Point(87, 67);
			this.comTCPPortLabael.Name = "comTCPPortLabael";
			this.comTCPPortLabael.Size = new System.Drawing.Size(62, 12);
			this.comTCPPortLabael.TabIndex = 1;
			this.comTCPPortLabael.Text = "Port(tx/rx):";
			// 
			// comTCPRecvPortText
			// 
			this.comTCPRecvPortText.Location = new System.Drawing.Point(214, 64);
			this.comTCPRecvPortText.Name = "comTCPRecvPortText";
			this.comTCPRecvPortText.Size = new System.Drawing.Size(53, 19);
			this.comTCPRecvPortText.TabIndex = 0;
			// 
			// comSerialCBox
			// 
			this.comSerialCBox.FormattingEnabled = true;
			this.comSerialCBox.Location = new System.Drawing.Point(155, 3);
			this.comSerialCBox.Name = "comSerialCBox";
			this.comSerialCBox.Size = new System.Drawing.Size(121, 20);
			this.comSerialCBox.TabIndex = 0;
			// 
			// comTCPSendPortText
			// 
			this.comTCPSendPortText.Location = new System.Drawing.Point(155, 64);
			this.comTCPSendPortText.Name = "comTCPSendPortText";
			this.comTCPSendPortText.Size = new System.Drawing.Size(53, 19);
			this.comTCPSendPortText.TabIndex = 0;
			// 
			// comTCPIPLabel
			// 
			this.comTCPIPLabel.AutoSize = true;
			this.comTCPIPLabel.Location = new System.Drawing.Point(122, 42);
			this.comTCPIPLabel.Name = "comTCPIPLabel";
			this.comTCPIPLabel.Size = new System.Drawing.Size(17, 12);
			this.comTCPIPLabel.TabIndex = 1;
			this.comTCPIPLabel.Text = "IP:";
			// 
			// comTCPIPText
			// 
			this.comTCPIPText.Location = new System.Drawing.Point(155, 39);
			this.comTCPIPText.Name = "comTCPIPText";
			this.comTCPIPText.Size = new System.Drawing.Size(121, 19);
			this.comTCPIPText.TabIndex = 0;
			// 
			// comSerialPort
			// 
			this.comSerialPort.BaudRate = 115200;
			// 
			// comConnectCBox
			// 
			this.comConnectCBox.Appearance = System.Windows.Forms.Appearance.Button;
			this.comConnectCBox.Location = new System.Drawing.Point(201, 100);
			this.comConnectCBox.Name = "comConnectCBox";
			this.comConnectCBox.Size = new System.Drawing.Size(75, 23);
			this.comConnectCBox.TabIndex = 6;
			this.comConnectCBox.Text = "connect";
			this.comConnectCBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.comConnectCBox.UseVisualStyleBackColor = true;
			this.comConnectCBox.CheckedChanged += new System.EventHandler(this.comConConnectCBox_CheckedChanged);
			// 
			// CommunicationControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.comConnectCBox);
			this.Controls.Add(this.comTCPRBtn);
			this.Controls.Add(this.comSerialRBtn);
			this.Controls.Add(this.comTCPIPText);
			this.Controls.Add(this.comSerialLabel);
			this.Controls.Add(this.comTCPIPLabel);
			this.Controls.Add(this.comTCPPortLabael);
			this.Controls.Add(this.comTCPSendPortText);
			this.Controls.Add(this.comTCPRecvPortText);
			this.Controls.Add(this.comSerialCBox);
			this.Name = "CommunicationControl";
			this.Size = new System.Drawing.Size(288, 126);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton comTCPRBtn;
		private System.Windows.Forms.RadioButton comSerialRBtn;
		private System.Windows.Forms.Label comSerialLabel;
		private System.Windows.Forms.Label comTCPPortLabael;
		private System.Windows.Forms.TextBox comTCPRecvPortText;
		private System.Windows.Forms.ComboBox comSerialCBox;
		private System.Windows.Forms.TextBox comTCPSendPortText;
		private System.Windows.Forms.Label comTCPIPLabel;
		private System.Windows.Forms.TextBox comTCPIPText;
		private System.IO.Ports.SerialPort comSerialPort;
		private System.Windows.Forms.CheckBox comConnectCBox;
	}
}
