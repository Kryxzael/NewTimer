namespace NewTimer.Forms
{
    partial class TimerFormBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimerFormBase));
            this.tabSecondsOnly = new System.Windows.Forms.TabPage();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.SecondsOnlySecond = new NewTimer.FormParts.AutoLabel();
            this.SecondsOnlyTitle = new NewTimer.FormParts.AutoLabel();
            this.tabMinutesOnly = new System.Windows.Forms.TabPage();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.MinutesOnlyMinutes = new NewTimer.FormParts.AutoLabel();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.MinutesOnlyFraction = new NewTimer.FormParts.AutoLabel();
            this.MinutesOnlyTitle = new NewTimer.FormParts.AutoLabel();
            this.tabHoursOnly = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.HoursOnlyHour = new NewTimer.FormParts.AutoLabel();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.HoursOnlyFraction = new NewTimer.FormParts.AutoLabel();
            this.HoursOnlyTitle = new NewTimer.FormParts.AutoLabel();
            this.tabTimeOnly = new System.Windows.Forms.TabPage();
            this.TimeOnlyTime = new NewTimer.FormParts.AutoLabel();
            this.tabBarOnly = new System.Windows.Forms.TabPage();
            this.tabFull = new System.Windows.Forms.TabPage();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabAnalog = new System.Windows.Forms.TabPage();
            this.analogSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tabCircle = new System.Windows.Forms.TabPage();
            this.tabCircleSimple = new System.Windows.Forms.TabPage();
            this.tabRolling = new System.Windows.Forms.TabPage();
            this.rollDay3 = new NewTimer.FormParts.RollingDigitControl();
            this.rollDay2 = new NewTimer.FormParts.RollingDigitControl();
            this.rollDay1 = new NewTimer.FormParts.RollingDigitControl();
            this.rollHour2 = new NewTimer.FormParts.RollingDigitControl();
            this.rollMinute1 = new NewTimer.FormParts.RollingDigitControl();
            this.rollSecond1 = new NewTimer.FormParts.RollingDigitControl();
            this.rollMinute2 = new NewTimer.FormParts.RollingDigitControl();
            this.rollSecond2 = new NewTimer.FormParts.RollingDigitControl();
            this.rollHour1 = new NewTimer.FormParts.RollingDigitControl();
            this.tabTextOnly = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.TextOnlyMinute = new NewTimer.FormParts.AutoLabel();
            this.TextOnlySecond = new NewTimer.FormParts.AutoLabel();
            this.tabSecondsOnly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.tabMinutesOnly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.tabHoursOnly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.tabTimeOnly.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabAnalog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.analogSplitContainer)).BeginInit();
            this.analogSplitContainer.SuspendLayout();
            this.tabRolling.SuspendLayout();
            this.tabTextOnly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSecondsOnly
            // 
            this.tabSecondsOnly.Controls.Add(this.splitContainer7);
            this.tabSecondsOnly.Location = new System.Drawing.Point(4, 22);
            this.tabSecondsOnly.Name = "tabSecondsOnly";
            this.tabSecondsOnly.Padding = new System.Windows.Forms.Padding(3);
            this.tabSecondsOnly.Size = new System.Drawing.Size(311, 245);
            this.tabSecondsOnly.TabIndex = 4;
            this.tabSecondsOnly.Text = "Sec";
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(3, 3);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.SecondsOnlySecond);
            this.splitContainer7.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.SecondsOnlyTitle);
            this.splitContainer7.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer7.Size = new System.Drawing.Size(305, 239);
            this.splitContainer7.SplitterDistance = 174;
            this.splitContainer7.TabIndex = 0;
            // 
            // SecondsOnlySecond
            // 
            this.SecondsOnlySecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SecondsOnlySecond.Font = new System.Drawing.Font("Microsoft Sans Serif", 93.83218F);
            this.SecondsOnlySecond.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(0)))));
            this.SecondsOnlySecond.GetText = null;
            this.SecondsOnlySecond.Location = new System.Drawing.Point(0, 0);
            this.SecondsOnlySecond.Name = "SecondsOnlySecond";
            this.SecondsOnlySecond.Size = new System.Drawing.Size(305, 174);
            this.SecondsOnlySecond.TabIndex = 1;
            this.SecondsOnlySecond.Text = "0?";
            this.SecondsOnlySecond.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SecondsOnlyTitle
            // 
            this.SecondsOnlyTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SecondsOnlyTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.40094F);
            this.SecondsOnlyTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(0)))));
            this.SecondsOnlyTitle.GetText = null;
            this.SecondsOnlyTitle.Location = new System.Drawing.Point(0, 0);
            this.SecondsOnlyTitle.Name = "SecondsOnlyTitle";
            this.SecondsOnlyTitle.Size = new System.Drawing.Size(305, 61);
            this.SecondsOnlyTitle.TabIndex = 0;
            this.SecondsOnlyTitle.Text = "0?";
            this.SecondsOnlyTitle.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // tabMinutesOnly
            // 
            this.tabMinutesOnly.Controls.Add(this.splitContainer5);
            this.tabMinutesOnly.Location = new System.Drawing.Point(4, 22);
            this.tabMinutesOnly.Name = "tabMinutesOnly";
            this.tabMinutesOnly.Padding = new System.Windows.Forms.Padding(3);
            this.tabMinutesOnly.Size = new System.Drawing.Size(311, 245);
            this.tabMinutesOnly.TabIndex = 3;
            this.tabMinutesOnly.Text = "Min";
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(3, 3);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.MinutesOnlyMinutes);
            this.splitContainer5.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.splitContainer6);
            this.splitContainer5.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer5.Size = new System.Drawing.Size(305, 239);
            this.splitContainer5.SplitterDistance = 121;
            this.splitContainer5.TabIndex = 1;
            // 
            // MinutesOnlyMinutes
            // 
            this.MinutesOnlyMinutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MinutesOnlyMinutes.Font = new System.Drawing.Font("Microsoft Sans Serif", 62.80188F);
            this.MinutesOnlyMinutes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(68)))), ((int)(((byte)(136)))));
            this.MinutesOnlyMinutes.GetText = null;
            this.MinutesOnlyMinutes.Location = new System.Drawing.Point(0, 0);
            this.MinutesOnlyMinutes.Name = "MinutesOnlyMinutes";
            this.MinutesOnlyMinutes.Size = new System.Drawing.Size(305, 121);
            this.MinutesOnlyMinutes.TabIndex = 0;
            this.MinutesOnlyMinutes.Text = "0?";
            this.MinutesOnlyMinutes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.MinutesOnlyFraction);
            this.splitContainer6.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.MinutesOnlyTitle);
            this.splitContainer6.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer6.Size = new System.Drawing.Size(305, 114);
            this.splitContainer6.SplitterDistance = 66;
            this.splitContainer6.TabIndex = 0;
            // 
            // MinutesOnlyFraction
            // 
            this.MinutesOnlyFraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MinutesOnlyFraction.Font = new System.Drawing.Font("Microsoft Sans Serif", 29.98136F);
            this.MinutesOnlyFraction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.MinutesOnlyFraction.GetText = null;
            this.MinutesOnlyFraction.Location = new System.Drawing.Point(0, 0);
            this.MinutesOnlyFraction.Name = "MinutesOnlyFraction";
            this.MinutesOnlyFraction.Size = new System.Drawing.Size(305, 66);
            this.MinutesOnlyFraction.TabIndex = 1;
            this.MinutesOnlyFraction.Text = "0?";
            this.MinutesOnlyFraction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MinutesOnlyTitle
            // 
            this.MinutesOnlyTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MinutesOnlyTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.06294F);
            this.MinutesOnlyTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(34)))), ((int)(((byte)(68)))));
            this.MinutesOnlyTitle.GetText = null;
            this.MinutesOnlyTitle.Location = new System.Drawing.Point(0, 0);
            this.MinutesOnlyTitle.Name = "MinutesOnlyTitle";
            this.MinutesOnlyTitle.Size = new System.Drawing.Size(305, 44);
            this.MinutesOnlyTitle.TabIndex = 0;
            this.MinutesOnlyTitle.Text = "0?";
            this.MinutesOnlyTitle.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // tabHoursOnly
            // 
            this.tabHoursOnly.Controls.Add(this.splitContainer3);
            this.tabHoursOnly.Location = new System.Drawing.Point(4, 22);
            this.tabHoursOnly.Name = "tabHoursOnly";
            this.tabHoursOnly.Padding = new System.Windows.Forms.Padding(3);
            this.tabHoursOnly.Size = new System.Drawing.Size(311, 245);
            this.tabHoursOnly.TabIndex = 2;
            this.tabHoursOnly.Text = "Hr";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.HoursOnlyHour);
            this.splitContainer3.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer3.Size = new System.Drawing.Size(305, 239);
            this.splitContainer3.SplitterDistance = 121;
            this.splitContainer3.TabIndex = 1;
            // 
            // HoursOnlyHour
            // 
            this.HoursOnlyHour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HoursOnlyHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 62.80188F);
            this.HoursOnlyHour.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.HoursOnlyHour.GetText = null;
            this.HoursOnlyHour.Location = new System.Drawing.Point(0, 0);
            this.HoursOnlyHour.Name = "HoursOnlyHour";
            this.HoursOnlyHour.Size = new System.Drawing.Size(305, 121);
            this.HoursOnlyHour.TabIndex = 0;
            this.HoursOnlyHour.Text = "0?";
            this.HoursOnlyHour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.HoursOnlyFraction);
            this.splitContainer4.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.HoursOnlyTitle);
            this.splitContainer4.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer4.Size = new System.Drawing.Size(305, 114);
            this.splitContainer4.SplitterDistance = 67;
            this.splitContainer4.TabIndex = 0;
            // 
            // HoursOnlyFraction
            // 
            this.HoursOnlyFraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HoursOnlyFraction.Font = new System.Drawing.Font("Microsoft Sans Serif", 29.98136F);
            this.HoursOnlyFraction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.HoursOnlyFraction.GetText = null;
            this.HoursOnlyFraction.Location = new System.Drawing.Point(0, 0);
            this.HoursOnlyFraction.Name = "HoursOnlyFraction";
            this.HoursOnlyFraction.Size = new System.Drawing.Size(305, 67);
            this.HoursOnlyFraction.TabIndex = 1;
            this.HoursOnlyFraction.Text = "0?";
            this.HoursOnlyFraction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HoursOnlyTitle
            // 
            this.HoursOnlyTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HoursOnlyTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.06294F);
            this.HoursOnlyTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.HoursOnlyTitle.GetText = null;
            this.HoursOnlyTitle.Location = new System.Drawing.Point(0, 0);
            this.HoursOnlyTitle.Name = "HoursOnlyTitle";
            this.HoursOnlyTitle.Size = new System.Drawing.Size(305, 43);
            this.HoursOnlyTitle.TabIndex = 0;
            this.HoursOnlyTitle.Text = "0?";
            this.HoursOnlyTitle.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // tabTimeOnly
            // 
            this.tabTimeOnly.Controls.Add(this.TimeOnlyTime);
            this.tabTimeOnly.Location = new System.Drawing.Point(4, 22);
            this.tabTimeOnly.Name = "tabTimeOnly";
            this.tabTimeOnly.Padding = new System.Windows.Forms.Padding(3);
            this.tabTimeOnly.Size = new System.Drawing.Size(311, 245);
            this.tabTimeOnly.TabIndex = 1;
            this.tabTimeOnly.Text = "Tme";
            // 
            // TimeOnlyTime
            // 
            this.TimeOnlyTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TimeOnlyTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 132.6201F);
            this.TimeOnlyTime.GetText = null;
            this.TimeOnlyTime.Location = new System.Drawing.Point(3, 3);
            this.TimeOnlyTime.Name = "TimeOnlyTime";
            this.TimeOnlyTime.Size = new System.Drawing.Size(305, 239);
            this.TimeOnlyTime.TabIndex = 0;
            this.TimeOnlyTime.Text = "0?";
            this.TimeOnlyTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabBarOnly
            // 
            this.tabBarOnly.Location = new System.Drawing.Point(4, 22);
            this.tabBarOnly.Name = "tabBarOnly";
            this.tabBarOnly.Size = new System.Drawing.Size(311, 245);
            this.tabBarOnly.TabIndex = 6;
            this.tabBarOnly.Text = "Bar";
            this.tabBarOnly.UseVisualStyleBackColor = true;
            // 
            // tabFull
            // 
            this.tabFull.Location = new System.Drawing.Point(4, 22);
            this.tabFull.Margin = new System.Windows.Forms.Padding(0);
            this.tabFull.Name = "tabFull";
            this.tabFull.Size = new System.Drawing.Size(311, 245);
            this.tabFull.TabIndex = 0;
            this.tabFull.Text = "Digi";
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabFull);
            this.tabs.Controls.Add(this.tabAnalog);
            this.tabs.Controls.Add(this.tabCircle);
            this.tabs.Controls.Add(this.tabCircleSimple);
            this.tabs.Controls.Add(this.tabRolling);
            this.tabs.Controls.Add(this.tabBarOnly);
            this.tabs.Controls.Add(this.tabTimeOnly);
            this.tabs.Controls.Add(this.tabHoursOnly);
            this.tabs.Controls.Add(this.tabMinutesOnly);
            this.tabs.Controls.Add(this.tabSecondsOnly);
            this.tabs.Controls.Add(this.tabTextOnly);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.ItemSize = new System.Drawing.Size(25, 18);
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Multiline = true;
            this.tabs.Name = "tabs";
            this.tabs.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(319, 271);
            this.tabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabs.TabIndex = 0;
            this.tabs.Selected += new System.Windows.Forms.TabControlEventHandler(this.OnTabSelected);
            // 
            // tabAnalog
            // 
            this.tabAnalog.Controls.Add(this.analogSplitContainer);
            this.tabAnalog.Location = new System.Drawing.Point(4, 22);
            this.tabAnalog.Name = "tabAnalog";
            this.tabAnalog.Size = new System.Drawing.Size(311, 245);
            this.tabAnalog.TabIndex = 7;
            this.tabAnalog.Text = "Ana";
            this.tabAnalog.UseVisualStyleBackColor = true;
            // 
            // analogSplitContainer
            // 
            this.analogSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.analogSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.analogSplitContainer.Name = "analogSplitContainer";
            this.analogSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.analogSplitContainer.Panel2MinSize = 2;
            this.analogSplitContainer.Size = new System.Drawing.Size(311, 245);
            this.analogSplitContainer.SplitterDistance = 216;
            this.analogSplitContainer.TabIndex = 0;
            // 
            // tabCircle
            // 
            this.tabCircle.Location = new System.Drawing.Point(4, 22);
            this.tabCircle.Name = "tabCircle";
            this.tabCircle.Size = new System.Drawing.Size(311, 245);
            this.tabCircle.TabIndex = 8;
            this.tabCircle.Text = "Cir";
            this.tabCircle.UseVisualStyleBackColor = true;
            // 
            // tabCircleSimple
            // 
            this.tabCircleSimple.Location = new System.Drawing.Point(4, 22);
            this.tabCircleSimple.Name = "tabCircleSimple";
            this.tabCircleSimple.Size = new System.Drawing.Size(311, 245);
            this.tabCircleSimple.TabIndex = 9;
            this.tabCircleSimple.Text = "Cdn";
            this.tabCircleSimple.UseVisualStyleBackColor = true;
            // 
            // tabRolling
            // 
            this.tabRolling.Controls.Add(this.rollDay3);
            this.tabRolling.Controls.Add(this.rollDay2);
            this.tabRolling.Controls.Add(this.rollDay1);
            this.tabRolling.Controls.Add(this.rollHour2);
            this.tabRolling.Controls.Add(this.rollMinute1);
            this.tabRolling.Controls.Add(this.rollSecond1);
            this.tabRolling.Controls.Add(this.rollMinute2);
            this.tabRolling.Controls.Add(this.rollSecond2);
            this.tabRolling.Controls.Add(this.rollHour1);
            this.tabRolling.Location = new System.Drawing.Point(4, 22);
            this.tabRolling.Name = "tabRolling";
            this.tabRolling.Size = new System.Drawing.Size(311, 245);
            this.tabRolling.TabIndex = 11;
            this.tabRolling.Text = "Roll";
            this.tabRolling.UseVisualStyleBackColor = true;
            // 
            // rollDay3
            // 
            this.rollDay3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rollDay3.Digit = NewTimer.FormParts.RollingDigitControl.DateTimeDigit.DayUnits;
            this.rollDay3.GetValue = null;
            this.rollDay3.Location = new System.Drawing.Point(79, 210);
            this.rollDay3.Name = "rollDay3";
            this.rollDay3.Size = new System.Drawing.Size(32, 32);
            this.rollDay3.TabIndex = 8;
            this.rollDay3.TrackSecondaryTimer = false;
            // 
            // rollDay2
            // 
            this.rollDay2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rollDay2.Digit = NewTimer.FormParts.RollingDigitControl.DateTimeDigit.DayTens;
            this.rollDay2.GetValue = null;
            this.rollDay2.Location = new System.Drawing.Point(41, 210);
            this.rollDay2.Name = "rollDay2";
            this.rollDay2.Size = new System.Drawing.Size(32, 32);
            this.rollDay2.TabIndex = 7;
            this.rollDay2.TrackSecondaryTimer = false;
            // 
            // rollDay1
            // 
            this.rollDay1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rollDay1.Digit = NewTimer.FormParts.RollingDigitControl.DateTimeDigit.DayHundreds;
            this.rollDay1.GetValue = null;
            this.rollDay1.Location = new System.Drawing.Point(3, 210);
            this.rollDay1.Name = "rollDay1";
            this.rollDay1.Size = new System.Drawing.Size(32, 32);
            this.rollDay1.TabIndex = 6;
            this.rollDay1.TrackSecondaryTimer = false;
            // 
            // rollHour2
            // 
            this.rollHour2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rollHour2.Digit = NewTimer.FormParts.RollingDigitControl.DateTimeDigit.HourUnits;
            this.rollHour2.GetValue = null;
            this.rollHour2.Location = new System.Drawing.Point(81, 3);
            this.rollHour2.Name = "rollHour2";
            this.rollHour2.Size = new System.Drawing.Size(72, 72);
            this.rollHour2.TabIndex = 5;
            this.rollHour2.TrackSecondaryTimer = false;
            // 
            // rollMinute1
            // 
            this.rollMinute1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rollMinute1.Digit = NewTimer.FormParts.RollingDigitControl.DateTimeDigit.MinuteTens;
            this.rollMinute1.GetValue = null;
            this.rollMinute1.Location = new System.Drawing.Point(81, 86);
            this.rollMinute1.Name = "rollMinute1";
            this.rollMinute1.Size = new System.Drawing.Size(72, 72);
            this.rollMinute1.TabIndex = 4;
            this.rollMinute1.TrackSecondaryTimer = false;
            // 
            // rollSecond1
            // 
            this.rollSecond1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rollSecond1.Digit = NewTimer.FormParts.RollingDigitControl.DateTimeDigit.SecondTens;
            this.rollSecond1.GetValue = null;
            this.rollSecond1.Location = new System.Drawing.Point(159, 170);
            this.rollSecond1.Name = "rollSecond1";
            this.rollSecond1.Size = new System.Drawing.Size(72, 72);
            this.rollSecond1.TabIndex = 3;
            this.rollSecond1.TrackSecondaryTimer = false;
            // 
            // rollMinute2
            // 
            this.rollMinute2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rollMinute2.Digit = NewTimer.FormParts.RollingDigitControl.DateTimeDigit.MinutesUnits;
            this.rollMinute2.GetValue = null;
            this.rollMinute2.Location = new System.Drawing.Point(159, 86);
            this.rollMinute2.Name = "rollMinute2";
            this.rollMinute2.Size = new System.Drawing.Size(72, 72);
            this.rollMinute2.TabIndex = 2;
            this.rollMinute2.TrackSecondaryTimer = false;
            // 
            // rollSecond2
            // 
            this.rollSecond2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rollSecond2.Digit = NewTimer.FormParts.RollingDigitControl.DateTimeDigit.SecondsUnits;
            this.rollSecond2.GetValue = null;
            this.rollSecond2.Location = new System.Drawing.Point(236, 170);
            this.rollSecond2.Name = "rollSecond2";
            this.rollSecond2.Size = new System.Drawing.Size(72, 72);
            this.rollSecond2.TabIndex = 1;
            this.rollSecond2.TrackSecondaryTimer = false;
            // 
            // rollHour1
            // 
            this.rollHour1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rollHour1.Digit = NewTimer.FormParts.RollingDigitControl.DateTimeDigit.HourTens;
            this.rollHour1.GetValue = null;
            this.rollHour1.Location = new System.Drawing.Point(3, 3);
            this.rollHour1.Name = "rollHour1";
            this.rollHour1.Size = new System.Drawing.Size(72, 72);
            this.rollHour1.TabIndex = 0;
            this.rollHour1.TrackSecondaryTimer = false;
            // 
            // tabTextOnly
            // 
            this.tabTextOnly.Controls.Add(this.splitContainer1);
            this.tabTextOnly.Location = new System.Drawing.Point(4, 22);
            this.tabTextOnly.Name = "tabTextOnly";
            this.tabTextOnly.Padding = new System.Windows.Forms.Padding(3);
            this.tabTextOnly.Size = new System.Drawing.Size(311, 245);
            this.tabTextOnly.TabIndex = 10;
            this.tabTextOnly.Text = "Txt";
            this.tabTextOnly.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TextOnlyMinute);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.TextOnlySecond);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(305, 239);
            this.splitContainer1.SplitterDistance = 174;
            this.splitContainer1.TabIndex = 1;
            // 
            // TextOnlyMinute
            // 
            this.TextOnlyMinute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextOnlyMinute.Font = new System.Drawing.Font("Microsoft Sans Serif", 93.83218F);
            this.TextOnlyMinute.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(143)))), ((int)(((byte)(43)))), ((int)(((byte)(189)))));
            this.TextOnlyMinute.GetText = null;
            this.TextOnlyMinute.Location = new System.Drawing.Point(0, 0);
            this.TextOnlyMinute.Name = "TextOnlyMinute";
            this.TextOnlyMinute.Size = new System.Drawing.Size(305, 174);
            this.TextOnlyMinute.TabIndex = 1;
            this.TextOnlyMinute.Text = "0?";
            this.TextOnlyMinute.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextOnlySecond
            // 
            this.TextOnlySecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextOnlySecond.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.40094F);
            this.TextOnlySecond.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(33)))), ((int)(((byte)(79)))));
            this.TextOnlySecond.GetText = null;
            this.TextOnlySecond.Location = new System.Drawing.Point(0, 0);
            this.TextOnlySecond.Name = "TextOnlySecond";
            this.TextOnlySecond.Size = new System.Drawing.Size(305, 61);
            this.TextOnlySecond.TabIndex = 0;
            this.TextOnlySecond.Text = "0?";
            this.TextOnlySecond.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // TimerFormBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 271);
            this.Controls.Add(this.tabs);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TimerFormBase";
            this.Text = "s";
            this.tabSecondsOnly.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.tabMinutesOnly.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.tabHoursOnly.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.tabTimeOnly.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabAnalog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.analogSplitContainer)).EndInit();
            this.analogSplitContainer.ResumeLayout(false);
            this.tabRolling.ResumeLayout(false);
            this.tabTextOnly.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabSecondsOnly;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private FormParts.AutoLabel SecondsOnlySecond;
        private FormParts.AutoLabel SecondsOnlyTitle;
        private System.Windows.Forms.TabPage tabMinutesOnly;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private FormParts.AutoLabel MinutesOnlyMinutes;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private FormParts.AutoLabel MinutesOnlyFraction;
        private FormParts.AutoLabel MinutesOnlyTitle;
        private System.Windows.Forms.TabPage tabHoursOnly;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private FormParts.AutoLabel HoursOnlyHour;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private FormParts.AutoLabel HoursOnlyFraction;
        private FormParts.AutoLabel HoursOnlyTitle;
        private System.Windows.Forms.TabPage tabTimeOnly;
        private System.Windows.Forms.TabPage tabBarOnly;
        private System.Windows.Forms.TabPage tabFull;
        private System.Windows.Forms.TabControl tabs;
        private FormParts.AutoLabel TimeOnlyTime;
        private System.Windows.Forms.TabPage tabAnalog;
        private System.Windows.Forms.TabPage tabCircle;
        private System.Windows.Forms.TabPage tabCircleSimple;
        private System.Windows.Forms.SplitContainer analogSplitContainer;
        private System.Windows.Forms.TabPage tabTextOnly;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private FormParts.AutoLabel TextOnlyMinute;
        private FormParts.AutoLabel TextOnlySecond;
        private System.Windows.Forms.TabPage tabRolling;
        private FormParts.RollingDigitControl rollDay3;
        private FormParts.RollingDigitControl rollDay2;
        private FormParts.RollingDigitControl rollDay1;
        private FormParts.RollingDigitControl rollHour2;
        private FormParts.RollingDigitControl rollMinute1;
        private FormParts.RollingDigitControl rollSecond1;
        private FormParts.RollingDigitControl rollMinute2;
        private FormParts.RollingDigitControl rollSecond2;
        private FormParts.RollingDigitControl rollHour1;
    }
}