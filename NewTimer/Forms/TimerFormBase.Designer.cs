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
            this.tabMinutesOnly = new System.Windows.Forms.TabPage();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.tabHoursOnly = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.tabTimeOnly = new System.Windows.Forms.TabPage();
            this.tabBarOnly = new System.Windows.Forms.TabPage();
            this.tabFull = new System.Windows.Forms.TabPage();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabAnalog = new System.Windows.Forms.TabPage();
            this.TimeOnlyTime = new NewTimer.FormParts.AutoLabel();
            this.HoursOnlyHour = new NewTimer.FormParts.AutoLabel();
            this.HoursOnlyFraction = new NewTimer.FormParts.AutoLabel();
            this.HoursOnlyTitle = new NewTimer.FormParts.AutoLabel();
            this.MinutesOnlyMinutes = new NewTimer.FormParts.AutoLabel();
            this.MinutesOnlyFraction = new NewTimer.FormParts.AutoLabel();
            this.MinutesOnlyTitle = new NewTimer.FormParts.AutoLabel();
            this.SecondsOnlySecond = new NewTimer.FormParts.AutoLabel();
            this.SecondsOnlyTitle = new NewTimer.FormParts.AutoLabel();
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
            this.SuspendLayout();
            // 
            // tabSecondsOnly
            // 
            this.tabSecondsOnly.Controls.Add(this.splitContainer7);
            this.tabSecondsOnly.Location = new System.Drawing.Point(4, 22);
            this.tabSecondsOnly.Name = "tabSecondsOnly";
            this.tabSecondsOnly.Padding = new System.Windows.Forms.Padding(3);
            this.tabSecondsOnly.Size = new System.Drawing.Size(362, 245);
            this.tabSecondsOnly.TabIndex = 4;
            this.tabSecondsOnly.Text = "Seconds";
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
            this.splitContainer7.Size = new System.Drawing.Size(356, 239);
            this.splitContainer7.SplitterDistance = 175;
            this.splitContainer7.TabIndex = 0;
            // 
            // tabMinutesOnly
            // 
            this.tabMinutesOnly.Controls.Add(this.splitContainer5);
            this.tabMinutesOnly.Location = new System.Drawing.Point(4, 22);
            this.tabMinutesOnly.Name = "tabMinutesOnly";
            this.tabMinutesOnly.Padding = new System.Windows.Forms.Padding(3);
            this.tabMinutesOnly.Size = new System.Drawing.Size(362, 245);
            this.tabMinutesOnly.TabIndex = 3;
            this.tabMinutesOnly.Text = "Minutes";
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
            this.splitContainer5.Size = new System.Drawing.Size(356, 239);
            this.splitContainer5.SplitterDistance = 122;
            this.splitContainer5.TabIndex = 1;
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
            this.splitContainer6.Size = new System.Drawing.Size(356, 113);
            this.splitContainer6.SplitterDistance = 67;
            this.splitContainer6.TabIndex = 0;
            // 
            // tabHoursOnly
            // 
            this.tabHoursOnly.Controls.Add(this.splitContainer3);
            this.tabHoursOnly.Location = new System.Drawing.Point(4, 22);
            this.tabHoursOnly.Name = "tabHoursOnly";
            this.tabHoursOnly.Padding = new System.Windows.Forms.Padding(3);
            this.tabHoursOnly.Size = new System.Drawing.Size(362, 245);
            this.tabHoursOnly.TabIndex = 2;
            this.tabHoursOnly.Text = "Hours";
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
            this.splitContainer3.Size = new System.Drawing.Size(356, 239);
            this.splitContainer3.SplitterDistance = 122;
            this.splitContainer3.TabIndex = 1;
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
            this.splitContainer4.Size = new System.Drawing.Size(356, 113);
            this.splitContainer4.SplitterDistance = 67;
            this.splitContainer4.TabIndex = 0;
            // 
            // tabTimeOnly
            // 
            this.tabTimeOnly.Controls.Add(this.TimeOnlyTime);
            this.tabTimeOnly.Location = new System.Drawing.Point(4, 22);
            this.tabTimeOnly.Name = "tabTimeOnly";
            this.tabTimeOnly.Padding = new System.Windows.Forms.Padding(3);
            this.tabTimeOnly.Size = new System.Drawing.Size(362, 245);
            this.tabTimeOnly.TabIndex = 1;
            this.tabTimeOnly.Text = "Time";
            // 
            // tabBarOnly
            // 
            this.tabBarOnly.Location = new System.Drawing.Point(4, 22);
            this.tabBarOnly.Name = "tabBarOnly";
            this.tabBarOnly.Size = new System.Drawing.Size(362, 245);
            this.tabBarOnly.TabIndex = 6;
            this.tabBarOnly.Text = "Bar";
            this.tabBarOnly.UseVisualStyleBackColor = true;
            // 
            // tabFull
            // 
            this.tabFull.Location = new System.Drawing.Point(4, 22);
            this.tabFull.Margin = new System.Windows.Forms.Padding(0);
            this.tabFull.Name = "tabFull";
            this.tabFull.Size = new System.Drawing.Size(362, 245);
            this.tabFull.TabIndex = 0;
            this.tabFull.Text = "Digi";
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabFull);
            this.tabs.Controls.Add(this.tabAnalog);
            this.tabs.Controls.Add(this.tabBarOnly);
            this.tabs.Controls.Add(this.tabTimeOnly);
            this.tabs.Controls.Add(this.tabHoursOnly);
            this.tabs.Controls.Add(this.tabMinutesOnly);
            this.tabs.Controls.Add(this.tabSecondsOnly);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Multiline = true;
            this.tabs.Name = "tabs";
            this.tabs.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(370, 271);
            this.tabs.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabs.TabIndex = 0;
            this.tabs.Selected += new System.Windows.Forms.TabControlEventHandler(this.OnTabSelected);
            // 
            // tabAnalog
            // 
            this.tabAnalog.Location = new System.Drawing.Point(4, 22);
            this.tabAnalog.Name = "tabAnalog";
            this.tabAnalog.Size = new System.Drawing.Size(362, 245);
            this.tabAnalog.TabIndex = 7;
            this.tabAnalog.Text = "Ana";
            this.tabAnalog.UseVisualStyleBackColor = true;
            // 
            // TimeOnlyTime
            // 
            this.TimeOnlyTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TimeOnlyTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 132.6201F);
            this.TimeOnlyTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(0)))));
            this.TimeOnlyTime.GetText = null;
            this.TimeOnlyTime.Location = new System.Drawing.Point(3, 3);
            this.TimeOnlyTime.Name = "TimeOnlyTime";
            this.TimeOnlyTime.Size = new System.Drawing.Size(356, 239);
            this.TimeOnlyTime.TabIndex = 0;
            this.TimeOnlyTime.Text = "0?";
            this.TimeOnlyTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HoursOnlyHour
            // 
            this.HoursOnlyHour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HoursOnlyHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 62.80188F);
            this.HoursOnlyHour.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.HoursOnlyHour.GetText = null;
            this.HoursOnlyHour.Location = new System.Drawing.Point(0, 0);
            this.HoursOnlyHour.Name = "HoursOnlyHour";
            this.HoursOnlyHour.Size = new System.Drawing.Size(356, 122);
            this.HoursOnlyHour.TabIndex = 0;
            this.HoursOnlyHour.Text = "0?";
            this.HoursOnlyHour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HoursOnlyFraction
            // 
            this.HoursOnlyFraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HoursOnlyFraction.Font = new System.Drawing.Font("Microsoft Sans Serif", 29.98136F);
            this.HoursOnlyFraction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.HoursOnlyFraction.GetText = null;
            this.HoursOnlyFraction.Location = new System.Drawing.Point(0, 0);
            this.HoursOnlyFraction.Name = "HoursOnlyFraction";
            this.HoursOnlyFraction.Size = new System.Drawing.Size(356, 67);
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
            this.HoursOnlyTitle.Size = new System.Drawing.Size(356, 42);
            this.HoursOnlyTitle.TabIndex = 0;
            this.HoursOnlyTitle.Text = "0?";
            this.HoursOnlyTitle.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // MinutesOnlyMinutes
            // 
            this.MinutesOnlyMinutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MinutesOnlyMinutes.Font = new System.Drawing.Font("Microsoft Sans Serif", 62.80188F);
            this.MinutesOnlyMinutes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(68)))), ((int)(((byte)(136)))));
            this.MinutesOnlyMinutes.GetText = null;
            this.MinutesOnlyMinutes.Location = new System.Drawing.Point(0, 0);
            this.MinutesOnlyMinutes.Name = "MinutesOnlyMinutes";
            this.MinutesOnlyMinutes.Size = new System.Drawing.Size(356, 122);
            this.MinutesOnlyMinutes.TabIndex = 0;
            this.MinutesOnlyMinutes.Text = "0?";
            this.MinutesOnlyMinutes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MinutesOnlyFraction
            // 
            this.MinutesOnlyFraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MinutesOnlyFraction.Font = new System.Drawing.Font("Microsoft Sans Serif", 29.98136F);
            this.MinutesOnlyFraction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.MinutesOnlyFraction.GetText = null;
            this.MinutesOnlyFraction.Location = new System.Drawing.Point(0, 0);
            this.MinutesOnlyFraction.Name = "MinutesOnlyFraction";
            this.MinutesOnlyFraction.Size = new System.Drawing.Size(356, 67);
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
            this.MinutesOnlyTitle.Size = new System.Drawing.Size(356, 42);
            this.MinutesOnlyTitle.TabIndex = 0;
            this.MinutesOnlyTitle.Text = "0?";
            this.MinutesOnlyTitle.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // SecondsOnlySecond
            // 
            this.SecondsOnlySecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SecondsOnlySecond.Font = new System.Drawing.Font("Microsoft Sans Serif", 94.42892F);
            this.SecondsOnlySecond.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(0)))));
            this.SecondsOnlySecond.GetText = null;
            this.SecondsOnlySecond.Location = new System.Drawing.Point(0, 0);
            this.SecondsOnlySecond.Name = "SecondsOnlySecond";
            this.SecondsOnlySecond.Size = new System.Drawing.Size(356, 175);
            this.SecondsOnlySecond.TabIndex = 1;
            this.SecondsOnlySecond.Text = "0?";
            this.SecondsOnlySecond.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SecondsOnlyTitle
            // 
            this.SecondsOnlyTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SecondsOnlyTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8042F);
            this.SecondsOnlyTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(0)))));
            this.SecondsOnlyTitle.GetText = null;
            this.SecondsOnlyTitle.Location = new System.Drawing.Point(0, 0);
            this.SecondsOnlyTitle.Name = "SecondsOnlyTitle";
            this.SecondsOnlyTitle.Size = new System.Drawing.Size(356, 60);
            this.SecondsOnlyTitle.TabIndex = 0;
            this.SecondsOnlyTitle.Text = "0?";
            this.SecondsOnlyTitle.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // TimerFormBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 271);
            this.Controls.Add(this.tabs);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TimerFormBase";
            this.Text = "Countdown";
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
    }
}