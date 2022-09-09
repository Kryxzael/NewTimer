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
    /// <summary>
    /// A small minimal control that displays the timers
    /// </summary>
    public class MicroView : UserControl, ICountdown
    {
        private static readonly PrivateFontCollection FONT_STORE = new PrivateFontCollection();
        private static readonly FontFamily            DEFAULT_FONT_FAM;
        private static readonly Font                  DEFAULT_FONT;
        private static readonly Font                  SMALL_FONT;

        private const int   PANEL_WIDTH     = 115;
        private const int   PANEL_HEIGHT    =  55;
        private const float FONT_SIZE       =  40f;
        private const float SMALL_FONT_SIZE =  14f;

        /// <summary>
        /// Gets or sets the information that is currently being rendered
        /// </summary>
        public MicroViewCommand CurrentCommand { get; set; }

        /// <summary>
        /// Gets or sets the information that is currently being rendered as a secondary command
        /// </summary>
        public MicroViewCommand SecondaryCommand { get; set; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override Size MinimumSize
        {
            get => new Size(PANEL_WIDTH, PANEL_HEIGHT);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override Size MaximumSize
        {
            get => new Size(PANEL_WIDTH, PANEL_HEIGHT);
        }

        /// <summary>
        /// Loads the custom font into RAM
        /// </summary>
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

        /// <summary>
        /// Creates a new micro view
        /// </summary>
        public MicroView()
        {
            Size = new Size(PANEL_WIDTH, PANEL_HEIGHT);
            DoubleBuffered = true;
            ForeColor = Globals.PrimaryTimer.ColorScheme.GenerateOne(Globals.MasterRandom);
        }

        /// <summary>
        /// <inheritdoc />
        /// Changes the color scheme when the control is clicked
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ForeColor = Globals.PrimaryTimer.ColorScheme.GenerateOne(Globals.MasterRandom);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Brush fgBrush = new SolidBrush(ForeColor);
            Brush fgFadeBrush = null;
            Brush bgBrush = new SolidBrush(Color.FromArgb(0x5F, Color.Black));

            //Create brush that's used to fade between secondary timer and unit
            if (SecondaryCommand.IsValid)
            {
                if (DateTime.Now.Second % 5 == 4)
                    fgFadeBrush = new SolidBrush(Color.FromArgb((int)((1000 - DateTime.Now.Millisecond) / 1000f * 255), ForeColor));

                else if (DateTime.Now.Second % 5 == 0)
                    fgFadeBrush = new SolidBrush(Color.FromArgb((int)(DateTime.Now.Millisecond / 1000f * 255), ForeColor));
            }

            if (fgFadeBrush == null)
                fgFadeBrush = new SolidBrush(ForeColor);


            string numDisplay;
            bool displayDot;
            bool displaySecondaryDot = false;
            char offset;
            char unit = CurrentCommand.Unit;

            getDisplaySettings(CurrentCommand.Number, CurrentCommand.AllowDecimals, out numDisplay, out displayDot, out offset);

            if (DateTime.Now.Second % 10 < 5 && SecondaryCommand.IsValid)
            {
                string secondaryTimerDisplay;
                getDisplaySettings(SecondaryCommand.Number, SecondaryCommand.AllowDecimals, out secondaryTimerDisplay, out displaySecondaryDot, out _);

                offset = secondaryTimerDisplay[0];
                unit = secondaryTimerDisplay[1];
            }

            e.Graphics.DrawString("@@", DEFAULT_FONT, bgBrush, new Point(0, 0));
            e.Graphics.DrawString("@", SMALL_FONT, bgBrush, new Point(PANEL_WIDTH - 20, PANEL_HEIGHT - 25));
            e.Graphics.DrawString(".",  DEFAULT_FONT, bgBrush, new Point(19, 0));
            e.Graphics.DrawString(".", SMALL_FONT, bgBrush, new PointF(PANEL_WIDTH - 20, 9.5f));


            e.Graphics.DrawString(numDisplay, DEFAULT_FONT, fgBrush, new Point(0, 0));
            e.Graphics.DrawString(offset.ToString(),  SMALL_FONT, fgFadeBrush, new Point(PANEL_WIDTH - 20, 5));
            e.Graphics.DrawString(unit.ToString(),  SMALL_FONT, fgFadeBrush, new Point(PANEL_WIDTH - 20, PANEL_HEIGHT - 25));


            if (displayDot)
                e.Graphics.DrawString(".", DEFAULT_FONT, fgBrush, new Point(19, 0));

            if (displaySecondaryDot)
                e.Graphics.DrawString(".", SMALL_FONT, fgFadeBrush, new PointF(PANEL_WIDTH - 19, 9.5f));

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
                else if (input >= 10)
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

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="span"></param>
        /// <param name="secondarySpan"></param>
        /// <param name="isOvertime"></param>
        public void OnCountdownTick(TimeSpan span, TimeSpan secondarySpan, bool isOvertime)
        {
            if (Globals.PrimaryTimer.InFreeMode)
            {
                char amPm = DateTime.Now.Hour < 12 ? 'A' : 'P';
                int hour = DateTime.Now.Hour;

                if (DateTime.Now.Second % 10 >= 5)
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
                if (secondarySpan.TotalSeconds < 100)
                    SecondaryCommand = new MicroViewCommand(secondarySpan.TotalSeconds, ' ', false);

                else if (secondarySpan.TotalMinutes < 100)
                    SecondaryCommand = new MicroViewCommand(secondarySpan.TotalMinutes, 'M', true);

                else if (secondarySpan.TotalHours < 100)
                    SecondaryCommand = new MicroViewCommand(secondarySpan.TotalHours, 'H', true);

                else
                    SecondaryCommand = new MicroViewCommand(secondarySpan.TotalDays, 'D', true);
            }
            else if (!Globals.PrimaryTimer.InFreeMode)
            {
                SecondaryCommand = MicroViewCommand.INVALID;
            }


            if (isOvertime)
                BackColor = Globals.GlobalOvertimeColor;

            else
                BackColor = Globals.GlobalBackColor;

            Invalidate();
        }

        /// <summary>
        /// Represents settings for how the control will display content
        /// </summary>
        public struct MicroViewCommand
        {
            /// <summary>
            /// A command that will show up as two dashes in primary, and not show up at all as secondary
            /// </summary>
            public static readonly MicroViewCommand INVALID = new MicroViewCommand(100, ' ', false);

            /// <summary>
            /// Gets the number to display. This number is always non-negative
            /// </summary>
            public double Number { get; }

            /// <summary>
            /// Gets the character to display in the unit section. This field is unused for secondary commands
            /// </summary>
            public char Unit { get; }

            /// <summary>
            /// Gets whether decimals are trimmed or kept when the number is less than ten
            /// </summary>
            public bool AllowDecimals { get; }

            /// <summary>
            /// Gets whether this command has valid data
            /// </summary>
            public bool IsValid
            {
                get
                {
                    return Number < 100;
                }
            }

            /// <summary>
            /// Creates a new command
            /// </summary>
            /// <param name="number"></param>
            /// <param name="unit"></param>
            /// <param name="allowDecimals"></param>
            public MicroViewCommand(double number, char unit, bool allowDecimals)
            {
                Number = Math.Abs(number);
                Unit = unit;
                AllowDecimals = allowDecimals;
            }
        }
    }
}
