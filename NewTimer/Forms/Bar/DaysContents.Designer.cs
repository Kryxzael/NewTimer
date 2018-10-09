namespace NewTimer.Forms.Bar
{
    partial class DaysContents
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.autoLabel1 = new NewTimer.FormParts.AutoLabel();
            this.autoLabel2 = new NewTimer.FormParts.AutoLabel();
            this.overwatchBar1 = new NewTimer.FormParts.TimerBar();
            this.overwatchBar3 = new NewTimer.FormParts.TimerBar();
            this.DaysOnlyFraction = new NewTimer.FormParts.AutoLabel();
            this.DaysOnlyDay = new NewTimer.FormParts.AutoLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.overwatchBar1);
            this.splitContainer1.Size = new System.Drawing.Size(388, 268);
            this.splitContainer1.SplitterDistance = 188;
            this.splitContainer1.TabIndex = 6;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.autoLabel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.autoLabel2);
            this.splitContainer2.Size = new System.Drawing.Size(388, 188);
            this.splitContainer2.SplitterDistance = 119;
            this.splitContainer2.TabIndex = 1;
            // 
            // autoLabel1
            // 
            this.autoLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 61.01167F);
            this.autoLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.autoLabel1.GetText = null;
            this.autoLabel1.Location = new System.Drawing.Point(0, 0);
            this.autoLabel1.Name = "autoLabel1";
            this.autoLabel1.Size = new System.Drawing.Size(388, 119);
            this.autoLabel1.TabIndex = 0;
            this.autoLabel1.Text = "0?";
            this.autoLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoLabel2
            // 
            this.autoLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.78789F);
            this.autoLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.autoLabel2.GetText = null;
            this.autoLabel2.Location = new System.Drawing.Point(0, 0);
            this.autoLabel2.Name = "autoLabel2";
            this.autoLabel2.Size = new System.Drawing.Size(388, 65);
            this.autoLabel2.TabIndex = 2;
            this.autoLabel2.Text = "0?";
            this.autoLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // overwatchBar1
            // 
            this.overwatchBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overwatchBar1.FillColor = System.Drawing.Color.Empty;
            this.overwatchBar1.Interval = 1;
            this.overwatchBar1.Location = new System.Drawing.Point(0, 0);
            this.overwatchBar1.Margin = new System.Windows.Forms.Padding(0);
            this.overwatchBar1.MaxValue = 1F;
            this.overwatchBar1.Name = "overwatchBar1";
            this.overwatchBar1.OverflowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(211)))), ((int)(((byte)(255)))));
            this.overwatchBar1.OverrideValueCode = null;
            this.overwatchBar1.Size = new System.Drawing.Size(388, 76);
            this.overwatchBar1.TabIndex = 1;
            this.overwatchBar1.TabStop = false;
            this.overwatchBar1.Value = 0F;
            // 
            // overwatchBar3
            // 
            this.overwatchBar3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overwatchBar3.FillColor = System.Drawing.Color.Empty;
            this.overwatchBar3.Interval = 1;
            this.overwatchBar3.Location = new System.Drawing.Point(0, 0);
            this.overwatchBar3.Margin = new System.Windows.Forms.Padding(0);
            this.overwatchBar3.MaxValue = 1F;
            this.overwatchBar3.Name = "overwatchBar3";
            this.overwatchBar3.OverflowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(122)))));
            this.overwatchBar3.OverrideValueCode = null;
            this.overwatchBar3.Size = new System.Drawing.Size(388, 268);
            this.overwatchBar3.TabIndex = 4;
            this.overwatchBar3.TabStop = false;
            this.overwatchBar3.Value = 0F;
            // 
            // DaysOnlyFraction
            // 
            this.DaysOnlyFraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DaysOnlyFraction.Font = new System.Drawing.Font("Microsoft Sans Serif", 203.035F);
            this.DaysOnlyFraction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.DaysOnlyFraction.GetText = null;
            this.DaysOnlyFraction.Location = new System.Drawing.Point(0, 0);
            this.DaysOnlyFraction.Name = "DaysOnlyFraction";
            this.DaysOnlyFraction.Size = new System.Drawing.Size(388, 268);
            this.DaysOnlyFraction.TabIndex = 5;
            this.DaysOnlyFraction.Text = "0?";
            this.DaysOnlyFraction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DaysOnlyDay
            // 
            this.DaysOnlyDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DaysOnlyDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 203.035F);
            this.DaysOnlyDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.DaysOnlyDay.GetText = null;
            this.DaysOnlyDay.Location = new System.Drawing.Point(0, 0);
            this.DaysOnlyDay.Name = "DaysOnlyDay";
            this.DaysOnlyDay.Size = new System.Drawing.Size(388, 268);
            this.DaysOnlyDay.TabIndex = 3;
            this.DaysOnlyDay.Text = "0?";
            this.DaysOnlyDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DaysContents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.overwatchBar3);
            this.Controls.Add(this.DaysOnlyFraction);
            this.Controls.Add(this.DaysOnlyDay);
            this.Name = "DaysContents";
            this.Size = new System.Drawing.Size(388, 268);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FormParts.AutoLabel DaysOnlyDay;
        private FormParts.AutoLabel DaysOnlyFraction;
        private NewTimer.FormParts.TimerBar overwatchBar3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private FormParts.AutoLabel autoLabel1;
        private FormParts.AutoLabel autoLabel2;
        private NewTimer.FormParts.TimerBar overwatchBar1;
    }
}
