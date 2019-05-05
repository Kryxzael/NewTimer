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
    [DesignerCategory("")]
    public class TimeSugestion : UserControl
    {
        public override string Text { get => base.Text; set => base.Text = value; }

        /// <summary>
        /// Gets or sets the time this sugestion button will target when clicked
        /// </summary>
        public DateTime Target;

        public TimeSugestion()
        {
            Margin = new Padding(1, 0, 1, 0);
            Size = new Size(105, 25);
        }

        public TimeSugestion(string text, DateTime target) : this()
        {
            Text = text;
            Target = target;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            //Draws the text
            Width = TextRenderer.MeasureText(e.Graphics, Text, Font, e.ClipRectangle.Size).Width + 10;

            //Draws background
            e.Graphics.DrawImage(Properties.Resources.TimeSuggestionBG, new RectangleF(0, 0, Width, Height));


            using (SolidBrush b = new SolidBrush(ForeColor))
                e.Graphics.DrawString(Text, Font, b, new RectangleF(0, 0, Width, Height), new StringFormat(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });

            
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            Config.StartTimer(Target, (ParentForm as Forms.Setup).GetSelectedColorScheme(), ParentForm);
        }
    }
}
