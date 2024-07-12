// #define USE_WRITTEN_NUMBERS_ON_BAR

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bars;

namespace NewTimer.FormParts
{
    /// <summary>
    /// A bar that displays the time left of the timer in the form of dynamic segments
    /// </summary>
    [DesignerCategory("")]
    public class TimerBar : SegmentedBar, ICountdown
    {
        /// <summary>
        /// The color that will fade in on the left side of the bar when a resolution switch is coming up
        /// </summary>
        private Color _leftSideFadeInColor = Color.Transparent;

        /// <summary>
        /// Gets the timer that the bar is rendering for
        /// </summary>
        private TimerConfig Timer
        {
            get
            {
                if (TrackSecondaryTimer)
                    return Globals.SecondaryTimer;

                return Globals.PrimaryTimer;
            }
        }

        private bool _trackSecondaryTimer;

        /// <summary>
        /// Gets or sets whether this timer bar will show the secondary timer
        /// </summary>
        public bool TrackSecondaryTimer
        {
            get => _trackSecondaryTimer;
            set
            {
                _trackSecondaryTimer = value;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="span"></param>
        /// <param name="isOvertime"></param>
        public void OnCountdownTick(TimeSpan span, TimeSpan secondSpan, bool isOvertime)
        {
            if (Timer == Globals.SecondaryTimer)
                span = secondSpan;

            if (TrackSecondaryTimer && Globals.SecondaryTimer.InFreeMode)
            {
                Visible = false;
                return;
            }
            else
            {
                Visible = true;
            }

            if (Timer.InFreeMode)
            {
                const int BASE_SCALE = 1200;

                SuspendLayout();
                FillColor = Timer.MicroViewColor; //Just cause it's the easiest thing to do

                int hour = DateTime.Now.Hour % 12;

                if (hour == 0)
                    hour = 12;

                Interval = (int)(1f / hour * BASE_SCALE);
                Value = DateTime.Now.Minute / 60f * BASE_SCALE;
                MaxValue = BASE_SCALE;
                BarMargin = 3;
                ResumeLayout();
            }
            else
            {
                /*
                 * Sets the base value, This is the segment(s) that "stand out" at the left side of the bar
                 */
                Value = GetNewValue(span);


                //Apply the correct bar settings for the current time left
                int settingIndex = Timer.BarSettings.TakeWhile(i => i.Key > span).Count();

                if (!Timer.Overtime)
                {
                    if (settingIndex + 1 >= Timer.BarSettings.Count)
                        _leftSideFadeInColor = Color.Transparent;
                    else
                        _leftSideFadeInColor = Timer.BarSettings.ElementAt(settingIndex + 1).Value.FillColor;
                }
                else
                {
                    _leftSideFadeInColor = Timer.BarSettings.ElementAt(settingIndex).Value.OverflowColor;
                }
                

                ApplySettings(Timer.BarSettings.ElementAt(settingIndex).Value);
            }
        }

        public static float GetNewValue(TimeSpan span)
        {
            if (span >= new TimeSpan(365, 0, 0, 0)) //Sets base to 1 year
            {
                return (float)span.TotalDays / 365f;
            }
            else if (span >= new TimeSpan(1, 0, 0, 0)) //Sets base to 1 day
            {
                return (float)span.TotalDays;
            }
            else if (span >= new TimeSpan(0, 1, 0, 0)) //Sets base to 1 hour
            {
                return (float)span.TotalHours;
            }
            else if (span >= new TimeSpan(0, 0, 1, 0)) //Sets base to 1 minute
            {
                return (float)span.TotalMinutes;
            }
            else //Sets base to 1 second
            {
                return (float)span.TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the background color of the control
        /// </summary>
        public override Color BackColor => Globals.GlobalBackColor;

        /// <summary>
        /// Gets the text color of the control
        /// </summary>
        public override Color ForeColor => Globals.GlobalBackColor;

        /// <summary>
        /// Gets the text that will be displayed on a given segment on the bar
        /// </summary>
        /// <param name="segmentValue"></param>
        /// <returns></returns>
        protected override string[] GetStringsForBarSegment(int segmentValue)
        {
            string splitAsLines = string.Join(Environment.NewLine, (IEnumerable<char>)segmentValue.ToString());

            if (Timer.InFreeMode)
                return new string[0];

            if (Timer.TimeLeft.TotalMinutes < 1)
            {
                return new[]
                {
#if USE_WRITTEN_NUMBERS_ON_BAR
                    createString(" second", " seconds", true),
                    createString(" sec", " sec", true),
                    createString("", "", true),
#endif

                    createString(" second", " seconds"),
                    createString("second", "seconds"),
                    createString(" sec"),
                    createString("sec"),
                    createString(" s"),
                    createString("s"), 
                    createString(""),
                    splitAsLines
                };
            }

            else if (Timer.TimeLeft.TotalHours < 1)
            {
                return new[] 
                {
#if USE_WRITTEN_NUMBERS_ON_BAR
                    createString(" minute", " minutes", true),
                    createString(" min", " min", true),
                    createString("", "", true),
#endif

                    createString(" minute", " minutes"), 
                    createString("minute", "minutes"), 
                    createString(" min"), 
                    createString("min"),
                    createString(" m"), 
                    createString("m"), 
                    createString(""),
                    splitAsLines
                };
            }

            else if (Timer.TimeLeft.TotalDays < 1)
            {
                return new[] 
                {
#if USE_WRITTEN_NUMBERS_ON_BAR
                    createString(" hour", " hours", true),
                    createString(" hr", " hr", true),
                    createString("", "", true),
#endif

                    createString(" hour", " hours"), 
                    createString("hour", "hours"), 
                    createString(" hr"), 
                    createString("hr"),
                    createString(" h"), 
                    createString("h"), 
                    createString(""),
                    splitAsLines
                };
            }
                
            else if (Timer.TimeLeft.TotalDays < 365)
            {
                return new[] 
                {
#if USE_WRITTEN_NUMBERS_ON_BAR
                    createString(" day", " days", true),
                    createString("", "", true),
#endif

                    createString(" day", " days"), 
                    createString("day", "days"), 
                    createString(" d"), 
                    createString("d"),
                    createString(""),
                    splitAsLines
                };
            }
                
            else
            {
                return new[] 
                {
#if USE_WRITTEN_NUMBERS_ON_BAR
                    createString(" year", " years", true),
                    createString(" yr", " yr", true),
                    createString("", "", true),
                    createString(""),
                    splitAsLines

#else
                    createString(" year", " years"), 
                    createString("year", "years"), 
                    createString(" yr"), 
                    createString("yr"), 
                    createString(" y"), 
                    createString("y"),
                    createString(""),
                    splitAsLines
#endif
                };
            }

            string createString(string suffixSingular, string suffixPlural = "", bool useWrittenNumbers = false)
            {
                if (string.IsNullOrEmpty(suffixPlural))
                    suffixPlural = suffixSingular;

                //Text is rendered Right-to-left. But that fucks up the order of words.
                //Strings with spaces become a problem unless we counteract the RTL with a LTR marker
                const char LTR_MARK = '\x200E';
                string number = segmentValue.ToString();

                if (useWrittenNumbers)
                    number = TaskbarUtility.NumberToWord(segmentValue, true);

                if (Math.Abs(segmentValue) != 1)
                    return number + (LTR_MARK + suffixPlural);

                return number + (LTR_MARK + suffixSingular);
            }
        }

        /// <summary>
        /// Gets the amount of subsegments that will be drawn given a timespan
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        private TimerBarResolutionInfo GetCustomTimerBarResolutionInfo(TimeSpan span)
        {
            /*
             * Table legend:
             * Column 1: Sub-segment count. This is how many sub-segments there are on the left side of the bar (Not necessarily a single segment
             * Column 2: Sub-segment count belonging to the next resolution: This is how many sub-segments on the current bar that will become real segments when the resolution switches (for fade-in)
             * Column 3: Left fade-out start value: This is the value the bar must have in overtime before the left side begins to fade away
             * Column 4: Left fade-out end value: This is the value the bar must have in overtime before the left side should have faded away completely
             */

            if (span.Days    >= 365) return new TimerBarResolutionInfo(12, 1, 999f, 999f); // Shows year
            if (span.Days    >=  50) return new TimerBarResolutionInfo( 4, 1, 300f, 365f); // v (Sub-segment count adjustments)
            if (span.Days    >=  30) return new TimerBarResolutionInfo(30, 7, 300f, 365f); // Shows months (ish)
            if (span.Days    >=   7) return new TimerBarResolutionInfo( 7, 1,  25f,  30f); // Shows weeks
            if (span.Days    >=   4) return new TimerBarResolutionInfo( 6, 1,   6f,   7f); // v (Sub-segment count adjustments)
            if (span.Days    >=   2) return new TimerBarResolutionInfo(12, 1,   6f,   7f); // v (Sub-segment count adjustments)
            if (span.Days    >=   1) return new TimerBarResolutionInfo(24, 1,   6f,   7f); // Shows days
            if (span.Hours   >=   1) return new TimerBarResolutionInfo( 4, 2,  23f,  24f); // Shows hours
            if (span.Minutes >=  30) return new TimerBarResolutionInfo( 6, 2,  50f,  60f); // Shows 15-minutes
            if (span.Minutes >=  10) return new TimerBarResolutionInfo(10, 1,  25f,  30f); // Shows 5-minutes
            if (span.Minutes >=   1) return new TimerBarResolutionInfo( 6, 1,   9f,  10f); // Shows minutes
            if (span.Seconds >=  10) return new TimerBarResolutionInfo(10, 1,  45f,  60f); // Shows 10-seconds
            else                     return new TimerBarResolutionInfo( 0, 0,   5f,  10f); // Shows seconds
        }

        /// <summary>
        /// Gets a new set of colors from the color scheme when the bar is clicked
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Timer.Recolorize();
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="e"></param>
        protected override void DrawOnBar(PaintEventArgs e)
        {
            /*
             * Set hatch state
             */
            if (Timer.InFreeMode && !DrawHatched)
                DrawHatchedOverflow = DrawHatched = true;

            else if (!Timer.InFreeMode && DrawHatched)
                DrawHatchedOverflow = DrawHatched = false;

            //Do nothing in free mode
            if (Timer.InFreeMode)
                return;

            //Do nothing else if the value of the bar is zero. To prevent DIV/0
            if (Value == 0)
                return;

            //Custom information about the current resolution data
            TimerBarResolutionInfo resolutionInfo = GetCustomTimerBarResolutionInfo(Timer.TimeLeft);

            //This is the part of the bar that we should draw to (It excludes margins)
            Rectangle clientArea = new Rectangle(BarMargin, BarMargin, Width - (2 * BarMargin), Height - (2 * BarMargin));

            //Calculate the area to draw the subsegments in
            float scaleOfLeftSide = MaxValue / Value;
            float widthOfLeftSide = scaleOfLeftSide * clientArea.Width;

            //Do nothing if there is less than one second left of the timer (overflow is higher that the width of the control)
            if (widthOfLeftSide >= e.ClipRectangle.Width)
                return;

            /*
             * Left side fade-in
             */

            if (!Timer.Overtime)
            {
                //0-255
                int   leftSideFadeInOpacity = (int)LerpClamped(0x00, 0xFF, (scaleOfLeftSide - 0.75f) * 4f);
                Color leftSideFadeInColor   = Color.FromArgb(leftSideFadeInOpacity, _leftSideFadeInColor);

                using (SolidBrush leftSideFadeInBrush = new SolidBrush(leftSideFadeInColor))
                {
                    //Draw the left side fade-in animation when about to switch resolutions
                    float leftSideFadeInWidth = (float)resolutionInfo.SubSegmentCountBelongingToNextResolution / resolutionInfo.SubSegmentsAtLeftSide * widthOfLeftSide;

                    e.Graphics.FillRectangle(
                        leftSideFadeInBrush,
                        new RectangleF(
                            clientArea.Left,
                            clientArea.Top,
                            leftSideFadeInWidth,
                            clientArea.Height
                        )
                    );
                }
            }
            else 
            {

                int   leftSideFadeOutOpacity = (int)LerpClamped(0x00, 0xFF, InverseLerp(resolutionInfo.LeftFadeOutStartValue, resolutionInfo.LeftFadeOutEndValue, Value));
                Color leftSideFadeOutColor   = Color.FromArgb(leftSideFadeOutOpacity, _leftSideFadeInColor);

                using (SolidBrush leftSideFadeOutBrush = new SolidBrush(leftSideFadeOutColor))
                {
                    e.Graphics.FillRectangle(
                        leftSideFadeOutBrush,
                        new RectangleF(
                            clientArea.Left,
                            clientArea.Top,
                            widthOfLeftSide,
                            clientArea.Height
                        )
                    );
                }
            }

            /*
             * Subsegments
             */

            //Set pen's properties are based on how big the subsegment area is
            float subSegmentPenSize    = Lerp(1f, 4f, (scaleOfLeftSide - 0.75f) * 4f);
            float subSegmentPenOpacity = widthOfLeftSide / clientArea.Width; //0-1
            Color subSegmentPenColor   = Color.FromArgb((int)(subSegmentPenOpacity * 0x8F), Color.White);

            float subSegmentMarginScale = Lerp(0.04f, 0f, (scaleOfLeftSide - 0.5f) * 2f);
            float subSegmentMargin      = clientArea.Height * subSegmentMarginScale;

            using (Pen subSegmentPen = new Pen(subSegmentPenColor, subSegmentPenSize))
            {
                //Nothing to draw
                if (resolutionInfo.SubSegmentsAtLeftSide == 0)
                    return;

                //Width of each sub-segment
                float widthPerSubSegement = widthOfLeftSide / resolutionInfo.SubSegmentsAtLeftSide;

                //The total amount of sub-segments
                int visibleSubSegmentCount = (int)(clientArea.Width / widthPerSubSegement);

                //Stop after 50 to avoid too much strain
                if (visibleSubSegmentCount > 50)
                    return;

                for (int i = 1; i <= visibleSubSegmentCount; i++)
                {
                    float translatedXOffset = (float)i / resolutionInfo.SubSegmentsAtLeftSide * widthOfLeftSide;

                    e.Graphics.DrawLine(
                        pen: subSegmentPen,
                        x1: clientArea.Left   + translatedXOffset,
                        y1: clientArea.Top    + subSegmentMargin,

                        x2: clientArea.Left   + translatedXOffset,
                        y2: clientArea.Bottom - subSegmentMargin
                    );
                }

            }
        }

        /// <summary>
        /// Performs linear interpolation between a and b by t amount
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Lerp(float a, float b, float t) => a * (1 - t) + b * t;

        /// <summary>
        /// Performs linear interpolation between a and b by t amount, where t is clamped between 0 and 1
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float LerpClamped(float a, float b, float t) => Lerp(a, b, Math.Min(1f, Math.Max(0f, t)));

        public static float InverseLerp(float a, float b, float value) => (value - a) / (b - a);

        private struct TimerBarResolutionInfo
        {
            /// <summary>
            /// Gets the total amount of sub-segments to draw per segment in the current resolution
            /// </summary>
            public int SubSegmentsAtLeftSide { get; }

            /// <summary>
            /// Gets the total amount of sub-segments that will eventually become real segments 
            /// when a descending timer reaches the next threshold to switch resolution
            /// </summary>
            public int SubSegmentCountBelongingToNextResolution { get; }

            /// <summary>
            /// The actual value on the bar where the left fade-out in overtime will start happening
            /// </summary>
            public float LeftFadeOutStartValue { get; }

            /// <summary>
            /// The actual value on the bar where the left fade-out in overtime will finish
            /// </summary>
            public float LeftFadeOutEndValue { get; }

            /// <summary>
            /// Creates a new sub-segment info object
            /// </summary>
            /// <param name="subSegmentCount"></param>
            /// <param name="subSegmentCountBelongingToNextResolution"></param>
            public TimerBarResolutionInfo(int subSegmentCount, int subSegmentCountBelongingToNextResolution, float leftFadeOutStartValue, float leftFadeOutEndValue)
            {
                SubSegmentsAtLeftSide = subSegmentCount;
                SubSegmentCountBelongingToNextResolution = subSegmentCountBelongingToNextResolution;
                LeftFadeOutStartValue = leftFadeOutStartValue;
                LeftFadeOutEndValue = leftFadeOutEndValue;
            }
        }
    }
}
