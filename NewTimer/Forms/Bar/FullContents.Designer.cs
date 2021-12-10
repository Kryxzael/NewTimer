namespace NewTimer.Forms.Bar
{
    partial class FullContents
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
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.timerBar1 = new NewTimer.FormParts.TimerBar();
            this.FullD = new NewTimer.FormParts.LabelGrayedLeadingZeros();
            this.overwatchBar2 = new NewTimer.FormParts.TimerBar();
            this.FullS = new NewTimer.FormParts.LabelGrayedLeadingZeros();
            this.FullH = new NewTimer.FormParts.LabelGrayedLeadingZeros();
            this.FullFracM = new NewTimer.FormParts.LabelGrayedLeadingZeros();
            this.FullFracH = new NewTimer.FormParts.LabelGrayedLeadingZeros();
            this.FullTotalM = new NewTimer.FormParts.LabelGrayedLeadingZeros();
            this.FullTotalH = new NewTimer.FormParts.LabelGrayedLeadingZeros();
            this.FullM = new NewTimer.FormParts.LabelGrayedLeadingZeros();
            this.FullTotalS = new NewTimer.FormParts.LabelGrayedLeadingZeros();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.label11.Location = new System.Drawing.Point(269, 57);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Minutes";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.label10.Location = new System.Drawing.Point(-1, 56);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Hours";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.label9.Location = new System.Drawing.Point(131, 27);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Seconds";
            // 
            // timerBar1
            // 
            this.timerBar1.BarMargin = 0;
            this.timerBar1.FillColor = System.Drawing.Color.Empty;
            this.timerBar1.Interval = 1;
            this.timerBar1.Location = new System.Drawing.Point(0, 221);
            this.timerBar1.MaxValue = 1F;
            this.timerBar1.Name = "timerBar1";
            this.timerBar1.OverflowColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.timerBar1.OverrideValueCode = null;
            this.timerBar1.Size = new System.Drawing.Size(311, 24);
            this.timerBar1.TabIndex = 25;
            this.timerBar1.TabStop = false;
            this.timerBar1.TrackSecondaryTimer = true;
            this.timerBar1.Value = 0F;
            // 
            // FullD
            // 
            this.FullD.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.FullD.HighlightColor = System.Drawing.Color.Empty;
            this.FullD.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FullD.LeadingZerosColor = System.Drawing.Color.Gray;
            this.FullD.Location = new System.Drawing.Point(8, 87);
            this.FullD.Name = "FullD";
            this.FullD.Progress = 0F;
            this.FullD.RenderLeadingZeros = false;
            this.FullD.Size = new System.Drawing.Size(29, 35);
            this.FullD.TabIndex = 24;
            this.FullD.Text = "00";
            this.FullD.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // overwatchBar2
            // 
            this.overwatchBar2.BarMargin = 0;
            this.overwatchBar2.FillColor = System.Drawing.Color.Empty;
            this.overwatchBar2.Interval = 1;
            this.overwatchBar2.Location = new System.Drawing.Point(0, 170);
            this.overwatchBar2.MaxValue = 1F;
            this.overwatchBar2.Name = "overwatchBar2";
            this.overwatchBar2.OverflowColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.overwatchBar2.OverrideValueCode = null;
            this.overwatchBar2.Size = new System.Drawing.Size(311, 75);
            this.overwatchBar2.TabIndex = 12;
            this.overwatchBar2.TabStop = false;
            this.overwatchBar2.TrackSecondaryTimer = false;
            this.overwatchBar2.Value = 0F;
            // 
            // FullS
            // 
            this.FullS.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.FullS.HighlightColor = System.Drawing.Color.Empty;
            this.FullS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.FullS.LeadingZerosColor = System.Drawing.Color.Gray;
            this.FullS.Location = new System.Drawing.Point(209, 116);
            this.FullS.Name = "FullS";
            this.FullS.Progress = 0F;
            this.FullS.RenderLeadingZeros = false;
            this.FullS.Size = new System.Drawing.Size(100, 54);
            this.FullS.TabIndex = 23;
            this.FullS.Text = "00";
            this.FullS.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // FullH
            // 
            this.FullH.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.FullH.HighlightColor = System.Drawing.Color.Empty;
            this.FullH.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FullH.LeadingZerosColor = System.Drawing.Color.Gray;
            this.FullH.Location = new System.Drawing.Point(41, 83);
            this.FullH.Name = "FullH";
            this.FullH.Progress = 0F;
            this.FullH.RenderLeadingZeros = false;
            this.FullH.Size = new System.Drawing.Size(58, 54);
            this.FullH.TabIndex = 22;
            this.FullH.Text = "00";
            this.FullH.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FullFracM
            // 
            this.FullFracM.AutoSize = true;
            this.FullFracM.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullFracM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.FullFracM.HighlightColor = System.Drawing.Color.Empty;
            this.FullFracM.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FullFracM.LeadingZerosColor = System.Drawing.Color.Gray;
            this.FullFracM.Location = new System.Drawing.Point(284, 37);
            this.FullFracM.Name = "FullFracM";
            this.FullFracM.Progress = 0F;
            this.FullFracM.RenderLeadingZeros = false;
            this.FullFracM.Size = new System.Drawing.Size(34, 20);
            this.FullFracM.TabIndex = 18;
            this.FullFracM.Text = ".00";
            this.FullFracM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FullFracH
            // 
            this.FullFracH.AutoSize = true;
            this.FullFracH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullFracH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.FullFracH.HighlightColor = System.Drawing.Color.Empty;
            this.FullFracH.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FullFracH.LeadingZerosColor = System.Drawing.Color.Gray;
            this.FullFracH.Location = new System.Drawing.Point(-2, 36);
            this.FullFracH.Name = "FullFracH";
            this.FullFracH.Progress = 0F;
            this.FullFracH.RenderLeadingZeros = false;
            this.FullFracH.Size = new System.Drawing.Size(44, 20);
            this.FullFracH.TabIndex = 17;
            this.FullFracH.Text = ".000";
            this.FullFracH.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FullTotalM
            // 
            this.FullTotalM.AutoSize = true;
            this.FullTotalM.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullTotalM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.FullTotalM.HighlightColor = System.Drawing.Color.Empty;
            this.FullTotalM.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FullTotalM.LeadingZerosColor = System.Drawing.Color.Gray;
            this.FullTotalM.Location = new System.Drawing.Point(249, 0);
            this.FullTotalM.Name = "FullTotalM";
            this.FullTotalM.Progress = 0F;
            this.FullTotalM.RenderLeadingZeros = false;
            this.FullTotalM.Size = new System.Drawing.Size(77, 39);
            this.FullTotalM.TabIndex = 15;
            this.FullTotalM.Text = "000";
            this.FullTotalM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FullTotalH
            // 
            this.FullTotalH.AutoSize = true;
            this.FullTotalH.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullTotalH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.FullTotalH.HighlightColor = System.Drawing.Color.Empty;
            this.FullTotalH.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FullTotalH.LeadingZerosColor = System.Drawing.Color.Gray;
            this.FullTotalH.Location = new System.Drawing.Point(-5, 0);
            this.FullTotalH.Name = "FullTotalH";
            this.FullTotalH.Progress = 0F;
            this.FullTotalH.RenderLeadingZeros = false;
            this.FullTotalH.Size = new System.Drawing.Size(57, 39);
            this.FullTotalH.TabIndex = 14;
            this.FullTotalH.Text = "00";
            this.FullTotalH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FullM
            // 
            this.FullM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FullM.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.FullM.HighlightColor = System.Drawing.Color.Empty;
            this.FullM.LeadingZerosColor = System.Drawing.Color.Gray;
            this.FullM.Location = new System.Drawing.Point(99, 70);
            this.FullM.Name = "FullM";
            this.FullM.Progress = 0F;
            this.FullM.RenderLeadingZeros = false;
            this.FullM.Size = new System.Drawing.Size(208, 100);
            this.FullM.TabIndex = 13;
            this.FullM.Text = "00";
            this.FullM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FullTotalS
            // 
            this.FullTotalS.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullTotalS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.FullTotalS.HighlightColor = System.Drawing.Color.Empty;
            this.FullTotalS.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FullTotalS.LeadingZerosColor = System.Drawing.Color.Gray;
            this.FullTotalS.Location = new System.Drawing.Point(102, 0);
            this.FullTotalS.Name = "FullTotalS";
            this.FullTotalS.Progress = 0F;
            this.FullTotalS.RenderLeadingZeros = false;
            this.FullTotalS.Size = new System.Drawing.Size(136, 27);
            this.FullTotalS.TabIndex = 16;
            this.FullTotalS.Text = "0000000";
            this.FullTotalS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FullContents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.timerBar1);
            this.Controls.Add(this.FullD);
            this.Controls.Add(this.overwatchBar2);
            this.Controls.Add(this.FullS);
            this.Controls.Add(this.FullH);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.FullFracM);
            this.Controls.Add(this.FullFracH);
            this.Controls.Add(this.FullTotalM);
            this.Controls.Add(this.FullTotalH);
            this.Controls.Add(this.FullM);
            this.Controls.Add(this.FullTotalS);
            this.Name = "FullContents";
            this.Size = new System.Drawing.Size(312, 251);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private FormParts.LabelGrayedLeadingZeros FullFracM;
        private FormParts.LabelGrayedLeadingZeros FullFracH;
        private FormParts.LabelGrayedLeadingZeros FullTotalS;
        private FormParts.LabelGrayedLeadingZeros FullTotalM;
        private FormParts.LabelGrayedLeadingZeros FullTotalH;
        private FormParts.LabelGrayedLeadingZeros FullM;
        private NewTimer.FormParts.TimerBar overwatchBar2;
        private FormParts.LabelGrayedLeadingZeros FullH;
        private FormParts.LabelGrayedLeadingZeros FullS;
        private FormParts.LabelGrayedLeadingZeros FullD;
        private FormParts.TimerBar timerBar1;
    }
}
