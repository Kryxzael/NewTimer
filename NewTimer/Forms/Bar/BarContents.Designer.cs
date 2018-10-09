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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BarContents));
            this.overwatchBar1 = new NewTimer.FormParts.TimerBar();
            this.SuspendLayout();
            // 
            // overwatchBar1
            // 
            this.overwatchBar1.BackColor = System.Drawing.SystemColors.Control;
            this.overwatchBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overwatchBar1.Location = new System.Drawing.Point(0, 0);
            this.overwatchBar1.Name = "overwatchBar1";
            this.overwatchBar1.OverflowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(211)))), ((int)(((byte)(255)))));
            this.overwatchBar1.Size = new System.Drawing.Size(150, 150);
            this.overwatchBar1.TabIndex = 1;
            this.overwatchBar1.TabStop = false;
            // 
            // BarContents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.overwatchBar1);
            this.Name = "BarContents";
            this.ResumeLayout(false);

        }

        #endregion

        private NewTimer.FormParts.TimerBar overwatchBar1;
    }
}
