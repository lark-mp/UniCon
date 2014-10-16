using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniCon.Joypad;

namespace UniCon
{
	[System.Runtime.InteropServices.ComVisibleAttribute(true)]
	public partial class UniConForm : Form
	{
		Joypad.JoypadController joypadController;
		Dictionary<string, TrackBar> tBarList;

		HeadTracker.HeadTracker headTracker;
		Interpreter.Interpreter interpreter;
		CV.CvOculus cvOculus;
		TelemetryVisualizer.TelemetryVisualizer telemetryVisualizer;

        System.IO.Stream stream;
        System.IO.StreamWriter sw;

		public UniConForm()
		{
			InitializeComponent();
		}

		private void UniConForm_Load(object sender, EventArgs e)
		{
            saveTelemetryDialog.InitialDirectory = System.Environment.CurrentDirectory;
            InitializeJoypad();
            InitializeHeadTracker();
			InitializeGMap();
			InitializeAttitudeImage();
			InitializeCvOculus();

			// interpreter は最後に初期化
			InitializeInterpreter();
		}

		#region Initialization
		private void InitializeJoypad()
		{
			var devNames = JoypadController.getDeviceNames();
			joypadCBox.Items.AddRange(devNames.ToArray());
			if (joypadCBox.Items.Count > 0)
			{
				joypadCBox.SelectedIndex = 0;
			}

			tBarList = new Dictionary<string, TrackBar>();
			tBarList.Add("pitch", trackBarJoyPitch);
			tBarList.Add("roll", trackBarJoyroll);
			tBarList.Add("yaw", trackBarJoyYaw);
			tBarList.Add("flaps", trackBarJoyFlaps);
			tBarList.Add("throttle", trackBarJoyThrottle);
		}

		private void InitializeHeadTracker()
		{
			headTracker = new HeadTracker.HeadTracker();
		}

		private void InitializeGMap()
		{
			gMapWebBrowser.ObjectForScripting = this;
			string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			gMapWebBrowser.Navigate(new Uri(@"file:///" + appPath + @"\..\..\..\test.html"));
		}

		private void InitializeAttitudeImage()
		{
            telemetryVisualizer = new TelemetryVisualizer.TelemetryVisualizer(rollPictureBox, pitchPictureBox, headingPictureBox, gMapWebBrowser);
		}

		private void InitializeInterpreter()
		{
			interpreter = new Interpreter.Interpreter(headTracker, cvOculus, cam640Button, CamFullHDButton, camOffButton, camStabilizeCheckBox);
		}

		private void InitializeCvOculus()
		{
			cvOculus = new CV.CvOculus();
		}
		#endregion

		#region Events
		private void joypadTimer_Tick(object sender, EventArgs e)
		{
			joypadController.UpdateJoyState();
			joypadController.UpdateJoyView();
            string decodeText = interpreter.DecodeCon();
			teleConComCon.SendLine(decodeText);
            hostTBox.AppendText(decodeText);
		}

		private void connectJoypadCBox_CheckedChanged(object sender, EventArgs e)
		{
			if (connectJoypadCBox.Checked)
			{
				try
				{
					joypadController = new JoypadController(joypadCBox.SelectedIndex, tBarList);
                    interpreter.SetJoypad(joypadController);
					joypadTimer.Enabled = true;
                    joypadCBox.Enabled = false;
				}
				catch (Exception)
				{
					MessageBox.Show("接続できませんでした。",
						"エラー",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
					connectJoypadCBox.Checked = false;
				}
			}
			else
			{
				joypadTimer.Enabled = false;
				joypadController = null;
                joypadCBox.Enabled = true;
            }
		}

		private void rollPictureBox_Paint(object sender, PaintEventArgs e)
		{
            telemetryVisualizer.PaintRoll(e.Graphics);
		}

		private void headingPictureBox_Paint(object sender, PaintEventArgs e)
		{
            telemetryVisualizer.PaintHeading(e.Graphics);
		}

		private void pitchPictureBox_Paint(object sender, PaintEventArgs e)
		{
            telemetryVisualizer.PaintPitch(e.Graphics);
		}

		private void headTrackerComCon_LineReceived(object sender, Interfaces.ReceiveLineEventArgs e)
		{
			headTracker.decodeLine(e.line);
		}

		private delegate void Delegate_RcvDataToTextBox(string data);
		private void RcvDataToTextBox(string line)
		{
			line += "\r\n";
			deviceTBox.AppendText(line);
			debugTBox.AppendText(line);
			rollPictureBox.Invalidate();
			pitchPictureBox.Invalidate();
			headingPictureBox.Invalidate();
		}
		private void teleConComCon_LineReceived(object sender, Interfaces.ReceiveLineEventArgs e)
		{
			Invoke(new Delegate_RcvDataToTextBox(RcvDataToTextBox), new Object[] { e.line });
			telemetryVisualizer.DecodeLine(e.line);
            if (sw != null)
            {
                sw.Write(e.line);                
            }
		}

		private void commandTBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == System.Windows.Forms.Keys.Enter)
			{
				teleConComCon.SendLine(commandTBox.Text);
                hostTBox.Text += commandTBox.Text;
				commandTBox.Clear();
			}
		}

        private void gMapCurPosBtn_Click(object sender, EventArgs e)
        {
            object[] args = { };
            gMapWebBrowser.Document.InvokeScript("currentPos", args);
        }

        private void gMapClearBtn_Click(object sender, EventArgs e)
        {
            object[] args = { };
            gMapWebBrowser.Document.InvokeScript("clearLine", args);
        }

        private void headTrackZeroBtn_Click(object sender, EventArgs e)
        {
            interpreter.SetHeadTrackerZero();
        }

        private void telemetryFileOpenBtn_Click(object sender, EventArgs e)
        {
            saveTelemetryDialog.FileName = telemetrySaveTBox.Text;
            saveTelemetryDialog.ShowDialog();
            telemetrySaveTBox.Text = saveTelemetryDialog.FileName;
            telemetrySaveTBox.Select(telemetrySaveTBox.Text.Length, 0);
        }

        private void telemetryFileRecCBox_CheckedChanged(object sender, EventArgs e)
        {
            if (telemetryFileRecCBox.Checked)
            {
                telemetrySaveTBox.Enabled = false;
                telemetryFileOpenBtn.Enabled = false;

                try
                {
                    stream = saveTelemetryDialog.OpenFile();
                    if (stream != null)
                    {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(stream);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ファイルのオープンに失敗しました。",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    telemetryFileRecCBox.Checked = false;
                }
            }
            else
            {
                telemetrySaveTBox.Enabled = true;
                telemetryFileOpenBtn.Enabled = true;
                if (sw != null)
                {
                    sw.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
            }

        }


        #endregion

	}
}
