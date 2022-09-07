using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.FormParts
{
    public class MicroView : UserControl, ICountdown
    {
        private static readonly PrivateFontCollection FONT_STORE = new PrivateFontCollection();
        private static readonly FontFamily            DEFAULT_FONT_FAM;
        private static readonly Font                  DEFAULT_FONT;
        private static readonly Font                  SMALL_FONT;

        private const int   PANEL_WIDTH        = 120;
        private const int   PANEL_HEIGHT       =  55;
        private const float FONT_SIZE          =  40f;
        private const float SMALL_FONT_SIZE    =  14f;

        private MicroViewCommand _currentCommand;

        /// <summary>
        /// Gets or sets the information that is currently being rendered
        /// </summary>
        public MicroViewCommand CurrentCommand
        {
            get => _currentCommand;
            set
            {
                _currentCommand = value;
                Invalidate();
            }
        }

        public override Size MinimumSize
        {
            get => new Size(PANEL_WIDTH, PANEL_HEIGHT);
        }

        public override Size MaximumSize
        {
            get => new Size(PANEL_WIDTH, PANEL_HEIGHT);
        }

        static MicroView()
        {
            byte[] fontData = Properties.Resources.deffont;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            FONT_STORE.AddMemoryFont(fontPtr, fontData.Length);
            DEFAULT_FONT_FAM = FONT_STORE.Families[0];

            DEFAULT_FONT = new Font(DEFAULT_FONT_FAM, FONT_SIZE);
            SMALL_FONT   = new Font(DEFAULT_FONT_FAM, SMALL_FONT_SIZE);
        }

        public MicroView()
        {
            Size = new Size(PANEL_WIDTH, PANEL_HEIGHT);
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Brush fgBrush = new SolidBrush(ForeColor);

            string numDisplay;
            bool displayDot;
            char offset;

            if (CurrentCommand.Number >= 100)
            {
                numDisplay = "--";
                displayDot = false;
                offset = ' ';
            }
            else if (CurrentCommand.Number > 10)
            {
                numDisplay = Math.Floor(CurrentCommand.Number).ToString("00", CultureInfo.InvariantCulture);
                displayDot = false;
                offset = getOffsetMarker(CurrentCommand.Number);
            }
            else
            {
                //ToString was annoying with rounding instead of flooring, so doing this instead
                int num = (int)(CurrentCommand.Number * 10);

                numDisplay = num.ToString("00", CultureInfo.InvariantCulture);
                displayDot = true;
                offset = getOffsetMarker(CurrentCommand.Number * 10);

            }

            e.Graphics.DrawString(numDisplay, DEFAULT_FONT, fgBrush, new Point(0, 0));
            e.Graphics.DrawString(offset.ToString(),  SMALL_FONT,   fgBrush, new Point(PANEL_WIDTH - 20, 10));
            e.Graphics.DrawString(CurrentCommand.Unit.ToString(),  SMALL_FONT,   fgBrush, new Point(PANEL_WIDTH - 20, PANEL_HEIGHT - 20));

            if (displayDot)
                e.Graphics.DrawString(".",  DEFAULT_FONT, fgBrush, new Point(19, 0));

            fgBrush.Dispose();

            char getOffsetMarker(double d)
            {
                d %= 1.0;

                if (d < 1.0 / 3.0)
                    return 'v';

                if (d < 2.0 / 3.0)
                    return '-';

                return '^';
            }
        }

        public void OnCountdownTick(TimeSpan span, TimeSpan secondarySpan, bool isOvertime)
        {
            if (span.TotalSeconds < 100)
                CurrentCommand = new MicroViewCommand(span.TotalSeconds, ' ');

            else if (span.TotalMinutes < 100)
                CurrentCommand = new MicroViewCommand(span.TotalMinutes, 'M');

            else if (span.TotalHours < 100)
                CurrentCommand = new MicroViewCommand(span.TotalHours, 'H');

            else
                CurrentCommand = new MicroViewCommand(span.TotalDays, 'D');
        }

        public struct MicroViewCommand
        {
            public double Number { get; }
            public char Unit { get; }

            public MicroViewCommand(double number, char unit)
            {
                Number = Math.Abs(number);
                Unit = unit;
            }
        }
    }
}
