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
            registryUpdate();
			InitializeComponent();
		}

        private static void registryUpdate()
        {
            //キー（HKEY_CURRENT_USER\Software\test\sub）を開く
            Microsoft.Win32.RegistryKey regkey =
                Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION");

            //REG_QWORDで書き込む
            regkey.SetValue("UniCon.exe", 0x2AF8, Microsoft.Win32.RegistryValueKind.DWord);
            regkey.SetValue("UniCon.vshost.exe", 0x2AF8, Microsoft.Win32.RegistryValueKind.DWord);

            //閉じる
            regkey.Close();
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
            telemetryVisualizer = new TelemetryVisualizer.TelemetryVisualizer(rollPictureBox, pitchPictureBox, headingPictureBox, gMapWebBrowser,this);
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
                hostTBox.Text += (commandTBox.Text+"\r\n");
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

        delegate void gMapInvokeDelegate(string function, object[] args);

        private void waypointAffirmButton_Click(object sender, EventArgs e)
        {
            
            object[] args = { };
            string waypointString = (string)gMapWebBrowser.Document.InvokeScript("getWaypointString", args);

            waypointString = waypointString.Replace("(","");
            waypointString = waypointString.Replace(" ","");
            string[] latlng = waypointString.Split(')');

            Console.WriteLine(waypointString);

            teleConComCon.SendLine("clearWaypoints");
            for (int i = 0; i < latlng.Length-1; i++)
            {
                string command = "setWaypoint " + latlng[i];
                teleConComCon.SendLine(command);
            }
        }

        public void setGpsStatusLabel(double lattitude,double longitude,double hdop,double height)
        {
            Object[] args = { lattitude, longitude, hdop, height };
            Invoke(new setGpsStatusLabelDelegate(setGpsStatusLabelDelegateFunc), args);
        }
        delegate void setGpsStatusLabelDelegate(double lattitude, double longitude, double hdop, double height);
        private void setGpsStatusLabelDelegateFunc(double lattitude, double longitude, double hdop, double height)
        {
            LatitudeLabel.Text = "Lat: " + lattitude.ToString("###.######") + "[deg]";
            LongitudeLabel.Text = "Lng: " + longitude.ToString("###.######") + "[deg]";
            hdopLabel.Text = "HDOP: " + hdop.ToString("###.##");
            altitudeLabel.Text = "Alt: " + height.ToString("###.##") + "[m]";
        }

        public void setAttitudeStatusLabel(double aoa,double roll, double pitch,double heading)
        {
            Object[] args = { aoa, roll, pitch, heading };
            Invoke(new setAttitudeStatusLabelDelegate(setAttitudeStatusLabelDelegateFunc),args);
        }
        delegate void setAttitudeStatusLabelDelegate(double aoa, double roll, double pitch, double heading);
        private void setAttitudeStatusLabelDelegateFunc(double aoa, double roll, double pitch, double heading)
        {
            aoaLabel.Text = "AOA: " + aoa.ToString("###.##");
            pitchLabel.Text = "Pitch: " + pitch.ToString("###.##");
            rollLabel.Text = "Roll: " + roll.ToString("###.##");
            headingLabel.Text = "Heading: " + heading.ToString("###.##");
        }

        public void setSpeedStatusLabel(double mpsXSpeed, double mpsYSpeed, double mpsZSpeed)
        {
            Object[] args = { mpsXSpeed, mpsYSpeed, mpsZSpeed };
            Invoke(new setSpeedStatusLabelDelegate(setSpeedStatusLabelDelegateFunc), args);

        }
        delegate void setSpeedStatusLabelDelegate(double mpsXSpeed,double mpsYSpeed,double mpsZSpeed);
        private void setSpeedStatusLabelDelegateFunc(double mpsXSpeed, double mpsYSpeed, double mpsZSpeed)
        {
            double mpsTotalSpeed = Math.Sqrt(mpsXSpeed*mpsXSpeed + mpsYSpeed*mpsYSpeed + mpsZSpeed*mpsZSpeed);
            double kphTotalSpeed = mpsTotalSpeed * 3.6;
            speedXLabel.Text = "X: " + mpsXSpeed.ToString("###.##") + "m/s";
            speedYLabel.Text = "Y: " + mpsYSpeed.ToString("###.##") + "m/s";
            speedZLabel.Text = "Z: " + mpsZSpeed.ToString("###.##") + "m/s";
            labelSpeedKmph.Text = kphTotalSpeed.ToString("###.##") +  "km/h";
            labelSpeedMps.Text = mpsTotalSpeed.ToString("###.##") + "m/s";
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
