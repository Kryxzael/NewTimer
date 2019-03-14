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
    [DesignerCategory("")]
    public class TimerBar : SegmentedBar, ICountdown
    {
        public void OnCountdownTick(TimeSpan span, bool isOvertime)
        {
            if (span >= new TimeSpan(360, 0, 0, 0)) //Sets base to 1 year
            {
                Value = (float)span.TotalDays / 360f;
            }
            else if (span >= new TimeSpan(1, 0, 0, 0)) //Sets base to 1 day
            {
                Value = (float)span.TotalDays;
            }
            else if (span >= new TimeSpan(0, 1, 0, 0)) //Sets base to 1 hour
            {
                Value = (float)span.TotalHours;
            }
            else if (span >= new TimeSpan(0, 0, 1, 0)) //Sets base to 1 minute
            {
                Value = (float)span.TotalMinutes;
            }
            else //Sets base to 1 second
            {
                Value = (float)span.TotalSeconds;
            }

            ApplySettings(Config.BarSettings.First(i => i.Key <= span).Value);
        }

        public override Color BackColor
        {
            get
            {
                return Config.GlobalBackColor;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return Color.Black;
            }
        }

        protected override string GetStringForBarSegment(int segmentValue)
        {


            if (Config.GetTimeLeft().TotalMinutes < 1)
            {
                return segmentValue + "sec";
            }            
            else if (Config.GetTimeLeft().TotalHours < 1)
            {
                return segmentValue + "min";
            }
            else if (Config.GetTimeLeft().TotalDays < 1)
            {
                return segmentValue + "hr";
            }
            else if (Config.GetTimeLeft().TotalDays < 365)
            {
                return segmentValue + "d";
            }
            else
            {
                return segmentValue + "y";
            }

            ///*local*/ string getFractionSymbol(float value)
            //{
            //    const string QUARTER = "¼";
            //    const string HALF = "½";
            //    const string THREE_QUARTERS = "¾";
            //    const string THIRD = "⅓";
            //    const string TWO_THIRDS = "⅔";

            //    int @decimal = Config.GetDecimals(value, 2);
            //    int @int = (int)value;

            //    string _ = @int == 0 ? "" : @int.ToString();
            //    switch (@decimal)
            //    {
            //        case 25:
            //            return QUARTER + _;
            //        case 50:
            //            return HALF + _;
            //        case 75:
            //            return THREE_QUARTERS + _;
            //        case 33:
            //            return THIRD + _;
            //        case 66:
            //            return TWO_THIRDS + _;
            //        default:
            //            return @int.ToString();
            //    }
            //}
        }

        protected virtual int GetSubSegmentCount(TimeSpan span)
        {
            if (span > new TimeSpan(365, 0, 0, 0)) return 12;
            else if (span > new TimeSpan(30, 0, 0, 0)) return 4;
            else if (span > new TimeSpan(7, 0, 0, 0)) return 7;
            else if (span > new TimeSpan(1, 0, 0, 0)) return 8;
            else if (span > new TimeSpan(12, 0, 0)) return 6;
            else if (span > new TimeSpan(6, 0, 0)) return 6;
            else if (span > new TimeSpan(1, 0, 0)) return 4;
            else if (span > new TimeSpan(0, 30, 0)) return 6;
            else if (span > new TimeSpan(0, 10, 0)) return 10;
            else if (span > new TimeSpan(0, 1, 0)) return 6;
            else if (span > new TimeSpan(0, 0, 10)) return 10;
            else return 0;
        }

        /// <summary>
        /// Randomizes the color scheme
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Config.RandomizeColorScheme();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Value == 0)
            {
                return;
            }

            float overflowWidth = MaxValue / Value * e.ClipRectangle.Width;

            if (overflowWidth >= e.ClipRectangle.Width)
            {
                return;
            }

            using (Pen transparentPen = new Pen(Color.FromArgb((int)(overflowWidth / e.ClipRectangle.Width * 0x8F), Color.White)))
            {
                int SubSegmentCount = GetSubSegmentCount(Config.GetTimeLeft());
                for (int i = 0; i < SubSegmentCount; i++)
                {
                    e.Graphics.DrawLine(
                        pen: transparentPen,
                        x1: (float)i / SubSegmentCount * overflowWidth,
                        y1: e.ClipRectangle.Top,
                        x2: (float)i / SubSegmentCount * overflowWidth,
                        y2: e.ClipRectangle.Bottom
                    );
                }
            }

        }
    }
}
