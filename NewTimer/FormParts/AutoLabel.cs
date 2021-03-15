using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.FormParts
{
    /// <summary>
    /// A label control whose text will automaticly be set by a function delegate
    /// </summary>
    public class AutoLabel : Label, ICountdown
    {
        /// <summary>
        /// Gets or sets the delegate that will get the text for this label
        /// </summary>
        public Func<string> GetText { get; set; }

        /// <summary>
        /// Gets the text of this autolabel
        /// </summary>
        public override string Text
        {
            get
            {
                if (GetText == null)
                {
                    return "0?";
                }

                return GetText();
            }
        }

        /// <summary>
        /// Autoscales the label to fill its area
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_requestsResize)
            {
                //Calculates the new font size
                float newFontEm = NewFontSize(e.Graphics, Size, Font, Text);

                //Upon error, use the current font size
                if (newFontEm < 0)
                {
                    newFontEm = Font.Size;
                }

                //Generate the new font and dispose the old one
                Font oldFont = Font;
                Font = new Font(Font.FontFamily, newFontEm, Font.Style);
                oldFont.Dispose();
                _requestsResize = false;
            }
        }

        /// <summary>
        /// This control has been resized and a new font must be made
        /// </summary>
        private bool _requestsResize;
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _requestsResize = true;
        }

        /// <summary>
        /// Uses the given graphics, size, font and text to calculate the maximum size this label can have
        /// </summary>
        /// <param name="graphics">The graphics that will be drawing the text</param>
        /// <param name="size">The size of the container</param>
        /// <param name="font">The font that will be used to draw the string</param>
        /// <param name="str">The text that will be drawn</param>
        /// <returns></returns>
        public static float NewFontSize(Graphics graphics, Size size, Font font, string str)
        {
            try
            {
                //Don't ask
                SizeF stringSize = graphics.MeasureString(str, font);
                float wRatio = size.Width / stringSize.Width;
                float hRatio = size.Height / stringSize.Height;
                float ratio = Math.Min(hRatio, wRatio);
                return font.Size * ratio - 10;
            }
            catch (Exception)
            {
                return -1;
            }

        }

        public void OnCountdownTick(TimeSpan span, bool isOvertime)
        {
            Refresh();
        }
    }
}
