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
    /// A combobox designed to display a preview of a color scheme
    /// </summary>
    public class ColorSchemeComboBox : ComboBox
    {
        public ColorSchemeComboBox()
        {
            DrawMode = DrawMode.Normal;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            //The given element is a color scheme and can be handled customly
            if (Items[e.Index] is ColorScheme s)
            {
                //Draws the background
                e.DrawBackground();

                //Calculates the size of each cell of the preview
                const int MARGIN = 2;
                Size cellSize = new Size(e.Bounds.Height - MARGIN * 2, e.Bounds.Height - MARGIN * 2);

                //Get colors to use
                Brush[] previewColors = s.GenerateForPreview(5, Config.MasterRandom)
                    .Select(i => new SolidBrush(i))
                    .ToArray();

                //Draw the cells
                for (int i = 0; i < previewColors.Length; i++)
                {
                    //Gets area to draw cell in
                    Rectangle area = new Rectangle(new Point(i * (cellSize.Width + MARGIN), e.Bounds.Top + MARGIN), cellSize);

                    //Draw color
                    e.Graphics.FillRectangle(previewColors[i], area);

                    //Draw border
                    e.Graphics.DrawRectangle(Pens.Black, area);

                    previewColors[i].Dispose();
                }

                //Draws the focus rectangle if selected
                e.DrawFocusRectangle();
            }

            //Not a color scheme, treat as normal text
            else
            {
                base.OnDrawItem(e);
            }

            
        }
    }
}
