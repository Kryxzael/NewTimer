using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.FormParts
{
    public class ColorSchemeComboBox : ComboBox
    {
        public ColorSchemeComboBox()
        {
            //DrawMode = DrawMode.OwnerDrawFixed;
            DrawMode = DrawMode.Normal;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (Items[e.Index] is ColorScheme s)
            {
                e.DrawBackground();

                const int MARGIN = 2;
                Size cellSize = new Size(e.Bounds.Height - MARGIN * 2, e.Bounds.Height - MARGIN * 2);

                //Get colors to use
                Brush[] previewColors = s.GenerateMany(5, Config.MasterRandom)
                    .Select(i => new SolidBrush(i))
                    .ToArray();

                for (int i = 0; i < previewColors.Length; i++)
                {
                    Rectangle area = new Rectangle(new Point(i * (cellSize.Width + MARGIN), MARGIN), cellSize);

                    e.Graphics.FillRectangle(previewColors[i], area);
                    e.Graphics.DrawRectangle(Pens.Black, area);
                }

                e.DrawFocusRectangle();
            }
            else
            {
                base.OnDrawItem(e);
            }

            
        }
    }
}
