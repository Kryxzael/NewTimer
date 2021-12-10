namespace NewTimer.Forms.Bar
{
    partial class BarContents
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
            this.timerBar1 = new NewTimer.FormParts.TimerBar();
            this.overwatchBar1 = new NewTimer.FormParts.TimerBar();
            this.SuspendLayout();
            // 
            // timerBar1
            // 
            this.timerBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.timerBar1.BarMargin = 0;
            this.timerBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.timerBar1.DrawHatched = true;
            this.timerBar1.DrawHatchedOverflow = true;
            this.timerBar1.FillColor = System.Drawing.Color.Empty;
            this.timerBar1.Interval = 1;
            this.timerBar1.Location = new System.Drawing.Point(0, 116);
            this.timerBar1.MaxValue = 1F;
            this.timerBar1.Name = "timerBar1";
            this.timerBar1.OverflowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(211)))), ((int)(((byte)(255)))));
            this.timerBar1.OverrideValueCode = null;
            this.timerBar1.Size = new System.Drawing.Size(150, 34);
            this.timerBar1.TabIndex = 2;
            this.timerBar1.TabStop = false;
            this.timerBar1.TrackSecondaryTimer = true;
            this.timerBar1.Value = 0F;
            // 
            // overwatchBar1
            // 
            this.overwatchBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.overwatchBar1.BarMargin = 0;
            this.overwatchBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overwatchBar1.DrawHatched = true;
            this.overwatchBar1.DrawHatchedOverflow = true;
            this.overwatchBar1.FillColor = System.Drawing.Color.Empty;
            this.overwatchBar1.Interval = 1;
            this.overwatchBar1.Location = new System.Drawing.Point(0, 0);
            this.overwatchBar1.MaxValue = 1F;
            this.overwatchBar1.Name = "overwatchBar1";
            this.overwatchBar1.OverflowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(211)))), ((int)(((byte)(255)))));
            this.overwatchBar1.OverrideValueCode = null;
            this.overwatchBar1.Size = new System.Drawing.Size(150, 150);
            this.overwatchBar1.TabIndex = 1;
            this.overwatchBar1.TabStop = false;
            this.overwatchBar1.TrackSecondaryTimer = false;
            this.overwatchBar1.Value = 0F;
            // 
            // BarContents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timerBar1);
            this.Controls.Add(this.overwatchBar1);
            this.Name = "BarContents";
            this.ResumeLayout(false);

        }

        #endregion

        private NewTimer.FormParts.TimerBar overwatchBar1;
        private FormParts.TimerBar timerBar1;
    }
}
