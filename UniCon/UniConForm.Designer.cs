namespace UniCon
{
	partial class UniConForm
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

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UniConForm));
            this.joypadTimer = new System.Windows.Forms.Timer(this.components);
            this.joypadCBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.trackBarJoyPitch = new System.Windows.Forms.TrackBar();
            this.trackBarJoyroll = new System.Windows.Forms.TrackBar();
            this.trackBarJoyYaw = new System.Windows.Forms.TrackBar();
            this.trackBarJoyThrottle = new System.Windows.Forms.TrackBar();
            this.trackBarJoyFlaps = new System.Windows.Forms.TrackBar();
            this.connectJoypadCBox = new System.Windows.Forms.CheckBox();
            this.joypadGBox = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.teleConGBox = new System.Windows.Forms.GroupBox();
            this.telemetryFileRecCBox = new System.Windows.Forms.CheckBox();
            this.telemetryFileOpenBtn = new System.Windows.Forms.Button();
            this.telemetrySaveTBox = new System.Windows.Forms.TextBox();
            this.teleSaveLabel = new System.Windows.Forms.Label();
            this.viewTab = new System.Windows.Forms.TabControl();
            this.visualView = new System.Windows.Forms.TabPage();
            this.ClearWaypointButton = new System.Windows.Forms.Button();
            this.gMapWaypointAffirmButton = new System.Windows.Forms.Button();
            this.gMapCurPosBtn = new System.Windows.Forms.Button();
            this.gMapClearBtn = new System.Windows.Forms.Button();
            this.pitchPictureBox = new System.Windows.Forms.PictureBox();
            this.headingPictureBox = new System.Windows.Forms.PictureBox();
            this.rollPictureBox = new System.Windows.Forms.PictureBox();
            this.gMapWebBrowser = new System.Windows.Forms.WebBrowser();
            this.debugView = new System.Windows.Forms.TabPage();
            this.debugTBox = new System.Windows.Forms.TextBox();
            this.cameraGBox = new System.Windows.Forms.GroupBox();
            this.pitchHoldButton = new System.Windows.Forms.CheckBox();
            this.manualControlBtn = new System.Windows.Forms.CheckBox();
            this.resetStateBtn = new System.Windows.Forms.CheckBox();
            this.camStabilizeCheckBox = new System.Windows.Forms.CheckBox();
            this.cam640Button = new System.Windows.Forms.CheckBox();
            this.camOffButton = new System.Windows.Forms.CheckBox();
            this.CamFullHDButton = new System.Windows.Forms.CheckBox();
            this.commandTBox = new System.Windows.Forms.TextBox();
            this.commandLabel = new System.Windows.Forms.Label();
            this.deviceTBox = new System.Windows.Forms.TextBox();
            this.deviceLabel = new System.Windows.Forms.Label();
            this.hostLabel = new System.Windows.Forms.Label();
            this.hostTBox = new System.Windows.Forms.TextBox();
            this.aoaLabel = new System.Windows.Forms.Label();
            this.labelSpeedKmph = new System.Windows.Forms.Label();
            this.labelSpeedMps = new System.Windows.Forms.Label();
            this.statusGBox = new System.Windows.Forms.GroupBox();
            this.controlPhaseLabel = new System.Windows.Forms.Label();
            this.speedGBox = new System.Windows.Forms.GroupBox();
            this.speedZLabel = new System.Windows.Forms.Label();
            this.speedYLabel = new System.Windows.Forms.Label();
            this.speedXLabel = new System.Windows.Forms.Label();
            this.attitudeGBox = new System.Windows.Forms.GroupBox();
            this.headingLabel = new System.Windows.Forms.Label();
            this.pitchLabel = new System.Windows.Forms.Label();
            this.rollLabel = new System.Windows.Forms.Label();
            this.gpsGBox = new System.Windows.Forms.GroupBox();
            this.hdopLabel = new System.Windows.Forms.Label();
            this.altitudeLabel = new System.Windows.Forms.Label();
            this.LongitudeLabel = new System.Windows.Forms.Label();
            this.LatitudeLabel = new System.Windows.Forms.Label();
            this.saveTelemetryDialog = new System.Windows.Forms.SaveFileDialog();
            this.filterLabel = new System.Windows.Forms.Label();
            this.filterTBox = new System.Windows.Forms.TextBox();
            this.teleConComCon = new UniCon.CommunicationControl.CommunicationControl();
            this.headTrackerComCon = new UniCon.CommunicationControl.CommunicationControl();
            this.volatgeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJoyPitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJoyroll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJoyYaw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJoyThrottle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJoyFlaps)).BeginInit();
            this.joypadGBox.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.teleConGBox.SuspendLayout();
            this.viewTab.SuspendLayout();
            this.visualView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pitchPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headingPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rollPictureBox)).BeginInit();
            this.debugView.SuspendLayout();
            this.cameraGBox.SuspendLayout();
            this.statusGBox.SuspendLayout();
            this.speedGBox.SuspendLayout();
            this.attitudeGBox.SuspendLayout();
            this.gpsGBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // joypadTimer
            // 
            this.joypadTimer.Interval = 200;
            this.joypadTimer.Tick += new System.EventHandler(this.joypadTimer_Tick);
            // 
            // joypadCBox
            // 
            this.joypadCBox.FormattingEnabled = true;
            resources.ApplyResources(this.joypadCBox, "joypadCBox");
            this.joypadCBox.Name = "joypadCBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // trackBarJoyPitch
            // 
            resources.ApplyResources(this.trackBarJoyPitch, "trackBarJoyPitch");
            this.trackBarJoyPitch.Maximum = 255;
            this.trackBarJoyPitch.Name = "trackBarJoyPitch";
            // 
            // trackBarJoyroll
            // 
            resources.ApplyResources(this.trackBarJoyroll, "trackBarJoyroll");
            this.trackBarJoyroll.Maximum = 255;
            this.trackBarJoyroll.Name = "trackBarJoyroll";
            // 
            // trackBarJoyYaw
            // 
            resources.ApplyResources(this.trackBarJoyYaw, "trackBarJoyYaw");
            this.trackBarJoyYaw.Maximum = 255;
            this.trackBarJoyYaw.Name = "trackBarJoyYaw";
            // 
            // trackBarJoyThrottle
            // 
            resources.ApplyResources(this.trackBarJoyThrottle, "trackBarJoyThrottle");
            this.trackBarJoyThrottle.Maximum = 255;
            this.trackBarJoyThrottle.Name = "trackBarJoyThrottle";
            // 
            // trackBarJoyFlaps
            // 
            resources.ApplyResources(this.trackBarJoyFlaps, "trackBarJoyFlaps");
            this.trackBarJoyFlaps.Maximum = 255;
            this.trackBarJoyFlaps.Name = "trackBarJoyFlaps";
            // 
            // connectJoypadCBox
            // 
            resources.ApplyResources(this.connectJoypadCBox, "connectJoypadCBox");
            this.connectJoypadCBox.Name = "connectJoypadCBox";
            this.connectJoypadCBox.UseVisualStyleBackColor = true;
            this.connectJoypadCBox.CheckedChanged += new System.EventHandler(this.connectJoypadCBox_CheckedChanged);
            // 
            // joypadGBox
            // 
            this.joypadGBox.Controls.Add(this.connectJoypadCBox);
            this.joypadGBox.Controls.Add(this.label1);
            this.joypadGBox.Controls.Add(this.trackBarJoyFlaps);
            this.joypadGBox.Controls.Add(this.joypadCBox);
            this.joypadGBox.Controls.Add(this.trackBarJoyThrottle);
            this.joypadGBox.Controls.Add(this.label2);
            this.joypadGBox.Controls.Add(this.trackBarJoyYaw);
            this.joypadGBox.Controls.Add(this.label3);
            this.joypadGBox.Controls.Add(this.trackBarJoyroll);
            this.joypadGBox.Controls.Add(this.label4);
            this.joypadGBox.Controls.Add(this.trackBarJoyPitch);
            this.joypadGBox.Controls.Add(this.label5);
            this.joypadGBox.Controls.Add(this.label6);
            resources.ApplyResources(this.joypadGBox, "joypadGBox");
            this.joypadGBox.Name = "joypadGBox";
            this.joypadGBox.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.headTrackerComCon);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // teleConGBox
            // 
            this.teleConGBox.Controls.Add(this.telemetryFileRecCBox);
            this.teleConGBox.Controls.Add(this.telemetryFileOpenBtn);
            this.teleConGBox.Controls.Add(this.telemetrySaveTBox);
            this.teleConGBox.Controls.Add(this.teleSaveLabel);
            this.teleConGBox.Controls.Add(this.teleConComCon);
            resources.ApplyResources(this.teleConGBox, "teleConGBox");
            this.teleConGBox.Name = "teleConGBox";
            this.teleConGBox.TabStop = false;
            // 
            // telemetryFileRecCBox
            // 
            resources.ApplyResources(this.telemetryFileRecCBox, "telemetryFileRecCBox");
            this.telemetryFileRecCBox.Name = "telemetryFileRecCBox";
            this.telemetryFileRecCBox.UseVisualStyleBackColor = true;
            this.telemetryFileRecCBox.CheckedChanged += new System.EventHandler(this.telemetryFileRecCBox_CheckedChanged);
            // 
            // telemetryFileOpenBtn
            // 
            resources.ApplyResources(this.telemetryFileOpenBtn, "telemetryFileOpenBtn");
            this.telemetryFileOpenBtn.Name = "telemetryFileOpenBtn";
            this.telemetryFileOpenBtn.UseVisualStyleBackColor = true;
            this.telemetryFileOpenBtn.Click += new System.EventHandler(this.telemetryFileOpenBtn_Click);
            // 
            // telemetrySaveTBox
            // 
            resources.ApplyResources(this.telemetrySaveTBox, "telemetrySaveTBox");
            this.telemetrySaveTBox.Name = "telemetrySaveTBox";
            // 
            // teleSaveLabel
            // 
            resources.ApplyResources(this.teleSaveLabel, "teleSaveLabel");
            this.teleSaveLabel.Name = "teleSaveLabel";
            // 
            // viewTab
            // 
            this.viewTab.Controls.Add(this.visualView);
            this.viewTab.Controls.Add(this.debugView);
            resources.ApplyResources(this.viewTab, "viewTab");
            this.viewTab.Name = "viewTab";
            this.viewTab.SelectedIndex = 0;
            // 
            // visualView
            // 
            this.visualView.BackColor = System.Drawing.SystemColors.Control;
            this.visualView.Controls.Add(this.ClearWaypointButton);
            this.visualView.Controls.Add(this.gMapWaypointAffirmButton);
            this.visualView.Controls.Add(this.gMapCurPosBtn);
            this.visualView.Controls.Add(this.gMapClearBtn);
            this.visualView.Controls.Add(this.pitchPictureBox);
            this.visualView.Controls.Add(this.headingPictureBox);
            this.visualView.Controls.Add(this.rollPictureBox);
            this.visualView.Controls.Add(this.gMapWebBrowser);
            resources.ApplyResources(this.visualView, "visualView");
            this.visualView.Name = "visualView";
            // 
            // ClearWaypointButton
            // 
            resources.ApplyResources(this.ClearWaypointButton, "ClearWaypointButton");
            this.ClearWaypointButton.Name = "ClearWaypointButton";
            this.ClearWaypointButton.UseVisualStyleBackColor = true;
            this.ClearWaypointButton.Click += new System.EventHandler(this.ClearWaypointButton_Click);
            // 
            // gMapWaypointAffirmButton
            // 
            resources.ApplyResources(this.gMapWaypointAffirmButton, "gMapWaypointAffirmButton");
            this.gMapWaypointAffirmButton.Name = "gMapWaypointAffirmButton";
            this.gMapWaypointAffirmButton.UseVisualStyleBackColor = true;
            this.gMapWaypointAffirmButton.Click += new System.EventHandler(this.waypointAffirmButton_Click);
            // 
            // gMapCurPosBtn
            // 
            resources.ApplyResources(this.gMapCurPosBtn, "gMapCurPosBtn");
            this.gMapCurPosBtn.Name = "gMapCurPosBtn";
            this.gMapCurPosBtn.UseVisualStyleBackColor = true;
            this.gMapCurPosBtn.Click += new System.EventHandler(this.gMapCurPosBtn_Click);
            // 
            // gMapClearBtn
            // 
            resources.ApplyResources(this.gMapClearBtn, "gMapClearBtn");
            this.gMapClearBtn.Name = "gMapClearBtn";
            this.gMapClearBtn.UseVisualStyleBackColor = true;
            this.gMapClearBtn.Click += new System.EventHandler(this.gMapClearBtn_Click);
            // 
            // pitchPictureBox
            // 
            resources.ApplyResources(this.pitchPictureBox, "pitchPictureBox");
            this.pitchPictureBox.Name = "pitchPictureBox";
            this.pitchPictureBox.TabStop = false;
            this.pitchPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pitchPictureBox_Paint);
            // 
            // headingPictureBox
            // 
            resources.ApplyResources(this.headingPictureBox, "headingPictureBox");
            this.headingPictureBox.Name = "headingPictureBox";
            this.headingPictureBox.TabStop = false;
            this.headingPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.headingPictureBox_Paint);
            // 
            // rollPictureBox
            // 
            resources.ApplyResources(this.rollPictureBox, "rollPictureBox");
            this.rollPictureBox.Name = "rollPictureBox";
            this.rollPictureBox.TabStop = false;
            this.rollPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.rollPictureBox_Paint);
            // 
            // gMapWebBrowser
            // 
            resources.ApplyResources(this.gMapWebBrowser, "gMapWebBrowser");
            this.gMapWebBrowser.Name = "gMapWebBrowser";
            this.gMapWebBrowser.ScrollBarsEnabled = false;
            // 
            // debugView
            // 
            this.debugView.BackColor = System.Drawing.SystemColors.Control;
            this.debugView.Controls.Add(this.debugTBox);
            resources.ApplyResources(this.debugView, "debugView");
            this.debugView.Name = "debugView";
            // 
            // debugTBox
            // 
            this.debugTBox.HideSelection = false;
            resources.ApplyResources(this.debugTBox, "debugTBox");
            this.debugTBox.Name = "debugTBox";
            // 
            // cameraGBox
            // 
            this.cameraGBox.Controls.Add(this.pitchHoldButton);
            this.cameraGBox.Controls.Add(this.manualControlBtn);
            this.cameraGBox.Controls.Add(this.resetStateBtn);
            this.cameraGBox.Controls.Add(this.camStabilizeCheckBox);
            this.cameraGBox.Controls.Add(this.cam640Button);
            this.cameraGBox.Controls.Add(this.camOffButton);
            this.cameraGBox.Controls.Add(this.CamFullHDButton);
            resources.ApplyResources(this.cameraGBox, "cameraGBox");
            this.cameraGBox.Name = "cameraGBox";
            this.cameraGBox.TabStop = false;
            // 
            // pitchHoldButton
            // 
            resources.ApplyResources(this.pitchHoldButton, "pitchHoldButton");
            this.pitchHoldButton.Name = "pitchHoldButton";
            this.pitchHoldButton.UseVisualStyleBackColor = true;
            // 
            // manualControlBtn
            // 
            resources.ApplyResources(this.manualControlBtn, "manualControlBtn");
            this.manualControlBtn.Name = "manualControlBtn";
            this.manualControlBtn.UseVisualStyleBackColor = true;
            // 
            // resetStateBtn
            // 
            resources.ApplyResources(this.resetStateBtn, "resetStateBtn");
            this.resetStateBtn.Name = "resetStateBtn";
            this.resetStateBtn.UseVisualStyleBackColor = true;
            // 
            // camStabilizeCheckBox
            // 
            resources.ApplyResources(this.camStabilizeCheckBox, "camStabilizeCheckBox");
            this.camStabilizeCheckBox.Name = "camStabilizeCheckBox";
            this.camStabilizeCheckBox.UseVisualStyleBackColor = true;
            // 
            // cam640Button
            // 
            resources.ApplyResources(this.cam640Button, "cam640Button");
            this.cam640Button.Name = "cam640Button";
            this.cam640Button.UseVisualStyleBackColor = true;
            // 
            // camOffButton
            // 
            resources.ApplyResources(this.camOffButton, "camOffButton");
            this.camOffButton.Name = "camOffButton";
            this.camOffButton.UseVisualStyleBackColor = true;
            // 
            // CamFullHDButton
            // 
            resources.ApplyResources(this.CamFullHDButton, "CamFullHDButton");
            this.CamFullHDButton.Name = "CamFullHDButton";
            this.CamFullHDButton.UseVisualStyleBackColor = true;
            // 
            // commandTBox
            // 
            resources.ApplyResources(this.commandTBox, "commandTBox");
            this.commandTBox.Name = "commandTBox";
            this.commandTBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.commandTBox_KeyDown);
            // 
            // commandLabel
            // 
            resources.ApplyResources(this.commandLabel, "commandLabel");
            this.commandLabel.Name = "commandLabel";
            // 
            // deviceTBox
            // 
            this.deviceTBox.HideSelection = false;
            resources.ApplyResources(this.deviceTBox, "deviceTBox");
            this.deviceTBox.Name = "deviceTBox";
            // 
            // deviceLabel
            // 
            resources.ApplyResources(this.deviceLabel, "deviceLabel");
            this.deviceLabel.Name = "deviceLabel";
            // 
            // hostLabel
            // 
            resources.ApplyResources(this.hostLabel, "hostLabel");
            this.hostLabel.Name = "hostLabel";
            // 
            // hostTBox
            // 
            this.hostTBox.HideSelection = false;
            resources.ApplyResources(this.hostTBox, "hostTBox");
            this.hostTBox.Name = "hostTBox";
            // 
            // aoaLabel
            // 
            resources.ApplyResources(this.aoaLabel, "aoaLabel");
            this.aoaLabel.Name = "aoaLabel";
            // 
            // labelSpeedKmph
            // 
            resources.ApplyResources(this.labelSpeedKmph, "labelSpeedKmph");
            this.labelSpeedKmph.Name = "labelSpeedKmph";
            // 
            // labelSpeedMps
            // 
            resources.ApplyResources(this.labelSpeedMps, "labelSpeedMps");
            this.labelSpeedMps.Name = "labelSpeedMps";
            // 
            // statusGBox
            // 
            this.statusGBox.Controls.Add(this.controlPhaseLabel);
            this.statusGBox.Controls.Add(this.volatgeLabel);
            this.statusGBox.Controls.Add(this.speedGBox);
            this.statusGBox.Controls.Add(this.attitudeGBox);
            this.statusGBox.Controls.Add(this.gpsGBox);
            resources.ApplyResources(this.statusGBox, "statusGBox");
            this.statusGBox.Name = "statusGBox";
            this.statusGBox.TabStop = false;
            // 
            // controlPhaseLabel
            // 
            resources.ApplyResources(this.controlPhaseLabel, "controlPhaseLabel");
            this.controlPhaseLabel.Name = "controlPhaseLabel";
            // 
            // speedGBox
            // 
            this.speedGBox.Controls.Add(this.speedZLabel);
            this.speedGBox.Controls.Add(this.speedYLabel);
            this.speedGBox.Controls.Add(this.speedXLabel);
            this.speedGBox.Controls.Add(this.labelSpeedKmph);
            this.speedGBox.Controls.Add(this.labelSpeedMps);
            resources.ApplyResources(this.speedGBox, "speedGBox");
            this.speedGBox.Name = "speedGBox";
            this.speedGBox.TabStop = false;
            // 
            // speedZLabel
            // 
            resources.ApplyResources(this.speedZLabel, "speedZLabel");
            this.speedZLabel.Name = "speedZLabel";
            // 
            // speedYLabel
            // 
            resources.ApplyResources(this.speedYLabel, "speedYLabel");
            this.speedYLabel.Name = "speedYLabel";
            // 
            // speedXLabel
            // 
            resources.ApplyResources(this.speedXLabel, "speedXLabel");
            this.speedXLabel.Name = "speedXLabel";
            // 
            // attitudeGBox
            // 
            this.attitudeGBox.Controls.Add(this.headingLabel);
            this.attitudeGBox.Controls.Add(this.aoaLabel);
            this.attitudeGBox.Controls.Add(this.pitchLabel);
            this.attitudeGBox.Controls.Add(this.rollLabel);
            resources.ApplyResources(this.attitudeGBox, "attitudeGBox");
            this.attitudeGBox.Name = "attitudeGBox";
            this.attitudeGBox.TabStop = false;
            // 
            // headingLabel
            // 
            resources.ApplyResources(this.headingLabel, "headingLabel");
            this.headingLabel.Name = "headingLabel";
            // 
            // pitchLabel
            // 
            resources.ApplyResources(this.pitchLabel, "pitchLabel");
            this.pitchLabel.Name = "pitchLabel";
            // 
            // rollLabel
            // 
            resources.ApplyResources(this.rollLabel, "rollLabel");
            this.rollLabel.Name = "rollLabel";
            // 
            // gpsGBox
            // 
            this.gpsGBox.Controls.Add(this.hdopLabel);
            this.gpsGBox.Controls.Add(this.altitudeLabel);
            this.gpsGBox.Controls.Add(this.LongitudeLabel);
            this.gpsGBox.Controls.Add(this.LatitudeLabel);
            resources.ApplyResources(this.gpsGBox, "gpsGBox");
            this.gpsGBox.Name = "gpsGBox";
            this.gpsGBox.TabStop = false;
            // 
            // hdopLabel
            // 
            resources.ApplyResources(this.hdopLabel, "hdopLabel");
            this.hdopLabel.Name = "hdopLabel";
            // 
            // altitudeLabel
            // 
            resources.ApplyResources(this.altitudeLabel, "altitudeLabel");
            this.altitudeLabel.Name = "altitudeLabel";
            // 
            // LongitudeLabel
            // 
            resources.ApplyResources(this.LongitudeLabel, "LongitudeLabel");
            this.LongitudeLabel.Name = "LongitudeLabel";
            // 
            // LatitudeLabel
            // 
            resources.ApplyResources(this.LatitudeLabel, "LatitudeLabel");
            this.LatitudeLabel.Name = "LatitudeLabel";
            // 
            // saveTelemetryDialog
            // 
            resources.ApplyResources(this.saveTelemetryDialog, "saveTelemetryDialog");
            this.saveTelemetryDialog.FilterIndex = 0;
            this.saveTelemetryDialog.RestoreDirectory = true;
            // 
            // filterLabel
            // 
            resources.ApplyResources(this.filterLabel, "filterLabel");
            this.filterLabel.Name = "filterLabel";
            // 
            // filterTBox
            // 
            resources.ApplyResources(this.filterTBox, "filterTBox");
            this.filterTBox.Name = "filterTBox";
            // 
            // teleConComCon
            // 
            resources.ApplyResources(this.teleConComCon, "teleConComCon");
            this.teleConComCon.Name = "teleConComCon";
            this.teleConComCon.LineReceived += new System.EventHandler<UniCon.Interfaces.ReceiveLineEventArgs>(this.teleConComCon_LineReceived);
            // 
            // headTrackerComCon
            // 
            resources.ApplyResources(this.headTrackerComCon, "headTrackerComCon");
            this.headTrackerComCon.Name = "headTrackerComCon";
            this.headTrackerComCon.LineReceived += new System.EventHandler<UniCon.Interfaces.ReceiveLineEventArgs>(this.headTrackerComCon_LineReceived);
            // 
            // volatgeLabel
            // 
            resources.ApplyResources(this.volatgeLabel, "volatgeLabel");
            this.volatgeLabel.Name = "volatgeLabel";
            // 
            // UniConForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.filterTBox);
            this.Controls.Add(this.filterLabel);
            this.Controls.Add(this.hostTBox);
            this.Controls.Add(this.hostLabel);
            this.Controls.Add(this.deviceLabel);
            this.Controls.Add(this.deviceTBox);
            this.Controls.Add(this.commandLabel);
            this.Controls.Add(this.commandTBox);
            this.Controls.Add(this.cameraGBox);
            this.Controls.Add(this.viewTab);
            this.Controls.Add(this.teleConGBox);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.joypadGBox);
            this.Controls.Add(this.statusGBox);
            this.Name = "UniConForm";
            this.Load += new System.EventHandler(this.UniConForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJoyPitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJoyroll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJoyYaw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJoyThrottle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJoyFlaps)).EndInit();
            this.joypadGBox.ResumeLayout(false);
            this.joypadGBox.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.teleConGBox.ResumeLayout(false);
            this.teleConGBox.PerformLayout();
            this.viewTab.ResumeLayout(false);
            this.visualView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pitchPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.headingPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rollPictureBox)).EndInit();
            this.debugView.ResumeLayout(false);
            this.debugView.PerformLayout();
            this.cameraGBox.ResumeLayout(false);
            this.cameraGBox.PerformLayout();
            this.statusGBox.ResumeLayout(false);
            this.statusGBox.PerformLayout();
            this.speedGBox.ResumeLayout(false);
            this.speedGBox.PerformLayout();
            this.attitudeGBox.ResumeLayout(false);
            this.attitudeGBox.PerformLayout();
            this.gpsGBox.ResumeLayout(false);
            this.gpsGBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Timer joypadTimer;
        private System.Windows.Forms.ComboBox joypadCBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar trackBarJoyPitch;
        private System.Windows.Forms.TrackBar trackBarJoyroll;
        private System.Windows.Forms.TrackBar trackBarJoyYaw;
        private System.Windows.Forms.TrackBar trackBarJoyThrottle;
        private System.Windows.Forms.TrackBar trackBarJoyFlaps;
        private System.Windows.Forms.CheckBox connectJoypadCBox;
        private System.Windows.Forms.GroupBox joypadGBox;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox teleConGBox;
		private CommunicationControl.CommunicationControl teleConComCon;
		private CommunicationControl.CommunicationControl headTrackerComCon;
		private System.Windows.Forms.TabControl viewTab;
		private System.Windows.Forms.TabPage visualView;
		private System.Windows.Forms.PictureBox pitchPictureBox;
		private System.Windows.Forms.PictureBox headingPictureBox;
		private System.Windows.Forms.PictureBox rollPictureBox;
		private System.Windows.Forms.WebBrowser gMapWebBrowser;
		private System.Windows.Forms.TabPage debugView;
		private System.Windows.Forms.GroupBox cameraGBox;
		private System.Windows.Forms.TextBox commandTBox;
		private System.Windows.Forms.Label commandLabel;
		private System.Windows.Forms.TextBox deviceTBox;
		private System.Windows.Forms.Label deviceLabel;
		private System.Windows.Forms.Label hostLabel;
		private System.Windows.Forms.TextBox hostTBox;
		private System.Windows.Forms.Label aoaLabel;
		private System.Windows.Forms.Label labelSpeedKmph;
		private System.Windows.Forms.Label labelSpeedMps;
		private System.Windows.Forms.GroupBox statusGBox;
		private System.Windows.Forms.TextBox debugTBox;
		private System.Windows.Forms.GroupBox speedGBox;
		private System.Windows.Forms.GroupBox attitudeGBox;
		private System.Windows.Forms.GroupBox gpsGBox;
		private System.Windows.Forms.TextBox telemetrySaveTBox;
		private System.Windows.Forms.Label teleSaveLabel;
		private System.Windows.Forms.Button gMapClearBtn;
		private System.Windows.Forms.Button gMapCurPosBtn;
		private System.Windows.Forms.CheckBox camStabilizeCheckBox;
		private System.Windows.Forms.CheckBox cam640Button;
		private System.Windows.Forms.CheckBox camOffButton;
        private System.Windows.Forms.CheckBox CamFullHDButton;
        private System.Windows.Forms.CheckBox telemetryFileRecCBox;
        private System.Windows.Forms.Button telemetryFileOpenBtn;
        private System.Windows.Forms.SaveFileDialog saveTelemetryDialog;
        private System.Windows.Forms.Label hdopLabel;
        private System.Windows.Forms.Label altitudeLabel;
        private System.Windows.Forms.Label LongitudeLabel;
        private System.Windows.Forms.Label LatitudeLabel;
        private System.Windows.Forms.Label speedZLabel;
        private System.Windows.Forms.Label speedYLabel;
        private System.Windows.Forms.Label speedXLabel;
        private System.Windows.Forms.Label headingLabel;
        private System.Windows.Forms.Label pitchLabel;
        private System.Windows.Forms.Label rollLabel;
        private System.Windows.Forms.Button gMapWaypointAffirmButton;
        private System.Windows.Forms.Label filterLabel;
        private System.Windows.Forms.TextBox filterTBox;
        private System.Windows.Forms.Button ClearWaypointButton;
        private System.Windows.Forms.CheckBox resetStateBtn;
        private System.Windows.Forms.CheckBox manualControlBtn;
        private System.Windows.Forms.Label controlPhaseLabel;
        private System.Windows.Forms.CheckBox pitchHoldButton;
        private System.Windows.Forms.Label volatgeLabel;

	}
}

