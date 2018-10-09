namespace NewTimer.Forms.Circle
{
    partial class CircleContents
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
            this.overwatchCircle4 = new NewTimer.FormParts.OverwatchCircle();
            this.SuspendLayout();
            // 
            // overwatchCircle4
            // 
            this.overwatchCircle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.overwatchCircle4.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(234)))), ((int)(((byte)(127))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(123)))), ((int)(((byte)(117))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(109)))), ((int)(((byte)(61))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(161)))), ((int)(((byte)(203))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(233)))), ((int)(((byte)(218))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(187)))), ((int)(((byte)(161))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(132)))), ((int)(((byte)(36))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(68)))), ((int)(((byte)(110))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(231)))), ((int)(((byte)(40)))))};
            this.overwatchCircle4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overwatchCircle4.Location = new System.Drawing.Point(0, 0);
            this.overwatchCircle4.Name = "overwatchCircle4";
            this.overwatchCircle4.Size = new System.Drawing.Size(150, 150);
            this.overwatchCircle4.TabIndex = 1;
            // 
            // CircleContents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.overwatchCircle4);
            this.Name = "CircleContents";
            this.ResumeLayout(false);

        }

        #endregion

        private FormParts.OverwatchCircle overwatchCircle4;
    }
}
