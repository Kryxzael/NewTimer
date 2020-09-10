using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.FormParts.Setup
{
    /// <summary>
    /// A button that acts as a quick selector at the bottom of the setup window. Clicking it will instantly initialize and start the timer
    /// </summary>
    [DesignerCategory("")]
    public class TimeSugestion : UserControl
    {
        public override string Text { get => base.Text; set => base.Text = value; } //Overriden for visibility in inspector

        /// <summary>
        /// Gets or sets the delegate used to get the time this sugestion button will target when clicked
        /// </summary>
        public Func<DateTime> GetTarget;

        /// <summary>
        /// Disclaimer: Refrain from using this constructor
        /// </summary>
        public TimeSugestion()
        {
            Margin = new Padding(1, 0, 1, 0);
            Size = new Size(105, 25);
        }

        /// <summary>
        /// Creates a new button
        /// </summary>
        /// <param name="text"></param>
        /// <param name="getTarget"></param>
        public TimeSugestion(string text, Func<DateTime> getTarget) : this()
        {
            Text = text;
            GetTarget = getTarget;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            //Gets the width of the text to messure how big the button needs to be
            Width = TextRenderer.MeasureText(e.Graphics, Text, Font, e.ClipRectangle.Size).Width + 10;

            //Draws background
            e.Graphics.DrawImage(Properties.Resources.TimeSuggestionBG, new RectangleF(0, 0, Width, Height));

            //Draws the text
            using (SolidBrush b = new SolidBrush(ForeColor))
                e.Graphics.DrawString(
                    s: Text, 
                    font: Font, 
                    brush: b, 
                    layoutRectangle: new RectangleF(0, 0, Width, Height), 
                    format: new StringFormat(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center }
                );

            
        }

        /// <summary>
        /// Sets and starts the timer when the button is clicked
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Config.StartTimer(
                target: GetTarget(), 
                colorScheme: (ParentForm as Forms.Setup).GetSelectedColorScheme(), 
                stopAtZero: (ParentForm as Forms.Setup).chkStopAtZero.Checked,
                closingForm: ParentForm
            );
        }
    }
}
