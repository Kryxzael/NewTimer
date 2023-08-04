using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

        /// <summary>
        /// Gets the "time left" that is displayed at the bar. This is exceptional in free mode
        /// </summary>
        private TimeSpan DisplayedTimeLeft
        {
            get
            {
                if (Timer.InFreeMode)
                {
                    if (DateTime.Now.Minute >= 30)
                        return DateTime.Today.AddHours(DateTime.Now.Hour + 1) - DateTime.Now;

                    return DateTime.Now - DateTime.Today.AddHours(DateTime.Now.Hour);
                }

                return Timer.TimeLeft;
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

            //Sorry about this but
            if (Timer.InFreeMode)
                span = DisplayedTimeLeft;

            /*
             * Sets the base value, This is the segment(s) that "stand out" at the left side of the bar
             */
            Value = GetNewValue(span);


            //Apply the correct bar settings for the current time left
            ApplySettings(Timer.BarSettings.First(i => i.Key <= span).Value);
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

            if (DisplayedTimeLeft.TotalMinutes < 1)
            {
                return new[]
                {
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

            else if (DisplayedTimeLeft.TotalHours < 1)
            {
                return new[] 
                { 
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

            else if (DisplayedTimeLeft.TotalDays < 1)
            {
                return new[] 
                { 
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
                
            else if (DisplayedTimeLeft.TotalDays < 365)
            {
                return new[] 
                { 
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
                    createString(" year", " years"), 
                    createString("year", "years"), 
                    createString(" yr"), 
                    createString("yr"), 
                    createString(" y"), 
                    createString("y"),
                    createString(""),
                    splitAsLines
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
        protected virtual int GetSubSegmentCount(TimeSpan span)
        {
            if (span > new TimeSpan(365, 0, 0, 0)) return 12;
            else if (span > new TimeSpan(30, 0, 0, 0)) return 4;
            else if (span > new TimeSpan(7, 0, 0, 0)) return 7;
            else if (span > new TimeSpan(1, 0, 0, 0)) return 8;
            else if (span > new TimeSpan(1, 0, 0)) return 4;
            else if (span > new TimeSpan(0, 30, 0)) return 6;
            else if (span > new TimeSpan(0, 10, 0)) return 10;
            else if (span > new TimeSpan(0, 1, 0)) return 6;
            else if (span > new TimeSpan(0, 0, 10)) return 10;
            else return 0;
        }

        /// <summary>
        /// Gets a new set of colors from the color scheme when the bar is clicked
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Timer.ColorizeTimerBar();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Draws the actual bar
            base.OnPaint(e);

            //Let's try to not create a recursive Refresh call, mmmk
            if ((Timer.InFreeMode || TrackSecondaryTimer) && !DrawHatched)
            {
                DrawHatchedOverflow = DrawHatched = true;
            }
            else if (!(Timer.InFreeMode || TrackSecondaryTimer) && DrawHatched)
            {
                DrawHatchedOverflow = DrawHatched = false;
            }

            Rectangle bounds = new Rectangle(BarMargin, BarMargin, Width - (2 * BarMargin), Height - (2 * BarMargin));

            //Do nothing else if the value of the bar is zero. To prevent DIV/0
            if (Value == 0)
                return;

            //Calculate the area to draw the subsegments in
            float overflowWidth = MaxValue / Value * bounds.Width;
            const float MARGIN_MULTIPLIER = 0.06f;
            const int MARGIN_MAX = 10;

            //Do nothing if there is less than one second left of the timer (overflow is higher that the width of the control)
            if (overflowWidth >= e.ClipRectangle.Width)
                return;

            //Set pen's transparency based on how big the subsegment area is
            using (Pen transparentPen = new Pen(Color.FromArgb((int)(overflowWidth / bounds.Width * 0x8F), Color.White)) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
            {
                //Draw subsegments
                int subSegmentCount = GetSubSegmentCount(DisplayedTimeLeft);
                float widthPerSubSegement = overflowWidth / subSegmentCount;
                int fullSubSegmentCount = (int)(bounds.Width / widthPerSubSegement);

                if (fullSubSegmentCount > 50)
                    return;

                for (int i = 0; i <= fullSubSegmentCount; i++)
                {
                    e.Graphics.DrawLine(
                        pen: transparentPen,
                        x1: bounds.Left + (float)i / subSegmentCount * overflowWidth,
                        y1: bounds.Top + Math.Min(bounds.Height * MARGIN_MULTIPLIER, MARGIN_MAX),
                        x2: bounds.Left + (float)i / subSegmentCount * overflowWidth,
                        y2: bounds.Bottom - Math.Min(bounds.Height * MARGIN_MULTIPLIER, MARGIN_MAX)
                    );
                }
            }
        }
    }
}
