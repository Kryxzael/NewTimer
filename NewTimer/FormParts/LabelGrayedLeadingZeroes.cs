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
    /// A standard label that will gray out leading zeros
    /// </summary>
    public class LabelGrayedLeadingZeros : Label
    {

        /*
         * Private Backing fields
         */

        private Color _leadingZeros = Color.Gray;
        private float _progress;
        private bool _renderLeadingZeros;
        private Color _hiColor;

        /// <summary>
        /// Get or sets the amount of the label that should be colored with highlighting color. Intended range is 0 through 1
        /// </summary>
        public float Progress
        {
            get => _progress;
            set => _progress = Math.Max(0, Math.Min(1, value));
        }

        /// <summary>
        /// Gets or sets the leading zeros' color
        /// </summary>
        public Color LeadingZerosColor
        {
            get => _leadingZeros;
            set
            {
                _leadingZeros = value;
                Invalidate();
            }
        }


        /// <summary>
        /// False by default. If true, leading zeros will be rendered
        /// </summary>
        public bool RenderLeadingZeros
        {
            get => _renderLeadingZeros;
            set
            {
                _renderLeadingZeros = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the color that will be used for progress highlighting
        /// </summary>
        public Color HighlightColor
        {
            get => _hiColor;
            set
            {
                _hiColor = value;
                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int offset = 0;
            bool renderGrays = !RenderLeadingZeros;

            /*
             * It might be weird that the background text code uses the highlight color and vice versa.
             * This is to make the effect go up and not down
             * It was done on purpose
             */

            //Draws the "background text"
            foreach (char i in Text)
            {
                Rectangle offsetClipRect = e.ClipRectangle;
                offsetClipRect.X += offset;
                offsetClipRect.Width -= offset;

                if (i != '0')
                {
                    renderGrays = false;
                }

                //Draw the char
                TextRenderer.DrawText(
                    dc: e.Graphics, 
                    text: i.ToString(), 
                    font: Font, 
                    bounds: offsetClipRect, 
                    foreColor: renderGrays ? LeadingZerosColor : HighlightColor, 
                    flags: TextFormatFlags.NoPadding
                );

                //Add char's width to offset variable
                offset += TextRenderer.MeasureText(
                    dc: e.Graphics, 
                    text: i.ToString(), 
                    font: Font, 
                    proposedSize: offsetClipRect.Size, 
                    flags: TextFormatFlags.NoPadding
                ).Width;
            }

            offset = 0;
            renderGrays = !RenderLeadingZeros;

            //Draws the "foreground text"
            foreach (char i in Text)
            {
                Rectangle offsetClipRect = e.ClipRectangle;
                offsetClipRect.X += offset;
                offsetClipRect.Width -= offset;
                offsetClipRect.Height = (int)(offsetClipRect.Height * (_progress));

                if (i != '0')
                {
                    renderGrays = false;
                }

                //Draw the char
                TextRenderer.DrawText(
                    dc: e.Graphics,
                    text: i.ToString(),
                    font: Font,
                    bounds: offsetClipRect,
                    foreColor: renderGrays ? LeadingZerosColor : ForeColor,
                    flags: TextFormatFlags.NoPadding
                );

                //Add char's width to offset variable
                offset += TextRenderer.MeasureText(
                    dc: e.Graphics,
                    text: i.ToString(),
                    font: Font,
                    proposedSize: offsetClipRect.Size,
                    flags: TextFormatFlags.NoPadding
                ).Width;
            }
        }
    }
}
