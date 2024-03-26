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
        /// Gets whether the micro view is currently displaying the secondary timer
        /// </summary>
        public bool CurrentlyShowingSecondaryTimer
        {
            get
            {
                return !Globals.SecondaryTimer.InFreeMode && DateTime.Now.Second % 10 < 5;
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

            //Set background colors
            if (Globals.PrimaryTimer.InFreeMode)
                BackColor = Globals.GlobalFreeModeBackColor;

            else if (Globals.PrimaryTimer.Overtime)
                BackColor = Globals.GlobalOvertimeColor;

            else
                BackColor = Globals.GlobalBackColor;

            //Broadcast milestones
            if (!Globals.PrimaryTimer.Paused)
            {
                BroadcastMilestones(Globals.PrimaryTimer);
            }

            //Get secondary timer brush if applicable
            if (!Globals.SecondaryTimer.InFreeMode && Globals.CurrentMicroBroadcastMessage == null)
                secondaryBrush = CreateSecondaryTimerFadeBrush();

            else
                secondaryBrush = new SolidBrush(Globals.PrimaryTimer.MicroViewColor);

            /*
             * Render
             */

            MicroViewCommand command = CreateCommand();

            RenderBackgrounds(e.Graphics, bgBrush);
            RenderBinaryBackground(command, e.Graphics, fadedPrimaryBrush);
            RenderCommand(command, e.Graphics, primaryBrush, secondaryBrush);

            primaryBrush.Dispose();
            bgBrush.Dispose();
            secondaryBrush.Dispose();
            fadedPrimaryBrush.Dispose();
        }

        /// <summary>
        /// Draws the number backgrounds on the micro-view area
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="backgroundBrush"></param>
        private void RenderBackgrounds(Graphics graphics, Brush backgroundBrush)
        {
            const char OFFSET_BACKGROUND  = 'Η';
            const char DEFAULT_BACKGROUND = '@';
            const char ANALOG_BACKGROUND  = 'ν';

            char primaryBackgroundSymbol = Globals.PrimaryTimer.MicroViewUnit == MicroViewUnitSelector.Analog 
                ? ANALOG_BACKGROUND 
                : DEFAULT_BACKGROUND;

            //Get the backgrounds for the right section
            char offsetBackgroundSymbol;
            char unitBackgroundSymbol;

            if (CurrentlyShowingSecondaryTimer) 
            {
                //Analog secondary timer
                if (Globals.SecondaryTimer.MicroViewUnit == MicroViewUnitSelector.Analog)
                {
                    offsetBackgroundSymbol = ANALOG_BACKGROUND;
                    unitBackgroundSymbol   = ANALOG_BACKGROUND;
                }

                //Digital secondary timer
                else
                {
                    offsetBackgroundSymbol = DEFAULT_BACKGROUND;
                    unitBackgroundSymbol   = DEFAULT_BACKGROUND;
                }
            }
            else
            {
                //Analog (with number as offset) or in free mode since they use the same backgrounds
                if (Globals.PrimaryTimer.MicroViewUnit == MicroViewUnitSelector.Analog || Globals.PrimaryTimer.InFreeMode)
                {
                    offsetBackgroundSymbol = DEFAULT_BACKGROUND;
                }

                //Digital (with arrow as offset)
                else
                {
                    offsetBackgroundSymbol = OFFSET_BACKGROUND;
                }

                unitBackgroundSymbol = DEFAULT_BACKGROUND;
            }

            //Text background
            graphics.DrawString(new string(primaryBackgroundSymbol, LongView ? 3 : 2), DEFAULT_FONT, backgroundBrush, new Point(0, 0));

            //Offset background
            graphics.DrawString(offsetBackgroundSymbol.ToString(), SMALL_FONT, backgroundBrush, new Point(BigDigitsWidth + 2, 5));

            //Unit background
            graphics.DrawString(unitBackgroundSymbol.ToString(), SMALL_FONT, backgroundBrush, new Point(BigDigitsWidth, PANEL_HEIGHT - 25));

            //Left Dot Background
            graphics.DrawString(".", DEFAULT_FONT, backgroundBrush, new Point(19, 0));

            //Right dot background
            if (LongView)
                graphics.DrawString(".", DEFAULT_FONT, backgroundBrush, new Point(64, 0));

            //Secondary dot background
            graphics.DrawString(".", SMALL_FONT, backgroundBrush, new PointF(BigDigitsWidth, 9.5f));
        }

        /// <summary>
        /// Draws the binary digits on the micro-view area
        /// </summary>
        /// <param name="command"></param>
        /// <param name="graphics"></param>
        /// <param name="brush"></param>
        private void RenderBinaryBackground(MicroViewCommand command, Graphics graphics, Brush brush)
        {
            int digitCount = LongView ? 3 : 2;
            int binaryWidth = (BigDigitsWidth / digitCount) + 2;
            int[] digitXs = { 6, binaryWidth + 4, 2 * binaryWidth };

            drawBinaryDigit(brush, new Rectangle(digitXs[0], 0, binaryWidth - 2, PANEL_HEIGHT), command.MainText[0]);
            drawBinaryDigit(brush, new Rectangle(digitXs[1], 0, binaryWidth - 4, PANEL_HEIGHT), command.MainText[1]);

            if (LongView)
                drawBinaryDigit(brush, new Rectangle(digitXs[2], 0, binaryWidth - 2, PANEL_HEIGHT), command.MainText[2]);

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

                    graphics.FillRectangle(color, fillArea);
                }
            }
        }

        /// <summary>
        /// Draws the text on the micro-view area
        /// </summary>
        /// <param name="command"></param>
        /// <param name="graphics"></param>
        /// <param name="foregroundBrush"></param>
        /// <param name="secondaryForegroundBrush"></param>
        private void RenderCommand(MicroViewCommand command, Graphics graphics, Brush foregroundBrush, Brush secondaryForegroundBrush)
        {
            //Draw number, offset and unit
            graphics.DrawString(command.MainText, DEFAULT_FONT, foregroundBrush, new Point(0, 0));
            graphics.DrawString(command.Offset.ToString(), SMALL_FONT, secondaryForegroundBrush, new Point(BigDigitsWidth + 2, 5));
            graphics.DrawString(command.Unit.ToString(), SMALL_FONT, secondaryForegroundBrush, new Point(BigDigitsWidth, PANEL_HEIGHT - 25));

            //Draw decimal separators
            if (command.SeparatorPosition == DecimalSeparatorPosition.HundredsTens || (command.SeparatorPosition == DecimalSeparatorPosition.TensUnits && !LongView))
                graphics.DrawString(".", DEFAULT_FONT, foregroundBrush, new Point(19, 0));

            else if (command.SeparatorPosition == DecimalSeparatorPosition.TensUnits)
                graphics.DrawString(".", DEFAULT_FONT, foregroundBrush, new Point(64, 0));

            if (command.ShowSecondaryDecimalSeparator)
                graphics.DrawString(".", SMALL_FONT, secondaryForegroundBrush, new PointF((LongView ? PANEL_LONG_WIDTH : PANEL_WIDTH) - 19, 9.5f));
        }
        
        /// <summary>
        /// Create the MicroViewCommand for the current configuration
        /// </summary>
        /// <returns></returns>
        private MicroViewCommand CreateCommand()
        {
            if (Globals.CurrentMicroBroadcastMessage != null)
            {
                if (LongView)
                    return new MicroViewCommand(Globals.CurrentLongMicroBroadcastMessage, LongView);

                else
                    return new MicroViewCommand(Globals.CurrentMicroBroadcastMessage, false);
            }
            else if (Globals.PrimaryTimer.InFreeMode)
            {
                MicroViewCommand command;

                //Hack to get analog support for free-mode
                if (Globals.PrimaryTimer.MicroViewUnit == MicroViewUnitSelector.Analog)
                {
                    command = new MicroViewCommand(
                        mainText: MicroViewUnitSelector.GetAnalogHandPosition(DateTime.Now.Hour) 
                            + MicroViewUnitSelector.GetAnalogHandPosition(DateTime.Now.Minute / 5).ToString(),

                        offset: (DateTime.Now.Minute % 5).ToString()[0],

                        decimalSeparator: DateTime.Now.Hour >= 12 
                            ? DecimalSeparatorPosition.TensUnits 
                            : DecimalSeparatorPosition.NoDecimalSeparator,

                        unit: ' ',
                        showSecondaryDecimalSeparator: false,
                        longView: LongView
                    );
                }
                else
                {
                    string minute = DateTime.Now.Minute.ToString("00");

                    command = new MicroViewCommand(
                        mainText: DateTime.Now.Hour.ToString(),
                        offset: minute[0],
                        unit: minute[1],
                        decimalSeparator: DecimalSeparatorPosition.NoDecimalSeparator,
                        showSecondaryDecimalSeparator: false,
                        longView: LongView
                    );
                }

                return applySecondaryTimer(command);
            }
            else if (Globals.PrimaryTimer.StopAtZero && Globals.PrimaryTimer.Overtime && Globals.PrimaryTimer.RealTimeLeft.Milliseconds < -500)
            {
                return new MicroViewCommand("", false);
            }
            else
            {
                MicroViewCommand command = Globals.PrimaryTimer.MicroViewUnit.Selector(Globals.PrimaryTimer.TimeLeft, LongView);
                return applySecondaryTimer(command);
            }

            MicroViewCommand applySecondaryTimer(MicroViewCommand source)
            {

                //Override to show the secondary timer
                if (CurrentlyShowingSecondaryTimer)
                {
                    MicroViewCommand secondaryCommand = Globals.SecondaryTimer.MicroViewUnit.Selector(Globals.SecondaryTimer.TimeLeft, false);
                    string secondaryText = secondaryCommand.MainText;

                    //Override the unit and offset of the primary command with the main text of the secondary command
                    return new MicroViewCommand(
                        offset: secondaryText[0],
                        unit: secondaryText[1],
                        showSecondaryDecimalSeparator: secondaryCommand.SeparatorPosition != DecimalSeparatorPosition.NoDecimalSeparator,

                        mainText: source.MainText,
                        decimalSeparator: source.SeparatorPosition,
                        longView: LongView
                    );
                }

                return source;
            }
        }

        /// <summary>
        /// Creates milestone broadcasts when applicable
        /// </summary>
        /// <param name="timer"></param>
        private static void BroadcastMilestones(TimerConfig timer)
        {
            broadcastAt("DAY ", "DAY ", 24, 0, false);
            broadcastAt("HOUR", "HOUR",  1, 0, false);
            broadcastAt("HALF", "HALF",  0, 30, false);
            broadcastAt("QUAT", "QUAT",  0, 15, false);
            broadcastAt("MIN ", "MIN",   0, 1, false);

            if (!timer.StopAtZero)
                broadcastAt("ZERO", "ZERO", 0, 0, true);

            /*
             * Broadcasts (to micro-view only) the provided message at the given amount of time on the clock
             */
            void broadcastAt(string msg, string longMsg, int hours, int minutes, bool overtimeOnly)
            {
                //Do not override "real" broadcasts
                if (Globals.CurrentBroadcastMessage != null)
                    return;

                if (!timer.Overtime && overtimeOnly)
                    return;

                if (timer.TimeLeft.Days != 0)
                    return;

                TimeSpan target = new TimeSpan(hours, minutes, 0);

                if (!timer.Overtime)
                    target -= new TimeSpan(0, 0, 1);

                if (timer.TimeLeft.Days       == target.Days 
                    && timer.TimeLeft.Hours   == target.Hours 
                    && timer.TimeLeft.Minutes == target.Minutes 
                    && timer.TimeLeft.Seconds == target.Seconds)
                {
                    if (timer.Overtime && timer.TimeLeft.Milliseconds > 100)
                        return;

                    if (!timer.Overtime && timer.TimeLeft.Milliseconds < 900)
                        return;

                    Globals.Broadcast(null, msg, longMsg);
                }
            }
        }
        
        /// <summary>
        /// Creates the brush that will be used to fade between primary and secondary colors for the secondary timer
        /// </summary>
        /// <returns></returns>
        private Brush CreateSecondaryTimerFadeBrush()
        {
            Brush secondaryBrush;
            Color baseColor;

            if (CurrentlyShowingSecondaryTimer)
                baseColor = Globals.SecondaryTimer.MicroViewColor;

            else
                baseColor = Globals.PrimaryTimer.MicroViewColor;

            if (DateTime.Now.Second % 5 == 4)
                secondaryBrush = new SolidBrush(Color.FromArgb((int)((1000 - DateTime.Now.Millisecond) / 1000f * 255), baseColor));

            else if (DateTime.Now.Second % 5 == 0)
                secondaryBrush = new SolidBrush(Color.FromArgb((int)(DateTime.Now.Millisecond / 1000f * 255), baseColor));

            else
                secondaryBrush = new SolidBrush(baseColor);
            return secondaryBrush;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="span"></param>
        /// <param name="secondarySpan"></param>
        /// <param name="isOvertime"></param>
        public void OnCountdownTick(TimeSpan span, TimeSpan secondarySpan, bool isOvertime)
        {
            Invalidate();
        }

        /// <summary>
        /// Represents settings for how the control will display content
        /// </summary>
        public struct MicroViewCommand
        {
            /// <summary>
            /// Gets the number to display.
            /// </summary>
            public string MainText { get; }

            /// <summary>
            /// Gets the offset marker to use
            /// </summary>
            public char Offset { get; }

            /// <summary>
            /// Gets the character to display in the unit section. This field is unused for secondary commands
            /// </summary>
            public char Unit { get; }

            /// <summary>
            /// Gets the position to display the decimal separator
            /// </summary>
            public DecimalSeparatorPosition SeparatorPosition { get; }

            /// <summary>
            /// Gets whether the decimal separator between the offset and unit indicator should be lit
            /// </summary>
            public bool ShowSecondaryDecimalSeparator { get; }

            /// <summary>
            /// Creates a new command
            /// </summary>
            /// <param name="number"></param>
            /// <param name="unit"></param>
            /// <param name="allowDecimals"></param>
            public MicroViewCommand(string mainText, char offset, char unit, DecimalSeparatorPosition decimalSeparator, bool showSecondaryDecimalSeparator, bool longView)
            {
                int digitCount = longView ? 3 : 2;

                MainText = mainText.PadLeft(digitCount).Substring(0, digitCount);
                Offset = offset;
                Unit = unit;
                SeparatorPosition = decimalSeparator;
                ShowSecondaryDecimalSeparator = showSecondaryDecimalSeparator;
            }

            /// <summary>
            /// Automatically assembles a micro-view command from a broadcast message
            /// </summary>
            /// <param name="broadcastText"></param>
            /// <param name="longView"></param>
            public MicroViewCommand(string broadcastText, bool longView)
            {
                int digitCount = longView ? 3 : 2;
                int maxLength  = digitCount + 2;

                broadcastText = broadcastText.PadRight(maxLength);

                MainText = broadcastText.Substring(0, digitCount);
                Offset   = broadcastText[digitCount];
                Unit     = broadcastText[digitCount + 1];

                SeparatorPosition = DecimalSeparatorPosition.NoDecimalSeparator;
                ShowSecondaryDecimalSeparator = false;
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
                        return FromSeconds(span, longView);

                    else if (span.TotalMinutes < 1000)
                        return FromMinutes(span, longView);

                    else if (span.TotalHours < 1000)
                        return FromHours(span, longView);

                    else
                        return FromDays(span, longView);
                }
                else
                {
                    if (span.TotalSeconds < 100)
                        return FromSeconds(span, longView);

                    else if (span.TotalMinutes < 100)
                        return FromMinutes(span, longView);

                    else if (span.TotalHours < 100)
                        return FromHours(span, longView);

                    else
                        return FromDays(span, longView);
                }
            });

            /// <summary>
            /// Displays time using days, hours, minutes and seconds. Switching units when the current unit drops below 1
            /// </summary>
            public static readonly MicroViewUnitSelector MostNatural = new MicroViewUnitSelector("natural", "N ", (span, longView) =>
            {
                if (span.TotalSeconds < 60)
                    return FromSeconds(span, longView);

                else if (span.TotalMinutes < 60)
                    return FromMinutes(span, longView);

                else if (span.TotalHours < 24)
                    return FromHours(span, longView);

                else
                    return FromDays(span, longView);
            });

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector AlwaysSeconds = new MicroViewUnitSelector("seconds", "S ", FromSeconds);

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector AlwaysMinutes = new MicroViewUnitSelector("minutes", "M ", FromMinutes);

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector AlwaysHours = new MicroViewUnitSelector("hours", "H ", FromHours);

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector AlwaysDays = new MicroViewUnitSelector("days", "D ", FromDays);

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector MinimumMinutes = new MicroViewUnitSelector("min-minutes", "MM", (span, longView) =>
            {
                if (span.TotalMinutes < 60)
                    return FromMinutes(span, longView);

                else if (span.TotalHours < 24)
                    return FromHours(span, longView);

                else
                    return FromDays(span, longView);
            });

            /// <summary>
            /// Always displays time using seconds
            /// </summary>
            public static readonly MicroViewUnitSelector MinimumHours = new MicroViewUnitSelector("min-hours", "MH", (span, longView) =>
            {
                if (span.TotalHours < 24)
                    return FromHours(span, longView);

                else
                    return FromDays(span, longView);
            });

            /// <summary>
            /// Displays a semi-analog interface
            /// </summary>
            public static readonly MicroViewUnitSelector Analog = new MicroViewUnitSelector("analog", "A ", (span, longView) => 
            {
                int primaryValue;
                int secondaryValue;
                bool showDot;
                int offset;
                char unit;

                if (span.TotalMinutes < 1)
                {
                    primaryValue = span.Seconds / 5;
                    secondaryValue = (int)(span.Milliseconds / 1000f * 12);
                    offset = span.Seconds % 5;
                    showDot = false;
                    unit = ' ';
                }
                else if (span.TotalHours < 1)
                {
                    primaryValue = span.Minutes / 5;
                    secondaryValue = span.Seconds / 5;
                    offset = span.Minutes % 5;
                    showDot = false;
                    unit = 'M';
                }
                else if (span.TotalHours <= 24)
                {
                    primaryValue = span.Hours % 12;
                    secondaryValue = span.Minutes / 5;
                    offset = span.Minutes % 5;
                    showDot = span.Hours >= 12;
                    unit = 'H';
                }
                else if (span.TotalDays <= 12)
                {
                    primaryValue = span.Days;
                    secondaryValue = span.Hours % 12;
                    offset = (int)(span.TotalHours % 1 * 10);
                    showDot = span.Hours >= 12;
                    unit = 'D';
                }
                else
                {
                    return new MicroViewCommand("--", ' ', ' ', DecimalSeparatorPosition.NoDecimalSeparator, false, longView);
                }

                return new MicroViewCommand(
                    mainText: GetAnalogHandPosition(primaryValue).ToString() + GetAnalogHandPosition(secondaryValue),
                    offset: offset.ToString()[0],
                    unit:   unit,
                    decimalSeparator: showDot ? DecimalSeparatorPosition.TensUnits : DecimalSeparatorPosition.NoDecimalSeparator,
                    showSecondaryDecimalSeparator: false,
                    longView: longView
                );
            });

            /// <summary>
            /// Creates an analog hand position symbol from the provided hour number
            /// </summary>
            /// <param name="hour"></param>
            /// <returns></returns>
            public static char GetAnalogHandPosition(int hour)
            {
                const char TWELVE_POSITION = 'α';
                return (char)((TWELVE_POSITION + hour % 12));
            }
        }

        /// <summary>
        /// Creates a micro-view command based on the amount of seconds in a time-span
        /// </summary>
        /// <param name="span"></param>
        /// <param name="longView"></param>
        /// <returns></returns>
        private static MicroViewCommand FromSeconds(TimeSpan span, bool longView)
        {
            return FromNumber(span.TotalSeconds, false, ' ', longView);
        }

        /// <summary>
        /// Creates a micro-view command based on the amount of minutes in a time-span
        /// </summary>
        /// <param name="span"></param>
        /// <param name="longView"></param>
        /// <returns></returns>
        private static MicroViewCommand FromMinutes(TimeSpan span, bool longView)
        {
            return FromNumber(span.TotalMinutes, true, 'M', longView);
        }

        /// <summary>
        /// Creates a micro-view command based on the amount of hours in a time-span
        /// </summary>
        /// <param name="span"></param>
        /// <param name="longView"></param>
        /// <returns></returns>
        private static MicroViewCommand FromHours(TimeSpan span, bool longView)
        {
            return FromNumber(span.TotalHours, true, 'H', longView);
        }

        /// <summary>
        /// Creates a micro-view command based on the amount of days in a time-span
        /// </summary>
        /// <param name="span"></param>
        /// <param name="longView"></param>
        /// <returns></returns>
        private static MicroViewCommand FromDays(TimeSpan span, bool longView)
        {
            return FromNumber(span.TotalDays, true, 'D', longView);
        }

        /// <summary>
        /// Creates a micro-view command dynamically based on the provided input value
        /// </summary>
        /// <param name="input"></param>
        /// <param name="allowDecimals"></param>
        /// <param name="unit"></param>
        /// <param name="longView"></param>
        /// <returns></returns>
        private static MicroViewCommand FromNumber(double input, bool allowDecimals, char unit, bool longView)
        {
            string mainText;
            DecimalSeparatorPosition separator;
            char offset;

            if (longView)
            {
                if (input >= 1000)
                {
                    mainText = "---";
                    separator = DecimalSeparatorPosition.NoDecimalSeparator;
                    offset = ' ';
                }
                else if (input >= 100)
                {
                    mainText = Math.Floor(input).ToString("000", CultureInfo.InvariantCulture);
                    separator = DecimalSeparatorPosition.NoDecimalSeparator;
                    offset = GetOffsetMarker(input);
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
                            mainText = " " + (num / 10).ToString("00", CultureInfo.InvariantCulture);
                            separator = DecimalSeparatorPosition.NoDecimalSeparator;
                        }

                        //Number needs a decimal part
                        else
                        {
                            mainText = num.ToString("000", CultureInfo.InvariantCulture);
                            separator = DecimalSeparatorPosition.TensUnits;
                        }


                        offset = GetOffsetMarker(input * 10);
                    }
                    else
                    {
                        //ToString was annoying with rounding instead of flooring, so doing this instead
                        int num = (int)(input * 100);

                        //Number is divisible by one hundred and should not have a decimal part
                        if (num % 100 == 0)
                        {
                            mainText = "  " + (num / 100).ToString("0", CultureInfo.InvariantCulture);
                            separator = DecimalSeparatorPosition.NoDecimalSeparator;
                        }

                        //Number is divisible by ten and should have a decimal part
                        else if (num % 10 == 0)
                        {
                            mainText = " " + (num / 10).ToString("00", CultureInfo.InvariantCulture);
                            separator = DecimalSeparatorPosition.TensUnits;
                        }

                        //Number needs a decimal part
                        else
                        {
                            mainText = num.ToString("000", CultureInfo.InvariantCulture);
                            separator = DecimalSeparatorPosition.HundredsTens;
                        }

                        offset = GetOffsetMarker(input * 100);
                    }
                }
                else
                {
                    mainText = Math.Floor(input).ToString("0", CultureInfo.InvariantCulture);
                    separator = DecimalSeparatorPosition.NoDecimalSeparator;
                    offset = GetOffsetMarker(input);
                }
            }
            else
            {
                if (input >= 100)
                {
                    mainText = "--";
                    separator = DecimalSeparatorPosition.NoDecimalSeparator;
                    offset = ' ';
                }
                else if (input >= 10)
                {
                    mainText = Math.Floor(input).ToString("00", CultureInfo.InvariantCulture);
                    separator = DecimalSeparatorPosition.NoDecimalSeparator;
                    offset = GetOffsetMarker(input);
                }
                else if (allowDecimals)
                {
                    //ToString was annoying with rounding instead of flooring, so doing this instead
                    int num = (int)(input * 10);

                    //Number is divisible by ten and should not have a decimal part
                    if (num % 10 == 0)
                    {
                        mainText = " " + (num / 10).ToString("0", CultureInfo.InvariantCulture);
                        separator = DecimalSeparatorPosition.NoDecimalSeparator;
                    }

                    //Number needs a decimal part
                    else
                    {
                        mainText = num.ToString("00", CultureInfo.InvariantCulture);
                        separator = DecimalSeparatorPosition.TensUnits;

                    }


                    offset = GetOffsetMarker(input * 10);
                }
                else
                {
                    mainText = " " + Math.Floor(input).ToString("0", CultureInfo.InvariantCulture);
                    separator = DecimalSeparatorPosition.NoDecimalSeparator;
                    offset = GetOffsetMarker(input);
                }
            }

            return new MicroViewCommand(mainText, offset, unit, separator, false, longView);
        }

        /// <summary>
        /// Gets an offset marker for the provided number's decimal section
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static char GetOffsetMarker(double d)
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

        /// <summary>
        /// Specifies the location to draw the decimal separator
        /// </summary>
        public enum DecimalSeparatorPosition
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
