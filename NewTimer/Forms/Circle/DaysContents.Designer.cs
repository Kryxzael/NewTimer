namespace NewTimer.Forms.Circle
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
            this.overwatchCircle3 = new NewTimer.FormParts.OverwatchCircle();
            this.DaysOnlyDay = new NewTimer.FormParts.AutoLabel();
            this.SuspendLayout();
            // 
            // overwatchCircle3
            // 
            this.overwatchCircle3.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(67)))), ((int)(((byte)(56))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(43)))), ((int)(((byte)(22))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(118)))), ((int)(((byte)(126))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(49)))), ((int)(((byte)(196))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(187)))), ((int)(((byte)(81))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(194)))), ((int)(((byte)(196))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(118)))), ((int)(((byte)(229))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(53)))), ((int)(((byte)(162))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))))};
            this.overwatchCircle3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overwatchCircle3.Location = new System.Drawing.Point(0, 0);
            this.overwatchCircle3.Name = "overwatchCircle3";
            this.overwatchCircle3.Size = new System.Drawing.Size(150, 150);
            this.overwatchCircle3.TabIndex = 2;
            // 
            // DaysOnlyDay
            // 
            this.DaysOnlyDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DaysOnlyDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 66.0731F);
            this.DaysOnlyDay.GetText = null;
            this.DaysOnlyDay.Location = new System.Drawing.Point(0, 0);
            this.DaysOnlyDay.Name = "DaysOnlyDay";
            this.DaysOnlyDay.Size = new System.Drawing.Size(150, 150);
            this.DaysOnlyDay.TabIndex = 3;
            this.DaysOnlyDay.Text = "0?";
            this.DaysOnlyDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DaysContents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DaysOnlyDay);
            this.Controls.Add(this.overwatchCircle3);
            this.Name = "DaysContents";
            this.ResumeLayout(false);

        }

        #endregion

        private FormParts.OverwatchCircle overwatchCircle3;
        private FormParts.AutoLabel DaysOnlyDay;
    }
}
