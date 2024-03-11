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

        private const int PANEL_WIDTH        = 115;
        private const int PANEL_LONG_WIDTH   = 157;
        private const int PANEL_HEIGHT       =  55;
        private const int SMALL_DIGITS_WIDTH =  20;

        private const float FONT_SIZE       =  40f;
        private const float SMALL_FONT_SIZE =  14f;

        /// <summary>
        /// If set, three digits will be used instead of two
        /// </summary>
        public bool LongView { get; set; } = true;

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
            get => new Size(LongView ? PANEL_LONG_WIDTH : PANEL_WIDTH, PANEL_HEIGHT);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override Size MaximumSize
        {
            get => new Size(LongView ? PANEL_LONG_WIDTH : PANEL_WIDTH, PANEL_HEIGHT);
        }

        /// <summary>
        /// Gets the width of the big digit area
        /// </summary>
        private int BigDigitsWidth
        {
            get
            {
                if (LongView)
                    return PANEL_LONG_WIDTH - SMALL_DIGITS_WIDTH;

                return PANEL_WIDTH - SMALL_DIGITS_WIDTH;
            }
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
            Brush fadedPrimaryBrush = new SolidBrush(Color.FromArgb(0x4F, Globals.PrimaryTimer.MicroViewColor));
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
            DecimalSeparatorPosition primarySeparatorPos;
            DecimalSeparatorPosition secondarySeparatorPos = DecimalSeparatorPosition.NoDecimalSeparator;
            char offset;
            char unit = CurrentCommand.Unit;

            if (Globals.CurrentMicroBroadcastMessage != null)
            {
                primarySeparatorPos = DecimalSeparatorPosition.NoDecimalSeparator;

                if (LongView)
                {
                    string paddedMessage = Globals.CurrentLongMicroBroadcastMessage.PadRight(5);
                    numDisplay = paddedMessage.Substring(0, 3);
                    offset = paddedMessage[3];
                    unit = paddedMessage[4];
                }
                else
                {
                    string paddedMessage = Globals.CurrentMicroBroadcastMessage.PadRight(4);
                    numDisplay = paddedMessage.Substring(0, 2);
                    offset = paddedMessage[2];
                    unit = paddedMessage[3];
                }

            }
            else
            {
                getDisplaySettings(CurrentCommand.Number, CurrentCommand.AllowDecimals, LongView, out numDisplay, out primarySeparatorPos, out offset);

                if (DateTime.Now.Second % 10 < 5 && SecondaryCommand.IsValid)
                {
                    string secondaryTimerDisplay;
                    getDisplaySettings(SecondaryCommand.Number, SecondaryCommand.AllowDecimals, false, out secondaryTimerDisplay, out secondarySeparatorPos, out _);

                    offset = secondaryTimerDisplay[0];
                    unit = secondaryTimerDisplay[1];
                }
            }

            const char OFFSET_BACKGROUND = 'Η';
            e.Graphics.DrawString(LongView ? "@@@" : "@@", DEFAULT_FONT, bgBrush, new Point(0, 0));
            e.Graphics.DrawString("@", SMALL_FONT, bgBrush, new Point(BigDigitsWidth, PANEL_HEIGHT - 25));
            e.Graphics.DrawString(OFFSET_BACKGROUND.ToString(), SMALL_FONT, bgBrush, new Point(BigDigitsWidth, 5));
            e.Graphics.DrawString(".", DEFAULT_FONT, bgBrush, new Point(19, 0));
            if (LongView) e.Graphics.DrawString(".", DEFAULT_FONT, bgBrush, new Point(64, 0));
            e.Graphics.DrawString(".", SMALL_FONT, bgBrush, new PointF(BigDigitsWidth, 9.5f));

            //Binary
            {
                int digitCount = LongView ? 3 : 2;
                int binaryWidth = (BigDigitsWidth / digitCount) + 2;
                int[] digitXs =
                {
                    6,
                    binaryWidth + 4,
                    2 * binaryWidth
                };

                drawBinaryDigit(fadedPrimaryBrush, new Rectangle(digitXs[0], 0, binaryWidth - 2, PANEL_HEIGHT), numDisplay[0]);
                drawBinaryDigit(fadedPrimaryBrush, new Rectangle(digitXs[1], 0, binaryWidth - 4, PANEL_HEIGHT), numDisplay[1]);

                if (LongView)
                    drawBinaryDigit(fadedPrimaryBrush, new Rectangle(digitXs[2], 0, binaryWidth - 2, PANEL_HEIGHT), numDisplay[2]);
            }

            //Draw number
            e.Graphics.DrawString(numDisplay, DEFAULT_FONT, primaryBrush, new Point(0, 0));

            e.Graphics.DrawString(offset.ToString(), SMALL_FONT, secondaryBrush, new Point(BigDigitsWidth, 5));
            e.Graphics.DrawString(unit.ToString(),  SMALL_FONT, secondaryBrush, new Point(BigDigitsWidth, PANEL_HEIGHT - 25));


            if (primarySeparatorPos == DecimalSeparatorPosition.HundredsTens || (primarySeparatorPos == DecimalSeparatorPosition.TensUnits && !LongView))
                e.Graphics.DrawString(".", DEFAULT_FONT, primaryBrush, new Point(19, 0));

            else if (primarySeparatorPos == DecimalSeparatorPosition.TensUnits)
                e.Graphics.DrawString(".", DEFAULT_FONT, primaryBrush, new Point(64, 0));

            if (secondarySeparatorPos == DecimalSeparatorPosition.TensUnits)
                e.Graphics.DrawString(".", SMALL_FONT, secondaryBrush, new PointF((LongView ? PANEL_LONG_WIDTH : PANEL_WIDTH) - 19, 9.5f));

            primaryBrush.Dispose();
            bgBrush.Dispose();
            fadedPrimaryBrush.Dispose();

            char getOffsetMarker(double d)
            {
                const char BLANK  = ' ';
                const char UP     = 'Δ';
                const char MIDDLE = 'Η';
                const char DOWN   = 'Ε';

                d %= 1.0;

                if (d <= 1.0 / 60.0)
                    return BLANK;

                if (d < 1.0 / 3.0)
                    return DOWN;

                if (d < 2.0 / 3.0)
                    return MIDDLE;

                return UP;
            }

            void getDisplaySettings(double input, bool allowDecimals, bool longView, out string output, out DecimalSeparatorPosition separatorPos, out char offsetOutput)
            {
                if (longView)
                {
                    if (input >= 1000)
                    {
                        output = "---";
                        separatorPos = DecimalSeparatorPosition.NoDecimalSeparator;
                        offsetOutput = ' ';
                    }
                    else if (input >= 100)
                    {
                        output = Math.Floor(input).ToString("000", CultureInfo.InvariantCulture);
                        separatorPos = DecimalSeparatorPosition.NoDecimalSeparator;
                        offsetOutput = getOffsetMarker(input);
                    }
                    else if (allowDecimals)
                    {
                        if (input >= 10)
                        {
                            //ToString was annoying with rounding instead of flooring, so doing this instead
                            int num = (int)(input * 10);

                            //Number is divisible by ten and should not have a decimal part
                            if (num % 10 == 0)
                            {
                                output = " " + (num / 10).ToString("00", CultureInfo.InvariantCulture);
                                separatorPos = DecimalSeparatorPosition.NoDecimalSeparator;
                            }

                            //Number needs a decimal part
                            else
                            {
                                output = num.ToString("000", CultureInfo.InvariantCulture);
                                separatorPos = DecimalSeparatorPosition.TensUnits;
                            }


                            offsetOutput = getOffsetMarker(input * 10);
                        }
                        else
                        {
                            //ToString was annoying with rounding instead of flooring, so doing this instead
                            int num = (int)(input * 100);

                            //Number is divisible by one hundred and should not have a decimal part
                            if (num % 100 == 0)
                            {
                                output = "  " + (num / 100).ToString("0", CultureInfo.InvariantCulture);
                                separatorPos = DecimalSeparatorPosition.NoDecimalSeparator;
                            }

                            //Number is divisible by ten and should have a decimal part
                            else if (num % 10 == 0)
                            {
                                output = " " + (num / 10).ToString("00", CultureInfo.InvariantCulture);
                                separatorPos = DecimalSeparatorPosition.TensUnits;
                            }

                            //Number needs a decimal part
                            else
                            {
                                output = num.ToString("000", CultureInfo.InvariantCulture);
                                separatorPos = DecimalSeparatorPosition.HundredsTens;
                            }

                            offsetOutput = getOffsetMarker(input * 100);
                        }
                        
                    }
                    else
                    {
                        output = Math.Floor(input).ToString("0", CultureInfo.InvariantCulture).PadLeft(3);
                        separatorPos = DecimalSeparatorPosition.NoDecimalSeparator;
                        offsetOutput = getOffsetMarker(input);
                    }
                }
                else
                {
                    if (input >= 100)
                    {
                        output = "--";
                        separatorPos = DecimalSeparatorPosition.NoDecimalSeparator;
                        offsetOutput = ' ';
                    }
                    else if (input >= 10)
                    {
                        output = Math.Floor(input).ToString("00", CultureInfo.InvariantCulture);
                        separatorPos = DecimalSeparatorPosition.NoDecimalSeparator;
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
                            separatorPos = DecimalSeparatorPosition.NoDecimalSeparator;
                        }

                        //Number needs a decimal part
                        else
                        {
                            output = num.ToString("00", CultureInfo.InvariantCulture);
                            separatorPos = DecimalSeparatorPosition.TensUnits;

                        }


                        offsetOutput = getOffsetMarker(input * 10);
                    }
                    else
                    {
                        output = " " + Math.Floor(input).ToString("0", CultureInfo.InvariantCulture);
                        separatorPos = DecimalSeparatorPosition.NoDecimalSeparator;
                        offsetOutput = getOffsetMarker(input);
                    }
                }
            }

            void drawBinaryDigit(Brush color, Rectangle area, char digit)
            {
                if (digit > '9' || digit < '0')
                    return;

                int digitNum = digit - '0';

                bool[] segments = Convert.ToString(digitNum, 2)
                    .PadLeft(4, '0')
                    .Select(i => i == '1')
                    .ToArray();

                const int MARGIN = 1;

                for (int i = 0; i < segments.Length; i++)
                {
                    if (!segments[i])
                        continue;

                    RectangleF fillArea = new RectangleF(
                    x: area.X,
                    y: area.Y + (float)i / segments.Length * area.Height,
                    width: area.Width,
                    height: 1f / segments.Length * area.Height
                );

                    fillArea.Inflate(-MARGIN, -MARGIN);

                    e.Graphics.FillRectangle(color, fillArea);
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
                int  hour = DateTime.Now.Hour;

                if (DateTime.Now.Second % 10 >= 5)
                {
                    hour %= 12;

                    if (hour == 0)
                        hour = 12;
                }

                //                                                        v Cheat to make offset display work
                CurrentCommand   = new MicroViewCommand(hour + DateTime.Now.Second / 60f, amPm, false);
                SecondaryCommand = new MicroViewCommand(DateTime.Now.Minute, amPm, false);

                BackColor = Globals.GlobalFreeModeBackColor;

                Invalidate();
                return;
            }

            /*
             * Primary timer is NOT in free mode
             */

            //Set commands
            CurrentCommand = Globals.PrimaryTimer.MicroViewUnit.Selector(Globals.PrimaryTimer.TimeLeft, LongView);

            if (!Globals.SecondaryTimer.InFreeMode)
                SecondaryCommand = Globals.SecondaryTimer.MicroViewUnit.Selector(Globals.SecondaryTimer.TimeLeft, false);

            else
                SecondaryCommand = MicroViewCommand.INVALID;

            //Blink when stopped at zero
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

            //Set background color
            if (isOvertime)
                BackColor = Globals.GlobalOvertimeColor;

            else
                BackColor = Globals.GlobalBackColor;

            //Broadcast milestones
            if (!Globals.PrimaryTimer.Paused)
            {
                broadcastAt("DAY ", "DAY ", 24,  0, false);
                broadcastAt("HOUR", "HOUR",  1,  0, false);
                broadcastAt("HALF", "HALF",  0, 30, false);
                broadcastAt("QUAT", "QUAT",  0, 15, false);
                broadcastAt("MIN ", "MIN",   0,  1, false);

                if (!Globals.PrimaryTimer.StopAtZero)
                    broadcastAt("ZERO", "ZERO", 0, 0, true);

                /*
                 * Broadcasts (to micro-view only) the provided message at the given amount of time on the clock
                 */
                void broadcastAt(string msg, string longMsg, int hours, int minutes, bool overtimeOnly)
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

                        Globals.Broadcast(null, msg, longMsg);
                    }
                }
            }

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

        public class MicroViewUnitSelector
        {
            public delegate MicroViewCommand MicroViewSelector(TimeSpan remainingTime, bool longView);

            /// <summary>
            /// The identifier of the unit selector
            /// </summary>
            public string ID { get; }

            /// <summary>
            /// Two-letter ID used by micro-view broadcasts
            /// </summary>
            public string ShortID { get; }

            /// <summary>
            /// The selector function that will generate commands when the micro-view is in short mode
            /// </summary>
            public MicroViewSelector Selector { get; }

            /// <summary>
            /// Holds all selectors
            /// </summary>
            public static readonly List<MicroViewUnitSelector> All = new List<MicroViewUnitSelector>();

            /// <summary>
            /// Creates a new micro-view unit selector
            /// </summary>
            /// <param name="id"></param>
            /// <param name="selector"></param>
            private MicroViewUnitSelector(string id, string shortId, MicroViewSelector selector)
            {
                ID = id;
                ShortID = shortId;
                Selector = selector;
                All.Add(this);
            }

            /// <summary>
            /// Displays time using the most accurate unit available. This is default
            /// </summary>
            public static readonly MicroViewUnitSelector MostAccurate = new MicroViewUnitSelector("default", "- ", (span, longView) =>
            {
                if (longView)
                {
                    if (span.TotalSeconds < 1000)
                        return new MicroViewCommand(span.TotalSeconds, ' ', false);

                    else if (span.TotalMinutes < 1000)
                        return new MicroViewCommand(span.TotalMinutes, 'M', true);

                    else if (span.TotalHours < 1000)
                        return new MicroViewCommand(span.TotalHours, 'H', true);

                    else
                        return new MicroViewCommand(span.TotalDays, 'D', true);
                }
                else
                {
                    if (span.TotalSeconds < 100)
                        return new MicroViewCommand(span.TotalSeconds, ' ', false);

                    else if (span.TotalMinutes < 100)
                        return new MicroViewCommand(span.TotalMinutes, 'M', true);

                    else if (span.TotalHours < 100)
                        return new MicroViewCommand(span.TotalHours, 'H', true);

                    else
                        return new MicroViewCommand(span.TotalDays, 'D', true);
                }
            });

            /// <summary>
            /// Displays time using days, hours, minutes and seconds. Switching units when the current unit drops below 1
            /// </summary>
            public static readonly MicroViewUnitSelector MostNatural = new MicroViewUnitSelector("natural", "N ", (span, longView) =>
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
            public static readonly MicroViewUnitSelector AlwaysSeconds = new MicroViewUnitSelector("seconds", "S ", (span, longView) =>
            {
                return new MicroViewCommand(span.TotalSeconds, ' ', true);
            });

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector AlwaysMinutes = new MicroViewUnitSelector("minutes", "M ", (span, longView) =>
            {
                return new MicroViewCommand(span.TotalMinutes, 'M', true);
            });

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector AlwaysHours = new MicroViewUnitSelector("hours", "H ", (span, longView) =>
            {
                return new MicroViewCommand(span.TotalHours, 'H', true);
            });

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector AlwaysDays = new MicroViewUnitSelector("days", "D ", (span, longView) =>
            {
                return new MicroViewCommand(span.TotalDays, 'D', true);
            });

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector MinimumMinutes = new MicroViewUnitSelector("min-minutes", "MM", (span, longView) =>
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
            public static readonly MicroViewUnitSelector MinimumHours = new MicroViewUnitSelector("min-hours", "MH", (span, longView) =>
            {
                if (span.TotalHours < 24)
                    return new MicroViewCommand(span.TotalHours, 'H', true);

                else
                    return new MicroViewCommand(span.TotalDays, 'D', true);
            });
        }

        /// <summary>
        /// Specifies the location to draw the decimal separator
        /// </summary>
        private enum DecimalSeparatorPosition
        {
            /// <summary>
            /// No separator should be displayed
            /// </summary>
            NoDecimalSeparator,

            /// <summary>
            /// Separator should be displayed between the hundreds' and tens' digits
            /// </summary>
            HundredsTens,

            /// <summary>
            /// Separator should be displayed between the tens' and units' digits
            /// </summary>
            TensUnits
        }
    }
}
