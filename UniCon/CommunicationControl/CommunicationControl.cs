using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using UniCon.Interfaces;
using UniCon.CommunicationControl.Communicator;

namespace UniCon.CommunicationControl
{
	public partial class CommunicationControl : UserControl
	{
		CommunicatorFactory comFactory;
		ICommunicator communicator;

		public CommunicationControl()
		{
			InitializeComponent();
			comFactory = new CommunicatorFactory();
			comSerialCBox.Items.AddRange(SerialPort.GetPortNames());
			if (comSerialCBox.Items.Count > 0)
			{
				comSerialCBox.SelectedIndex = 0;
			}
			comSerialRBtn.Checked = true;
		}

		private void EnableSerialDisableTCP()
		{
			comSerialCBox.Enabled = true;

			comTCPIPText.Enabled = false;
			comTCPRecvPortText.Enabled = false;
			comTCPSendPortText.Enabled = false;
		}

		private void EnableTCPDisableSerial()
		{
			comSerialCBox.Enabled = false;

			comTCPIPText.Enabled = true;
			comTCPRecvPortText.Enabled = true;
			comTCPSendPortText.Enabled = true;
		}

		void DisableByRadioButton()
		{
			if (comSerialRBtn.Checked)
			{
				EnableSerialDisableTCP();
			}
			else if (comTCPRBtn.Checked)
			{
				EnableTCPDisableSerial();
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		void DisableAllButton()
		{
			comSerialRBtn.Enabled = false;
			comTCPRBtn.Enabled = false;

            comSerialCBox.Enabled = false;

            comTCPIPText.Enabled = false;
            comTCPRecvPortText.Enabled = false;
            comTCPSendPortText.Enabled = false;
		}

		void EnableRadioButton()
		{
			comSerialRBtn.Enabled = true;
			comTCPRBtn.Enabled = true;
		}

		void OnCheckBoxChanged()
		{
			if (comConnectCBox.Checked)
			{
				try
				{
					CreateNewCommunicator();
					DisableAllButton();
				}
				catch (Exception)
				{
					MessageBox.Show("接続できませんでした。",
						"エラー",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
					comConnectCBox.Checked = false;
				}
			}
			else
			{
				DestroyCommunicator();
				EnableRadioButton();
				DisableByRadioButton();
			}
		}

		void CreateNewCommunicator()
		{
			if (comSerialRBtn.Checked)
			{
				communicator = comFactory.CreateCommunicator(ref comSerialPort, comSerialCBox.Text);
                //communicator = comFactory.CreateCommunicator(ref comSerialPort, comSerialCBox.Text,0);
			}
			else if (comTCPRBtn.Checked)
			{
				communicator = comFactory.CreateCommunicator(comTCPIPText.Text,
					Int32.Parse(comTCPSendPortText.Text), Int32.Parse(comTCPRecvPortText.Text));
			}
			else
			{
				throw new NotImplementedException();
			}
			communicator.ReceiveLineEvent += new ICommunicator.ReceiveLineEventHandler(this.OnLineReceived);
		}

		private void DestroyCommunicator()
		{
			if (communicator != null)
			{
				communicator.Disconnect();
				communicator = null;
			}
		}

		#region PublicEventsMethods
		public event EventHandler<ReceiveLineEventArgs> LineReceived;
		
		[Browsable(true)]
		[Description("1行読んだ時に発生するイベント")]
		protected virtual void OnLineReceived(ReceiveLineEventArgs e)
		{
			EventHandler<ReceiveLineEventArgs> eventHandler = LineReceived;

			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		public void SendLine(string s)
		{
            if (communicator != null)
            {
                communicator.SendLine(s);
            }
		}

		#endregion

		#region InternalEvents
		private void comConSerialRButton_CheckedChanged(object sender, EventArgs e)
		{
			DisableByRadioButton();
		}

		private void comConTCPRButton_CheckedChanged(object sender, EventArgs e)
		{
			DisableByRadioButton();
		}

		private void comConConnectCBox_CheckedChanged(object sender, EventArgs e)
		{
			OnCheckBoxChanged();
		}
		#endregion



	}
}
