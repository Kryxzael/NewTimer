using NewTimer.ThemedColors;

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
        }

        /// <summary>
        /// <inheritdoc />
        /// Changes the color scheme when the control is clicked
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Globals.PrimaryTimer.Recolorize();
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Brush primaryBrush = new SolidBrush(Globals.PrimaryTimer.MicroViewColor);
            Brush secondaryBrush;
            Brush bgBrush = new SolidBrush(Color.FromArgb(0x5F, new ThemedColor(Color.Silver, Color.Black)));

            //Create brush that's used to fade between secondary timer and unit
            if (SecondaryCommand.IsValid && Globals.CurrentMicroBroadcastMessage == null)
            {
                Color baseColor;

                if (DateTime.Now.Second % 10 >= 5 || Globals.PrimaryTimer.InFreeMode)
                    baseColor = Globals.PrimaryTimer.MicroViewColor;

                else
                    baseColor = Globals.SecondaryTimer.MicroViewColor;

                if (DateTime.Now.Second % 5 == 4)
                    secondaryBrush = new SolidBrush(Color.FromArgb((int)((1000 - DateTime.Now.Millisecond) / 1000f * 255), baseColor));

                else if (DateTime.Now.Second % 5 == 0)
                    secondaryBrush = new SolidBrush(Color.FromArgb((int)(DateTime.Now.Millisecond / 1000f * 255), baseColor));

                else
                    secondaryBrush = new SolidBrush(baseColor);
            }
            else
                secondaryBrush = new SolidBrush(Globals.PrimaryTimer.MicroViewColor);


            string numDisplay;
            bool displayDot;
            bool displaySecondaryDot = false;
            char offset;
            char unit = CurrentCommand.Unit;

            if (Globals.CurrentMicroBroadcastMessage != null)
            {
                displayDot = false;
                string paddedMessage = Globals.CurrentMicroBroadcastMessage.PadRight(4);
                numDisplay = paddedMessage.Substring(0, 2);
                offset = paddedMessage[2];
                unit = paddedMessage[3];
            }
            else
            {
                getDisplaySettings(CurrentCommand.Number, CurrentCommand.AllowDecimals, out numDisplay, out displayDot, out offset);

                if (DateTime.Now.Second % 10 < 5 && SecondaryCommand.IsValid)
                {
                    string secondaryTimerDisplay;
                    getDisplaySettings(SecondaryCommand.Number, SecondaryCommand.AllowDecimals, out secondaryTimerDisplay, out displaySecondaryDot, out _);

                    offset = secondaryTimerDisplay[0];
                    unit = secondaryTimerDisplay[1];
                }
            }

            e.Graphics.DrawString("@@", DEFAULT_FONT, bgBrush, new Point(0, 0));
            e.Graphics.DrawString("@", SMALL_FONT, bgBrush, new Point(PANEL_WIDTH - 20, PANEL_HEIGHT - 25));
            e.Graphics.DrawString(".",  DEFAULT_FONT, bgBrush, new Point(19, 0));
            e.Graphics.DrawString(".", SMALL_FONT, bgBrush, new PointF(PANEL_WIDTH - 20, 9.5f));


            e.Graphics.DrawString(numDisplay, DEFAULT_FONT, primaryBrush, new Point(0, 0));
            e.Graphics.DrawString(offset.ToString(),  SMALL_FONT, secondaryBrush, new Point(PANEL_WIDTH - 20, 5));
            e.Graphics.DrawString(unit.ToString(),  SMALL_FONT, secondaryBrush, new Point(PANEL_WIDTH - 20, PANEL_HEIGHT - 25));


            if (displayDot)
                e.Graphics.DrawString(".", DEFAULT_FONT, primaryBrush, new Point(19, 0));

            if (displaySecondaryDot)
                e.Graphics.DrawString(".", SMALL_FONT, secondaryBrush, new PointF(PANEL_WIDTH - 19, 9.5f));

            primaryBrush.Dispose();
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

                    //Number is divisible by ten and should not have a decimal part
                    if (num % 10 == 0)
                    {
                        output = " " + (num / 10).ToString("0", CultureInfo.InvariantCulture);
                        showDot = false;
                    }

                    //Number needs a decimal part
                    else
                    {
                        output = num.ToString("00", CultureInfo.InvariantCulture);
                        showDot = true;
                    }

                    
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

                //                                                        v Cheat to make offset display work
                CurrentCommand   = new MicroViewCommand(hour + DateTime.Now.Second / 60f, amPm, false);
                SecondaryCommand = new MicroViewCommand(DateTime.Now.Minute, amPm, false);
            }
            else
            {
                if (Globals.CurrentBroadcastMessage == null)
                {
                    if (Globals.PrimaryTimer.StopAtZero && isOvertime)
                    {
                        if (Globals.PrimaryTimer.RealTimeLeft.Milliseconds < -500)
                            Globals.Broadcast(null, "");
                        else
                            Globals.Broadcast(null, null);
                    }
                }

                CurrentCommand = Globals.PrimaryTimer.MicroViewUnit.Selector(Globals.PrimaryTimer.TimeLeft);
            }
            


            if (!Globals.SecondaryTimer.InFreeMode && !Globals.PrimaryTimer.InFreeMode)
            {
                SecondaryCommand = Globals.SecondaryTimer.MicroViewUnit.Selector(Globals.SecondaryTimer.TimeLeft);
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

            if (!Globals.PrimaryTimer.InFreeMode && !Globals.PrimaryTimer.Paused)
            {
                broadcastAt("DAY ", 24,  0, false);
                broadcastAt("HOUR",  1,  0, false);
                broadcastAt("HALF",  0, 30, false);
                broadcastAt("QUAT",  0, 15, false);
                broadcastAt("MIN ",  0,  1, false);

                if (!Globals.PrimaryTimer.StopAtZero)
                    broadcastAt("ZERO",  0,  0, true);

                /*
                 * Broadcasts (to micro-view only) the provided message at the given amount of time on the clock
                 */
                void broadcastAt(string msg, int hours, int minutes, bool overtimeOnly)
                {
                    //Do not override "real" broadcasts
                    if (Globals.CurrentBroadcastMessage != null)
                        return;

                    if (!isOvertime && overtimeOnly)
                        return;

                    if (span.Days != 0)
                        return;

                    TimeSpan target = new TimeSpan(hours, minutes, 0);

                    if (!isOvertime)
                        target -= new TimeSpan(0, 0, 1);

                    if (span.Days == target.Days && span.Hours == target.Hours && span.Minutes == target.Minutes && span.Seconds == target.Seconds)
                    {
                        if (isOvertime && span.Milliseconds > 100)
                            return;

                        if (!isOvertime && span.Milliseconds < 900)
                            return;

                        Globals.Broadcast(null, msg);
                    }
                        

                }
            }
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

        public class MicroViewUnitSelector
        {
            /// <summary>
            /// The identifier of the unit selector
            /// </summary>
            public string ID { get; }

            /// <summary>
            /// Two-letter ID used by micro-view broadcasts
            /// </summary>
            public string ShortID { get; }

            /// <summary>
            /// The selector function that will generate commands
            /// </summary>
            public Func<TimeSpan, MicroViewCommand> Selector { get; }

            /// <summary>
            /// Holds all selectors
            /// </summary>
            public static readonly List<MicroViewUnitSelector> All = new List<MicroViewUnitSelector>();

            /// <summary>
            /// Creates a new micro-view unit selector
            /// </summary>
            /// <param name="id"></param>
            /// <param name="selector"></param>
            private MicroViewUnitSelector(string id, string shortId, Func<TimeSpan, MicroViewCommand> selector)
            {
                ID = id;
                ShortID = shortId;
                Selector = selector;
                All.Add(this);
            }

            /// <summary>
            /// Displays time using the most accurate unit available. This is default
            /// </summary>
            public static readonly MicroViewUnitSelector MostAccurate = new MicroViewUnitSelector("default", "- ", span =>
            {
                if (span.TotalSeconds < 100)
                    return new MicroViewCommand(span.TotalSeconds, ' ', false);

                else if (span.TotalMinutes < 100)
                    return new MicroViewCommand(span.TotalMinutes, 'M', true);

                else if (span.TotalHours < 100)
                    return new MicroViewCommand(span.TotalHours, 'H', true);

                else
                    return new MicroViewCommand(span.TotalDays, 'D', true);
            });

            /// <summary>
            /// Displays time using days, hours, minutes and seconds. Switching units when the current unit drops below 1
            /// </summary>
            public static readonly MicroViewUnitSelector MostNatural = new MicroViewUnitSelector("natural", "N ", span =>
            {
                if (span.TotalSeconds < 60)
                    return new MicroViewCommand(span.TotalSeconds, ' ', false);

                else if (span.TotalMinutes < 60)
                    return new MicroViewCommand(span.TotalMinutes, 'M', true);

                else if (span.TotalHours < 24)
                    return new MicroViewCommand(span.TotalHours, 'H', true);

                else
                    return new MicroViewCommand(span.TotalDays, 'D', true);
            });

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector AlwaysSeconds = new MicroViewUnitSelector("seconds", "S ", span =>
            {
                return new MicroViewCommand(span.TotalSeconds, ' ', true);
            });

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector AlwaysMinutes = new MicroViewUnitSelector("minutes", "M ", span =>
            {
                return new MicroViewCommand(span.TotalMinutes, 'M', true);
            });

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector AlwaysHours = new MicroViewUnitSelector("hours", "H ", span =>
            {
                return new MicroViewCommand(span.TotalHours, 'H', true);
            });

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector AlwaysDays = new MicroViewUnitSelector("days", "D ", span =>
            {
                return new MicroViewCommand(span.TotalDays, 'D', true);
            });

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector MinimumMinutes = new MicroViewUnitSelector("min-minutes", "MM", span =>
            {
                if (span.TotalMinutes < 60)
                    return new MicroViewCommand(span.TotalMinutes, 'M', true);

                else if (span.TotalHours < 24)
                    return new MicroViewCommand(span.TotalHours, 'H', true);

                else
                    return new MicroViewCommand(span.TotalDays, 'D', true);
            });

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector MinimumHours = new MicroViewUnitSelector("min-hours", "MH", span =>
            {
                if (span.TotalHours < 24)
                    return new MicroViewCommand(span.TotalHours, 'H', true);

                else
                    return new MicroViewCommand(span.TotalDays, 'D', true);
            });
        }
    }
}
