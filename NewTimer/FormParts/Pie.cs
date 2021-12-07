using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.FormParts
{
    public class Pie : UserControl, ICountdown
    {
        Color _fcs = Color.Gray;
        public Color ForeColorSecondary
        {
            get => _fcs;
            set
            {
                if (value.A != 0xff)
                {
                    return;
                }
                _fcs = value;
            }
        }

        public void OnCountdownTick(TimeSpan span, bool isOvertime)
        {
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            const int W = 10;

            base.OnPaint(e);
            TimeSpan tl = Globals.PrimaryTimer.TimeLeft;

            using (Brush b1 = new SolidBrush(ForeColor))
            using (Brush b2 = new SolidBrush(ForeColorSecondary))
            {
                if (tl.Hours >= 1)
                {
                    e.Graphics.FillEllipse(tl.Hours % 2 == 0 ? b1 : b2, W, W, Width - 2 * W, Height - 2 * W);
                }
                e.Graphics.FillPie(tl.Hours % 2 == 0 ? b2 : b1, 0, 0, Width, Height, -90, tl.Minutes / 60f * 360);
            }
        }
    }
}
