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

        /// <summary>
        /// Gets or sets the information that is currently being rendered
        /// </summary>
        public MicroViewCommand CurrentCommand { get; set; }

        /// <summary>
        /// Gets or sets the information that is currently being rendered as a secondary command
        /// </summary>
        public MicroViewCommand SecondaryCommand { get; set; }
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
            ForeColor = Globals.PrimaryTimer.ColorScheme.GenerateOne(Globals.MasterRandom);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ForeColor = Globals.PrimaryTimer.ColorScheme.GenerateOne(Globals.MasterRandom);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Brush fgBrush = new SolidBrush(ForeColor);
            Brush bgBrush = new SolidBrush(Color.FromArgb(0x5F, Color.Black));

            string numDisplay;
            bool displayDot;
            bool displaySecondaryDot = false;
            char offset;
            char unit = CurrentCommand.Unit;

            getDisplaySettings(CurrentCommand.Number, CurrentCommand.AllowDecimals, out numDisplay, out displayDot, out offset);

            if (DateTime.Now.Second % 10 <= 5 && SecondaryCommand.Number < 100)
            {
                string secondaryTimerDisplay;
                getDisplaySettings(SecondaryCommand.Number, CurrentCommand.AllowDecimals, out secondaryTimerDisplay, out displaySecondaryDot, out _);

                offset = secondaryTimerDisplay[0];
                unit = secondaryTimerDisplay[1];
            }

            e.Graphics.DrawString("88", DEFAULT_FONT, bgBrush, new Point(0, 0));
            e.Graphics.DrawString("8", SMALL_FONT, bgBrush, new Point(PANEL_WIDTH - 20, PANEL_HEIGHT - 20));
            e.Graphics.DrawString(".",  DEFAULT_FONT, bgBrush, new Point(19, 0));
            e.Graphics.DrawString(".", SMALL_FONT, bgBrush, new Point(PANEL_WIDTH - 20, 15));


            e.Graphics.DrawString(numDisplay, DEFAULT_FONT, fgBrush, new Point(0, 0));
            e.Graphics.DrawString(offset.ToString(),  SMALL_FONT,   fgBrush, new Point(PANEL_WIDTH - 20, 10));
            e.Graphics.DrawString(unit.ToString(),  SMALL_FONT,   fgBrush, new Point(PANEL_WIDTH - 20, PANEL_HEIGHT - 20));


            if (displayDot)
                e.Graphics.DrawString(".", DEFAULT_FONT, fgBrush, new Point(19, 0));

            if (displaySecondaryDot)
                e.Graphics.DrawString(".", SMALL_FONT, fgBrush, new Point(PANEL_WIDTH - 20, 15));

            fgBrush.Dispose();
            bgBrush.Dispose();

            char getOffsetMarker(double d)
            {
                d %= 1.0;

                if (d < 1.0 / 3.0)
                    return 'v';

                if (d < 2.0 / 3.0)
                    return '-';

                return '^';
            }

            void getDisplaySettings(double input, bool allowDecimals, out string output, out bool showDot, out char offsetOutput)
            {
                if (input >= 100)
                {
                    output = "--";
                    showDot = false;
                    offsetOutput = ' ';
                }
                else if (input > 10)
                {
                    output = Math.Floor(input).ToString("00", CultureInfo.InvariantCulture);
                    showDot = false;
                    offsetOutput = getOffsetMarker(input);
                }
                else if (allowDecimals)
                {
                    //ToString was annoying with rounding instead of flooring, so doing this instead
                    int num = (int)(input * 10);

                    output = num.ToString("00", CultureInfo.InvariantCulture);
                    showDot = true;
                    offsetOutput = getOffsetMarker(input * 10);
                }
                else
                {
                    output = " " + Math.Floor(input).ToString("0", CultureInfo.InvariantCulture);
                    showDot = false;
                    offsetOutput = getOffsetMarker(input);
                }
            }
        }

        public void OnCountdownTick(TimeSpan span, TimeSpan secondarySpan, bool isOvertime)
        {
            if (Globals.PrimaryTimer.InFreeMode)
            {
                char amPm = DateTime.Now.Hour < 12 ? 'A' : 'P';
                int hour = DateTime.Now.Hour;

                if (DateTime.Now.Second % 10 > 5)
                {
                    hour %= 12;

                    if (hour == 0)
                        hour = 12;
                }

                if (hour >= 10)
                {
                    //                                                        v Cheat to make offset display work
                    CurrentCommand = new MicroViewCommand(hour + DateTime.Now.Second / 60f, amPm, false);
                }
                else
                {
                    CurrentCommand = new MicroViewCommand(hour + DateTime.Now.Second / 600f, amPm, false);
                }

                
                SecondaryCommand = new MicroViewCommand(DateTime.Now.Minute, amPm, false);
            }
            else
            {
                if (span.TotalSeconds < 100)
                    CurrentCommand = new MicroViewCommand(span.TotalSeconds, ' ', false);

                else if (span.TotalMinutes < 100)
                    CurrentCommand = new MicroViewCommand(span.TotalMinutes, 'M', true);

                else if (span.TotalHours < 100)
                    CurrentCommand = new MicroViewCommand(span.TotalHours, 'H', true);

                else
                    CurrentCommand = new MicroViewCommand(span.TotalDays, 'D', true);
            }
            


            if (!Globals.SecondaryTimer.InFreeMode && !Globals.PrimaryTimer.InFreeMode)
            {
                if (Globals.SecondaryTimer.TimeLeft.TotalSeconds < 100)
                    SecondaryCommand = new MicroViewCommand(Globals.SecondaryTimer.TimeLeft.TotalSeconds, ' ', false);

                else if (Globals.SecondaryTimer.TimeLeft.TotalMinutes < 100)
                    SecondaryCommand = new MicroViewCommand(Globals.SecondaryTimer.TimeLeft.TotalMinutes, 'M', true);

                else if (Globals.SecondaryTimer.TimeLeft.TotalHours < 100)
                    SecondaryCommand = new MicroViewCommand(Globals.SecondaryTimer.TimeLeft.TotalHours, 'H', true);

                else
                    SecondaryCommand = new MicroViewCommand(Globals.SecondaryTimer.TimeLeft.TotalDays, 'D', true);
            }
            else if (!Globals.PrimaryTimer.InFreeMode)
            {
                SecondaryCommand = new MicroViewCommand(100, ' ', false); //100 just to make sure it isn't displayed
            }


            if (isOvertime)
                BackColor = Globals.GlobalOvertimeColor;

            else
                BackColor = Globals.GlobalBackColor;

            Invalidate();
        }

        public struct MicroViewCommand
        {
            public double Number { get; }
            public char Unit { get; }
            public bool AllowDecimals { get; }

            public MicroViewCommand(double number, char unit, bool allowDecimals)
            {
                Number = Math.Abs(number);
                Unit = unit;
                AllowDecimals = allowDecimals;
            }
        }
    }
}
