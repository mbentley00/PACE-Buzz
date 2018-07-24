namespace PACEBuzz
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.imgPlaySound = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbSound = new System.Windows.Forms.ComboBox();
            this.cmbBuzzerGroup = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkPreviousBuzzControl = new System.Windows.Forms.CheckBox();
            this.chkPreviousBuzzShift = new System.Windows.Forms.CheckBox();
            this.cmbPreviousBuzzShortcutKey = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkLightCheckControl = new System.Windows.Forms.CheckBox();
            this.chkLightCheckShift = new System.Windows.Forms.CheckBox();
            this.cmbLightcheckShortcutKey = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkCountdownControl = new System.Windows.Forms.CheckBox();
            this.chkCountdownShift = new System.Windows.Forms.CheckBox();
            this.cmbCountdownShortcutKey = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkNextBuzzControl = new System.Windows.Forms.CheckBox();
            this.chkNextBuzzShift = new System.Windows.Forms.CheckBox();
            this.comboNextBuzzShortcutKey = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkResetControl = new System.Windows.Forms.CheckBox();
            this.chkResetShift = new System.Windows.Forms.CheckBox();
            this.cmbResetKey = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkShowInTaskbar = new System.Windows.Forms.CheckBox();
            this.chkBuzzerCancel = new System.Windows.Forms.CheckBox();
            this.chkBlink = new System.Windows.Forms.CheckBox();
            this.txtCountdownLength = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.chkFirstPlayerCanCancel = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkQuitPrompt = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.opPS2 = new System.Windows.Forms.RadioButton();
            this.opArcade = new System.Windows.Forms.RadioButton();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgPlaySound)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.imgPlaySound);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.cmbSound);
            this.groupBox4.Controls.Add(this.cmbBuzzerGroup);
            this.groupBox4.Location = new System.Drawing.Point(14, 97);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(794, 95);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Buzzer Noise";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // imgPlaySound
            // 
            this.imgPlaySound.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imgPlaySound.Image = global::PACEBuzz.Properties.Resources.sound;
            this.imgPlaySound.Location = new System.Drawing.Point(399, 39);
            this.imgPlaySound.Name = "imgPlaySound";
            this.imgPlaySound.Size = new System.Drawing.Size(19, 17);
            this.imgPlaySound.TabIndex = 30;
            this.imgPlaySound.TabStop = false;
            this.imgPlaySound.Click += new System.EventHandler(this.imgPlaySound_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(201, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 17);
            this.label7.TabIndex = 29;
            this.label7.Text = "Sound";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 17);
            this.label6.TabIndex = 28;
            this.label6.Text = "Buzzer Group:";
            // 
            // cmbSound
            // 
            this.cmbSound.FormattingEnabled = true;
            this.cmbSound.Location = new System.Drawing.Point(203, 39);
            this.cmbSound.Name = "cmbSound";
            this.cmbSound.Size = new System.Drawing.Size(188, 25);
            this.cmbSound.TabIndex = 10;
            this.cmbSound.SelectedIndexChanged += new System.EventHandler(this.cmbSound_SelectedIndexChanged);
            // 
            // cmbBuzzerGroup
            // 
            this.cmbBuzzerGroup.FormattingEnabled = true;
            this.cmbBuzzerGroup.Location = new System.Drawing.Point(7, 39);
            this.cmbBuzzerGroup.Name = "cmbBuzzerGroup";
            this.cmbBuzzerGroup.Size = new System.Drawing.Size(188, 25);
            this.cmbBuzzerGroup.TabIndex = 0;
            this.cmbBuzzerGroup.SelectedIndexChanged += new System.EventHandler(this.cmbBuzzerGroup_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkPreviousBuzzControl);
            this.groupBox3.Controls.Add(this.chkPreviousBuzzShift);
            this.groupBox3.Controls.Add(this.cmbPreviousBuzzShortcutKey);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.chkLightCheckControl);
            this.groupBox3.Controls.Add(this.chkLightCheckShift);
            this.groupBox3.Controls.Add(this.cmbLightcheckShortcutKey);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.chkCountdownControl);
            this.groupBox3.Controls.Add(this.chkCountdownShift);
            this.groupBox3.Controls.Add(this.cmbCountdownShortcutKey);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.chkNextBuzzControl);
            this.groupBox3.Controls.Add(this.chkNextBuzzShift);
            this.groupBox3.Controls.Add(this.comboNextBuzzShortcutKey);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.chkResetControl);
            this.groupBox3.Controls.Add(this.chkResetShift);
            this.groupBox3.Controls.Add(this.cmbResetKey);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(15, 198);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(544, 232);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Shortcuts";
            // 
            // chkPreviousBuzzControl
            // 
            this.chkPreviousBuzzControl.AutoSize = true;
            this.chkPreviousBuzzControl.Location = new System.Drawing.Point(465, 156);
            this.chkPreviousBuzzControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkPreviousBuzzControl.Name = "chkPreviousBuzzControl";
            this.chkPreviousBuzzControl.Size = new System.Drawing.Size(78, 21);
            this.chkPreviousBuzzControl.TabIndex = 130;
            this.chkPreviousBuzzControl.Text = "Control";
            this.chkPreviousBuzzControl.UseVisualStyleBackColor = true;
            this.chkPreviousBuzzControl.CheckedChanged += new System.EventHandler(this.chkPreviousBuzzControl_CheckedChanged);
            // 
            // chkPreviousBuzzShift
            // 
            this.chkPreviousBuzzShift.AutoSize = true;
            this.chkPreviousBuzzShift.Location = new System.Drawing.Point(398, 156);
            this.chkPreviousBuzzShift.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkPreviousBuzzShift.Name = "chkPreviousBuzzShift";
            this.chkPreviousBuzzShift.Size = new System.Drawing.Size(60, 21);
            this.chkPreviousBuzzShift.TabIndex = 120;
            this.chkPreviousBuzzShift.Text = "Shift";
            this.chkPreviousBuzzShift.UseVisualStyleBackColor = true;
            this.chkPreviousBuzzShift.CheckedChanged += new System.EventHandler(this.chkPreviousBuzzShift_CheckedChanged);
            // 
            // cmbPreviousBuzzShortcutKey
            // 
            this.cmbPreviousBuzzShortcutKey.FormattingEnabled = true;
            this.cmbPreviousBuzzShortcutKey.Location = new System.Drawing.Point(232, 154);
            this.cmbPreviousBuzzShortcutKey.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbPreviousBuzzShortcutKey.Name = "cmbPreviousBuzzShortcutKey";
            this.cmbPreviousBuzzShortcutKey.Size = new System.Drawing.Size(152, 25);
            this.cmbPreviousBuzzShortcutKey.TabIndex = 110;
            this.cmbPreviousBuzzShortcutKey.SelectedIndexChanged += new System.EventHandler(this.cmbPreviousBuzzShortcutKey_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 157);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 17);
            this.label9.TabIndex = 101;
            this.label9.Text = "Previous Buzz";
            // 
            // chkLightCheckControl
            // 
            this.chkLightCheckControl.AutoSize = true;
            this.chkLightCheckControl.Location = new System.Drawing.Point(465, 127);
            this.chkLightCheckControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkLightCheckControl.Name = "chkLightCheckControl";
            this.chkLightCheckControl.Size = new System.Drawing.Size(78, 21);
            this.chkLightCheckControl.TabIndex = 100;
            this.chkLightCheckControl.Text = "Control";
            this.chkLightCheckControl.UseVisualStyleBackColor = true;
            this.chkLightCheckControl.CheckedChanged += new System.EventHandler(this.chkLightCheckControl_CheckedChanged);
            // 
            // chkLightCheckShift
            // 
            this.chkLightCheckShift.AutoSize = true;
            this.chkLightCheckShift.Location = new System.Drawing.Point(398, 127);
            this.chkLightCheckShift.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkLightCheckShift.Name = "chkLightCheckShift";
            this.chkLightCheckShift.Size = new System.Drawing.Size(60, 21);
            this.chkLightCheckShift.TabIndex = 90;
            this.chkLightCheckShift.Text = "Shift";
            this.chkLightCheckShift.UseVisualStyleBackColor = true;
            this.chkLightCheckShift.CheckedChanged += new System.EventHandler(this.chkLightCheckShift_CheckedChanged);
            // 
            // cmbLightcheckShortcutKey
            // 
            this.cmbLightcheckShortcutKey.FormattingEnabled = true;
            this.cmbLightcheckShortcutKey.Location = new System.Drawing.Point(232, 125);
            this.cmbLightcheckShortcutKey.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbLightcheckShortcutKey.Name = "cmbLightcheckShortcutKey";
            this.cmbLightcheckShortcutKey.Size = new System.Drawing.Size(152, 25);
            this.cmbLightcheckShortcutKey.TabIndex = 80;
            this.cmbLightcheckShortcutKey.SelectedIndexChanged += new System.EventHandler(this.cmbLightcheckShortcutKey_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 17);
            this.label5.TabIndex = 38;
            this.label5.Text = "Light Check";
            // 
            // chkCountdownControl
            // 
            this.chkCountdownControl.AutoSize = true;
            this.chkCountdownControl.Location = new System.Drawing.Point(465, 92);
            this.chkCountdownControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkCountdownControl.Name = "chkCountdownControl";
            this.chkCountdownControl.Size = new System.Drawing.Size(78, 21);
            this.chkCountdownControl.TabIndex = 70;
            this.chkCountdownControl.Text = "Control";
            this.chkCountdownControl.UseVisualStyleBackColor = true;
            this.chkCountdownControl.CheckedChanged += new System.EventHandler(this.chkCountdownControl_CheckedChanged);
            // 
            // chkCountdownShift
            // 
            this.chkCountdownShift.AutoSize = true;
            this.chkCountdownShift.Location = new System.Drawing.Point(398, 92);
            this.chkCountdownShift.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkCountdownShift.Name = "chkCountdownShift";
            this.chkCountdownShift.Size = new System.Drawing.Size(60, 21);
            this.chkCountdownShift.TabIndex = 60;
            this.chkCountdownShift.Text = "Shift";
            this.chkCountdownShift.UseVisualStyleBackColor = true;
            this.chkCountdownShift.CheckedChanged += new System.EventHandler(this.chkCountdownShift_CheckedChanged);
            // 
            // cmbCountdownShortcutKey
            // 
            this.cmbCountdownShortcutKey.FormattingEnabled = true;
            this.cmbCountdownShortcutKey.Location = new System.Drawing.Point(232, 89);
            this.cmbCountdownShortcutKey.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbCountdownShortcutKey.Name = "cmbCountdownShortcutKey";
            this.cmbCountdownShortcutKey.Size = new System.Drawing.Size(152, 25);
            this.cmbCountdownShortcutKey.TabIndex = 60;
            this.cmbCountdownShortcutKey.SelectedIndexChanged += new System.EventHandler(this.cmbCountdownShortcutKey_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 17);
            this.label2.TabIndex = 34;
            this.label2.Text = "Start/Stop Countdown";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(553, 17);
            this.label4.TabIndex = 30;
            this.label4.Text = "Note: D keys are digits.  \"D1\" corresponds to the 1 key.   Do not overload a shor" +
    "tcut key.";
            // 
            // chkNextBuzzControl
            // 
            this.chkNextBuzzControl.AutoSize = true;
            this.chkNextBuzzControl.Location = new System.Drawing.Point(465, 60);
            this.chkNextBuzzControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkNextBuzzControl.Name = "chkNextBuzzControl";
            this.chkNextBuzzControl.Size = new System.Drawing.Size(78, 21);
            this.chkNextBuzzControl.TabIndex = 50;
            this.chkNextBuzzControl.Text = "Control";
            this.chkNextBuzzControl.UseVisualStyleBackColor = true;
            this.chkNextBuzzControl.CheckedChanged += new System.EventHandler(this.chkNextBuzzControl_CheckedChanged);
            // 
            // chkNextBuzzShift
            // 
            this.chkNextBuzzShift.AutoSize = true;
            this.chkNextBuzzShift.Location = new System.Drawing.Point(398, 60);
            this.chkNextBuzzShift.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkNextBuzzShift.Name = "chkNextBuzzShift";
            this.chkNextBuzzShift.Size = new System.Drawing.Size(60, 21);
            this.chkNextBuzzShift.TabIndex = 40;
            this.chkNextBuzzShift.Text = "Shift";
            this.chkNextBuzzShift.UseVisualStyleBackColor = true;
            this.chkNextBuzzShift.CheckedChanged += new System.EventHandler(this.chkNextBuzzShift_CheckedChanged);
            // 
            // comboNextBuzzShortcutKey
            // 
            this.comboNextBuzzShortcutKey.FormattingEnabled = true;
            this.comboNextBuzzShortcutKey.Location = new System.Drawing.Point(232, 58);
            this.comboNextBuzzShortcutKey.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboNextBuzzShortcutKey.Name = "comboNextBuzzShortcutKey";
            this.comboNextBuzzShortcutKey.Size = new System.Drawing.Size(152, 25);
            this.comboNextBuzzShortcutKey.TabIndex = 30;
            this.comboNextBuzzShortcutKey.SelectedIndexChanged += new System.EventHandler(this.comboNextBuzzShortcutKey_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 17);
            this.label3.TabIndex = 26;
            this.label3.Text = "Next Buzz:";
            // 
            // chkResetControl
            // 
            this.chkResetControl.AutoSize = true;
            this.chkResetControl.Location = new System.Drawing.Point(465, 27);
            this.chkResetControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkResetControl.Name = "chkResetControl";
            this.chkResetControl.Size = new System.Drawing.Size(78, 21);
            this.chkResetControl.TabIndex = 20;
            this.chkResetControl.Text = "Control";
            this.chkResetControl.UseVisualStyleBackColor = true;
            this.chkResetControl.CheckedChanged += new System.EventHandler(this.chkResetControl_CheckedChanged);
            // 
            // chkResetShift
            // 
            this.chkResetShift.AutoSize = true;
            this.chkResetShift.Location = new System.Drawing.Point(398, 28);
            this.chkResetShift.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkResetShift.Name = "chkResetShift";
            this.chkResetShift.Size = new System.Drawing.Size(60, 21);
            this.chkResetShift.TabIndex = 10;
            this.chkResetShift.Text = "Shift";
            this.chkResetShift.UseVisualStyleBackColor = true;
            this.chkResetShift.CheckedChanged += new System.EventHandler(this.chkResetShift_CheckedChanged);
            // 
            // cmbResetKey
            // 
            this.cmbResetKey.FormattingEnabled = true;
            this.cmbResetKey.Location = new System.Drawing.Point(232, 26);
            this.cmbResetKey.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbResetKey.Name = "cmbResetKey";
            this.cmbResetKey.Size = new System.Drawing.Size(152, 25);
            this.cmbResetKey.TabIndex = 0;
            this.cmbResetKey.SelectedIndexChanged += new System.EventHandler(this.cmbResetKey_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(153, 17);
            this.label8.TabIndex = 22;
            this.label8.Text = "Reset (clear all buzzes):";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // chkShowInTaskbar
            // 
            this.chkShowInTaskbar.AutoSize = true;
            this.chkShowInTaskbar.Location = new System.Drawing.Point(6, 40);
            this.chkShowInTaskbar.Name = "chkShowInTaskbar";
            this.chkShowInTaskbar.Size = new System.Drawing.Size(136, 21);
            this.chkShowInTaskbar.TabIndex = 10;
            this.chkShowInTaskbar.Text = "Show In Taskbar";
            this.chkShowInTaskbar.UseVisualStyleBackColor = true;
            this.chkShowInTaskbar.CheckedChanged += new System.EventHandler(this.chkShowInTaskbar_CheckedChanged);
            // 
            // chkBuzzerCancel
            // 
            this.chkBuzzerCancel.AutoSize = true;
            this.chkBuzzerCancel.Location = new System.Drawing.Point(6, 133);
            this.chkBuzzerCancel.Name = "chkBuzzerCancel";
            this.chkBuzzerCancel.Size = new System.Drawing.Size(202, 21);
            this.chkBuzzerCancel.TabIndex = 40;
            this.chkBuzzerCancel.Text = "Allow player buzzer cancels";
            this.chkBuzzerCancel.UseVisualStyleBackColor = true;
            this.chkBuzzerCancel.CheckedChanged += new System.EventHandler(this.chkBuzzerCancel_CheckedChanged);
            // 
            // chkBlink
            // 
            this.chkBlink.AutoSize = true;
            this.chkBlink.Location = new System.Drawing.Point(6, 65);
            this.chkBlink.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkBlink.Name = "chkBlink";
            this.chkBlink.Size = new System.Drawing.Size(169, 21);
            this.chkBlink.TabIndex = 20;
            this.chkBlink.Text = "Blink Window on Buzz";
            this.chkBlink.UseVisualStyleBackColor = true;
            this.chkBlink.CheckedChanged += new System.EventHandler(this.chkBlink_CheckedChanged);
            // 
            // txtCountdownLength
            // 
            this.txtCountdownLength.Location = new System.Drawing.Point(196, 192);
            this.txtCountdownLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCountdownLength.Name = "txtCountdownLength";
            this.txtCountdownLength.Size = new System.Drawing.Size(28, 23);
            this.txtCountdownLength.TabIndex = 60;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 195);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 17);
            this.label1.TabIndex = 32;
            this.label1.Text = "Countdown Length in Seconds:";
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.AutoSize = true;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(6, 18);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(125, 21);
            this.chkAlwaysOnTop.TabIndex = 0;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.UseVisualStyleBackColor = true;
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(628, 433);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(87, 32);
            this.cmdCancel.TabIndex = 28;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(721, 433);
            this.cmdSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(87, 32);
            this.cmdSave.TabIndex = 27;
            this.cmdSave.Text = "&Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // chkFirstPlayerCanCancel
            // 
            this.chkFirstPlayerCanCancel.AutoSize = true;
            this.chkFirstPlayerCanCancel.Location = new System.Drawing.Point(31, 156);
            this.chkFirstPlayerCanCancel.Name = "chkFirstPlayerCanCancel";
            this.chkFirstPlayerCanCancel.Size = new System.Drawing.Size(209, 21);
            this.chkFirstPlayerCanCancel.TabIndex = 50;
            this.chkFirstPlayerCanCancel.Text = "1st buzzing player can cancel";
            this.chkFirstPlayerCanCancel.UseVisualStyleBackColor = true;
            this.chkFirstPlayerCanCancel.CheckedChanged += new System.EventHandler(this.chkFirstPlayerCanCancel_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkQuitPrompt);
            this.groupBox1.Controls.Add(this.chkFirstPlayerCanCancel);
            this.groupBox1.Controls.Add(this.chkAlwaysOnTop);
            this.groupBox1.Controls.Add(this.chkBuzzerCancel);
            this.groupBox1.Controls.Add(this.chkShowInTaskbar);
            this.groupBox1.Controls.Add(this.chkBlink);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCountdownLength);
            this.groupBox1.Location = new System.Drawing.Point(565, 198);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(243, 232);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Settings";
            // 
            // chkQuitPrompt
            // 
            this.chkQuitPrompt.AutoSize = true;
            this.chkQuitPrompt.Location = new System.Drawing.Point(6, 89);
            this.chkQuitPrompt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkQuitPrompt.Name = "chkQuitPrompt";
            this.chkQuitPrompt.Size = new System.Drawing.Size(217, 21);
            this.chkQuitPrompt.TabIndex = 30;
            this.chkQuitPrompt.Text = "Prompt for Quit Confirmation";
            this.chkQuitPrompt.UseVisualStyleBackColor = true;
            this.chkQuitPrompt.CheckedChanged += new System.EventHandler(this.chkQuitPrompt_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.opArcade);
            this.groupBox2.Controls.Add(this.opPS2);
            this.groupBox2.Location = new System.Drawing.Point(15, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(790, 79);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Buzzer Type";
            // 
            // opPS2
            // 
            this.opPS2.AutoSize = true;
            this.opPS2.Checked = true;
            this.opPS2.Location = new System.Drawing.Point(11, 35);
            this.opPS2.Name = "opPS2";
            this.opPS2.Size = new System.Drawing.Size(113, 21);
            this.opPS2.TabIndex = 0;
            this.opPS2.TabStop = true;
            this.opPS2.Text = "PlayStation 2";
            this.opPS2.UseVisualStyleBackColor = true;
            this.opPS2.CheckedChanged += new System.EventHandler(this.opPS2_CheckedChanged);
            // 
            // opArcade
            // 
            this.opArcade.AutoSize = true;
            this.opArcade.Location = new System.Drawing.Point(135, 35);
            this.opArcade.Name = "opArcade";
            this.opArcade.Size = new System.Drawing.Size(74, 21);
            this.opArcade.TabIndex = 1;
            this.opArcade.Text = "Arcade";
            this.opArcade.UseVisualStyleBackColor = true;
            this.opArcade.CheckedChanged += new System.EventHandler(this.opArcade_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 478);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings - PACEBuzz";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgPlaySound)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkNextBuzzControl;
        private System.Windows.Forms.CheckBox chkNextBuzzShift;
        private System.Windows.Forms.ComboBox comboNextBuzzShortcutKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkResetControl;
        private System.Windows.Forms.CheckBox chkResetShift;
        private System.Windows.Forms.ComboBox cmbResetKey;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.CheckBox chkAlwaysOnTop;
        private System.Windows.Forms.TextBox txtCountdownLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkCountdownControl;
        private System.Windows.Forms.CheckBox chkCountdownShift;
        private System.Windows.Forms.ComboBox cmbCountdownShortcutKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkLightCheckControl;
        private System.Windows.Forms.CheckBox chkLightCheckShift;
        private System.Windows.Forms.ComboBox cmbLightcheckShortcutKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbSound;
        private System.Windows.Forms.ComboBox cmbBuzzerGroup;
        private System.Windows.Forms.CheckBox chkBlink;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox imgPlaySound;
        private System.Windows.Forms.CheckBox chkBuzzerCancel;
        private System.Windows.Forms.CheckBox chkShowInTaskbar;
        private System.Windows.Forms.CheckBox chkFirstPlayerCanCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkQuitPrompt;
        private System.Windows.Forms.CheckBox chkPreviousBuzzControl;
        private System.Windows.Forms.CheckBox chkPreviousBuzzShift;
        private System.Windows.Forms.ComboBox cmbPreviousBuzzShortcutKey;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton opArcade;
        private System.Windows.Forms.RadioButton opPS2;
    }
}