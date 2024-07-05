﻿using System;
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
                ApplySettings(Timer.BarSettings.First(i => i.Key <= span).Value);
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
            if      (span >= new TimeSpan(365, 00, 00, 00)) return 12;
            else if (span >= new TimeSpan( 50, 00, 00, 00)) return  4; //Roughly one week
            else if (span >= new TimeSpan( 30, 00, 00, 00)) return 30;
            else if (span >= new TimeSpan(  7, 00, 00, 00)) return  7;
            else if (span >= new TimeSpan(  4, 00, 00, 00)) return  6;
            else if (span >= new TimeSpan(  2, 00, 00, 00)) return 12;
            else if (span >= new TimeSpan(  1, 00, 00, 00)) return 24;
            else if (span >= new TimeSpan(  0, 01, 00, 00)) return  4;
            else if (span >= new TimeSpan(  0, 00, 30, 00)) return  6;
            else if (span >= new TimeSpan(  0, 00, 10, 00)) return 10;
            else if (span >= new TimeSpan(  0, 00, 01, 00)) return  6;
            else if (span >= new TimeSpan(  0, 00, 00, 10)) return 10;
            else                                            return  0;
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
            //Let's try to not create a recursive Refresh call, mmmk
            if (Timer.InFreeMode && !DrawHatched)
            {
                DrawHatchedOverflow = DrawHatched = true;
            }
            else if (!Timer.InFreeMode && DrawHatched)
            {
                DrawHatchedOverflow = DrawHatched = false;
            }

            if (Timer.InFreeMode)
                return;

            Rectangle bounds = new Rectangle(BarMargin, BarMargin, Width - (2 * BarMargin), Height - (2 * BarMargin));

            //Do nothing else if the value of the bar is zero. To prevent DIV/0
            if (Value == 0)
                return;

            //Calculate the area to draw the subsegments in
            float overflowScale = MaxValue / Value; //TODO: Name is stupid. It's the scale of the area that isn't overflown
            float overflowWidth = overflowScale * bounds.Width;

            //Do nothing if there is less than one second left of the timer (overflow is higher that the width of the control)
            if (overflowWidth >= e.ClipRectangle.Width)
                return;

            //Set pen's properties based on how big the subsegment area is
            float subSegmentPenSize = Lerp(1f, 4f, (overflowScale - 0.75f) * 4f);
            float subSegmentMarginScale = Lerp(0.04f, 0f, (overflowScale - 0.5f) * 2f);
            const int subSegmentMarginMax = 10;

            using (Pen transparentPen = new Pen(Color.FromArgb((int)(overflowWidth / bounds.Width * 0x8F), Color.White), subSegmentPenSize))
            {
                //Draw subsegments
                //This code was originally designed to only draw sub-segments on the left-most part of the bar
                //It has been hacked to draw segments over the entire bar. As a result, this code is ugly as fuck
                //Please rewrite this at some point. Thank you, and good luck
                int subSegmentCount = GetSubSegmentCount(Timer.TimeLeft);

                if (subSegmentCount == 0)
                    return;

                float widthPerSubSegement = overflowWidth / subSegmentCount;
                int fullSubSegmentCount = (int)(bounds.Width / widthPerSubSegement);

                if (fullSubSegmentCount > 50)
                    return;

                for (int i = 1; i <= fullSubSegmentCount; i++)
                {
                    e.Graphics.DrawLine(
                        pen: transparentPen,
                        x1: bounds.Left + (float)i / subSegmentCount * overflowWidth,
                        y1: bounds.Top + Math.Min(bounds.Height * subSegmentMarginScale, subSegmentMarginMax),
                        x2: bounds.Left + (float)i / subSegmentCount * overflowWidth,
                        y2: bounds.Bottom - Math.Min(bounds.Height * subSegmentMarginScale, subSegmentMarginMax)
                    );
                }
            }
        }

        public static float Lerp(float a, float b, float t) => a * (1 - t) + b * t;
    }
}
