using NewTimer.FormParts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.FormParts
{
    public class ClockControl : UserControl, ICountdown
    {
        private Color[] _colors;
        private bool _showDottedHourHand;

        /*
         * Constants and settings
         */

        //Background
        private static readonly Brush FRAME_BRUSH = new SolidBrush(ColorTranslator.FromHtml("#333"));
        private static readonly Brush BG_BRUSH = new SolidBrush(ColorTranslator.FromHtml("#222"));
        private static readonly Brush BG_TRUE_BRUSH = new SolidBrush(Config.GlobalBackColor);

        private static readonly Pen FRAME_MARK_PEN = new Pen(ColorTranslator.FromHtml("#444"), 3) { EndCap = LineCap.Round };
        private static readonly Pen FRAME_MARK_SMALL_PEN = new Pen(ColorTranslator.FromHtml("#444"), 1.5f) { EndCap = LineCap.Round };
        private static readonly Pen FRAME_MARK_BIG_PEN = new Pen(ColorTranslator.FromHtml("#444"), 6) { EndCap = LineCap.Round };

        //Scales
        private const float BG_FRAME_SCALE = 0.1f;
        private const float HOUR_HAND_SCALE = 0.5f;
        private const float MINUTE_HAND_SCALE = 1 - BG_FRAME_SCALE;
        private const float SECOND_HAND_SCALE = MINUTE_HAND_SCALE;

        private const float NUMBER_HOUR_DISTANCE_SCALE = 0.3f;
        private const float NUMBER_MINUTE_DISTANCE_SCALE = 0.5f;
        private const float NUMBER_SECOND_DISTANCE_SCALE = 0.7f;

        private const float NUMBER_HOUR_DEGREE_OFFSET = -25f;
        private const float NUMBER_MINUTE_DEGREE_OFFSET = -20f;
        private const float NUMBER_SECOND_DEGREE_OFFSET = -15f;

        private const float FONT_SIZE = 18f;

        //Hand and number colors
        private static readonly Color COLOR_HAND = ColorTranslator.FromHtml("#BBB");
        private static readonly Color COLOR_HAND_WEAK = ColorTranslator.FromHtml("#555");
        private static readonly Color COLOR_HAND_BORDER = ColorTranslator.FromHtml("#222");
        private static readonly Color COLOR_NUMBER = ColorTranslator.FromHtml("#444");

        //Hand and number pens and brushes
        private static readonly Pen PEN_BORDER_NORMAL = new Pen(COLOR_HAND_BORDER, 7) { EndCap = LineCap.ArrowAnchor, StartCap = LineCap.Round };
        private static readonly Pen PEN_FILL_NORMAL = new Pen(COLOR_HAND, 5) { EndCap = LineCap.ArrowAnchor, StartCap = LineCap.Round };

        private static readonly Pen PEN_BORDER_THIN = new Pen(COLOR_HAND_BORDER, 4) { EndCap = LineCap.Round, StartCap = LineCap.Round };
        private static readonly Pen PEN_FILL_THIN = new Pen(COLOR_HAND, 2) { EndCap = LineCap.Round, StartCap = LineCap.Round };

        private static readonly Pen PEN_DOTTED = new Pen(COLOR_HAND_WEAK, 4) { EndCap = LineCap.Round, StartCap = LineCap.Round, DashCap = DashCap.Round, DashStyle = DashStyle.Dot };
        private static readonly Pen PEN_DOTTED_THIN = new Pen(COLOR_HAND_WEAK, 2) { EndCap = LineCap.Round, StartCap = LineCap.Round, DashCap = DashCap.Round, DashStyle = DashStyle.Dot };

        //Disc settings
        private const float DISC_INITAL_SCALE = (1 - BG_FRAME_SCALE) * 0.95f;
        private const float DISC_INITAL_SCALE_HOURS = (1 - HOUR_HAND_SCALE) * 0.95f;
        private const float DISC_DIVIDEND_INCREMENT = 0.2f;

        private Rectangle squareArea;

        /// <summary>
        /// Creates a new clock control
        /// </summary>
        public ClockControl()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            _colors = Config.ColorScheme.GenerateMany(24, Config.MasterRandom).ToArray();

            Font = new Font(DefaultFont.FontFamily, FONT_SIZE);
        }

        /// <summary>
        /// Draws the foreground of the control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //Calculate area to draw 
            {
                int widthHeight = Math.Min(ClientRectangle.Width, ClientRectangle.Height);
                int xOffset = (ClientRectangle.Width - widthHeight) / 2;
                int yOffset = (ClientRectangle.Height - widthHeight) / 2;

                squareArea = new Rectangle(xOffset, yOffset, widthHeight, widthHeight);
            }

            base.OnPaint(e);

            //Set smoothing mode to anti-alias for smoothness
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //Only draw this when not in free mode
            if (!Config.InFreeMode)
            {
                OnDrawBackCircle(e);
                OnDrawNumbers(e);
            }

            OnDrawMinuteHand(e);
            OnDrawSecondHand(e);
            OnDrawHourHand(e);

            //Once again, do not draw anything more in free mode
            if (Config.InFreeMode)
                return;

            OnDrawMinutesLeftHand(e);
            OnDrawSecondsLeftHand(e);
            OnDrawHoursLeftHand(e);
        }

        /// <summary>
        /// Draws the background of the control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            //Set smoothing mode
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //Draw the frame
            e.Graphics.FillEllipse(FRAME_BRUSH, squareArea);

            /*
             * Draw the ticks;
             */

            //Calculates center point
            Point center = new Point(
                x: squareArea.Left + squareArea.Width / 2,
                y: squareArea.Top + squareArea.Height / 2
            );
            int radius = squareArea.Width / 2;

            //Draws every (60) tick
            for (int i = 0; i < 60; i++)
            {
                Pen pen;

                //Uses big pen for intervals of 15 minutes
                if (i % 15 == 0)
                    pen = FRAME_MARK_BIG_PEN;

                //Uses medium pen for intervals of 5 minutes
                else if (i % 5 == 0)
                    pen = FRAME_MARK_PEN;

                //Uses small pen for intervals of 1 minutes
                else
                    pen = FRAME_MARK_SMALL_PEN;

                //Draws the tick
                e.Graphics.DrawLine(
                    pen: pen,
                    pt1: center,
                    pt2: GetPointAtAngle(center, (int)(radius * 0.96f), i / 60f * 360f)
                );
            }

            /*
             * Overdraw colored ticks
             */
            if (!Config.InFreeMode)
            {
                //Calculates the start and end positions
                float target = getSegmentPosition(Config.Target);
                int i = getSegmentPosition(DateTime.Now);

                //Goes from the start to the target
                do
                {
                    i = (i + 1) % 60;

                    Pen pen;

                    //Uses a big pen for intervals of 15 minutes
                    if (i % 15 == 0)
                        pen = new Pen(_colors[0], FRAME_MARK_BIG_PEN.Width) { EndCap = LineCap.Round };

                    //Uses a medium pen for intervals of 5 minutes
                    else if (i % 5 == 0)
                        pen = new Pen(_colors[0], FRAME_MARK_PEN.Width) { EndCap = LineCap.Round };

                    //Uses a small pen for intervals of 1 minutes
                    else
                        pen = new Pen(_colors[0], FRAME_MARK_SMALL_PEN.Width) { EndCap = LineCap.Round };

                    //Draws colored tick
                    e.Graphics.DrawLine(
                        pen: pen,
                        pt1: center,
                        pt2: GetPointAtAngle(center, (int)(radius * 0.96f), (i / 60f * 360f) - 90f)
                    );

                } while (i != target);

                /*
                 * Gets the tick index for the provided time
                 */
                /* local */ int getSegmentPosition(DateTime time)
                {
                    return (time.Hour * 5 + (int)Math.Floor(time.Minute / 12f)) % 60;
                }
            }            

            /*
             * Draws the inner background (overdrawing the lines that make up the ticks)
             */
            Rectangle nonFrameArea = new Rectangle(
                x: (int)(squareArea.Left + (squareArea.Width * BG_FRAME_SCALE / 2)),
                y: (int)(squareArea.Top + (squareArea.Width * BG_FRAME_SCALE / 2)),
                width: (int)(squareArea.Width * (1 - BG_FRAME_SCALE)),
                height: (int)(squareArea.Height * (1 - BG_FRAME_SCALE))
            );

            e.Graphics.FillEllipse(Config.RealTimeLeft.TotalMinutes < 1f ? BG_TRUE_BRUSH : BG_BRUSH, nonFrameArea);
        }

        /// <summary>
        /// Draws the hour hand
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawHourHand(PaintEventArgs e)
        {
            OnDrawHand(
                e: e,
                scale: HOUR_HAND_SCALE, 
                angle: CalculateAngle(DateTime.Now.Hour % 12 + DateTime.Now.Minute / 60f, 12), 
                fillPen: PEN_FILL_NORMAL, 
                borderPen: PEN_BORDER_NORMAL
            );
        }

        /// <summary>
        /// Draws the minute hand
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawMinuteHand(PaintEventArgs e)
        {
            OnDrawHand(
                e: e,
                scale: MINUTE_HAND_SCALE, 
                angle: CalculateAngle(DateTime.Now.Minute + DateTime.Now.Second / 60f, 60), 
                fillPen: PEN_FILL_NORMAL, 
                borderPen: PEN_BORDER_NORMAL
            );
        }

        /// <summary>
        /// Draws the seconds hand
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawSecondHand(PaintEventArgs e)
        {
            OnDrawHand(
                e: e, 
                scale: SECOND_HAND_SCALE, 
                angle: CalculateAngle(DateTime.Now.Second + DateTime.Now.Millisecond / 1000f, 60), 
                fillPen: PEN_FILL_THIN, 
                borderPen: PEN_BORDER_THIN
            );
        }

        /// <summary>
        /// Draws the hours left hand
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawHoursLeftHand(PaintEventArgs e)
        {
            //If the application was started with less than one hour on the clock
            if (!_showDottedHourHand)
            {
                //Do not show the hours left hand
                if (Config.TimeLeft.Hours < 1)
                    return;

                //If there all of a sudden are more than one hour left, we want to show the hand again
                else
                    _showDottedHourHand = true;

            }

            //Draw hand
            OnDrawHand(
                e: e,
                scale: HOUR_HAND_SCALE, 
                angle: CalculateAngle((float)Config.TimeLeft.TotalHours % 12, 12), 
                fillPen: PEN_DOTTED, 
                borderPen: null
            );
        }

        /// <summary>
        /// Draws the minutes left hand
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawMinutesLeftHand(PaintEventArgs e)
        {
            OnDrawHand(
                e: e,
                scale: MINUTE_HAND_SCALE, 
                angle: CalculateAngle((float)Config.TimeLeft.TotalMinutes, 60), 
                fillPen: PEN_DOTTED, 
                borderPen: null
            );
        }

        /// <summary>
        /// Draws the seconds left hand
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawSecondsLeftHand(PaintEventArgs e)
        {
            //Only show the seconds left hand if the target's seconds component is non-zero
            if (Config.Target.Second == 0)
                return;

            //Draw hand
            OnDrawHand(
                e: e, 
                scale: SECOND_HAND_SCALE, 
                angle: CalculateAngle((float)Config.TimeLeft.TotalSeconds, 60), 
                fillPen: PEN_DOTTED_THIN, 
                borderPen: null
            );
        }

        /// <summary>
        /// Draws the background pie
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawBackCircle(PaintEventArgs e)
        {
            /*
             * Creates a filled pie
             */
            /* local */ void fillPie(Brush color, float startAngle, float angle, float scale)
            {
                Rectangle area = new Rectangle(
                    x: squareArea.X + (int)(squareArea.Width * (1 - scale) / 2f),
                    y: squareArea.Y + (int)(squareArea.Height * (1 - scale) / 2f),
                    width: Math.Max(1, (int)(squareArea.Width * scale)),
                    height: Math.Max(1, (int)(squareArea.Height * scale))
                );

                e.Graphics.FillPie(color, area, -90 + startAngle, angle);
            }

            /*
             * Creates an unfilled pie
             */
            /* local */ void drawPie(Pen color, float startAngle, float angle, float scale)
            {
                Rectangle area = new Rectangle(
                    x: squareArea.X + (int)(squareArea.Width * (1 - scale) / 2f),
                    y: squareArea.Y + (int)(squareArea.Height * (1 - scale) / 2f),
                    width: Math.Max(1, (int)(squareArea.Width * scale)),
                    height: Math.Max(1, (int)(squareArea.Height * scale))
                );

                e.Graphics.DrawPie(color, area, -90 + startAngle, angle);
            }

            //Create the final-minute color shift
            if (Config.TimeLeft.TotalMinutes < 1f && !Config.Overtime)
            {
                fillPie(
                    color: BG_BRUSH,
                    startAngle: (DateTime.Now.Second + DateTime.Now.Millisecond / 1000f) / 60f * 360f,
                    angle: (float)Config.RealTimeLeft.TotalSeconds / 60f * 360f,
                    scale: 1 - BG_FRAME_SCALE
                );
            }

            /*
             * Draw pies
             */

            //Draw for days (more than 1 day)
            if (Config.TimeLeft.TotalDays >= 1)
            {
                float dividend = 1f;
                for (int i = 0; i < Math.Ceiling(Config.TimeLeft.TotalDays / 12); i++)
                {
                    //Create a colored pen based on what days we are drawing
                    using (Brush b = new SolidBrush(_colors[(i + 6) % _colors.Length]))
                    {
                        if (i == Math.Floor(Config.TimeLeft.TotalDays / 12))
                        {
                            fillPie(
                                color: b,
                                startAngle: 0f,
                                angle: (float)Config.TimeLeft.TotalDays % 12f * 360f / 12f,
                                scale: DISC_INITAL_SCALE / dividend);
                        }
                        else
                        {
                            fillPie(
                                color: b,
                                startAngle: 0f,
                                angle: 360f,
                                scale: DISC_INITAL_SCALE / dividend
                            );
                        }
                    }

                    dividend += DISC_INITAL_SCALE_HOURS;
                }

                //Draw lines segmenting the bar
                using (Pen p = new Pen(Color.FromArgb(0x7F, Color.Silver), 1.5f) { DashStyle = DashStyle.Dash })
                {
                    for (int a = 0; a < Math.Min(12, Config.TimeLeft.TotalDays); a++)
                    {
                        Point startPoint = new Point(squareArea.X + squareArea.Width / 2, squareArea.Y + squareArea.Height / 2);
                        PointF endPoint = GetPointAtAngle(startPoint, (int)(DISC_INITAL_SCALE * squareArea.Width / 2), CalculateAngle(a * (Config.Overtime ? -1 : 1), 12));

                        e.Graphics.DrawLine(p, startPoint, endPoint);
                    }
                }

                //Draw week indicators
                fillPie(Brushes.Gray, 7 / 12f * 360f, 1f, DISC_INITAL_SCALE);

                if (Config.TimeLeft.TotalDays >= 7f)
                    fillPie(Brushes.Gray, 2 / 12f * 360f, 1f, DISC_INITAL_SCALE);

                if (Config.TimeLeft.TotalDays >= 14f)
                    fillPie(Brushes.Gray, 9 / 12f * 360f, 1f, DISC_INITAL_SCALE);

                fillPie(BG_BRUSH, 0f, 360f, DISC_INITAL_SCALE_HOURS);
            }

            //Draw for hours (more than 3 hours)
            else if (Config.TimeLeft.TotalHours >= 3)
            {
                float dividend = 1f;
                for (int i = 0; i < Math.Ceiling(Config.TimeLeft.TotalDays * 2); i++)
                {
                    //Create a colored pen based on what hours we are drawing
                    using (Brush b = new SolidBrush(_colors[(i + 4) % _colors.Length]))
                    {
                        if (i == Math.Floor(Config.TimeLeft.TotalDays * 2))
                        {
                            fillPie(
                                color: b,
                                startAngle: ((DateTime.Now.Hour % 12) + DateTime.Now.Minute / 60f) / 12f * 360f,
                                angle: (float)(Config.RealTimeLeft.TotalHours % 12) / 12f * 360f,
                                scale: DISC_INITAL_SCALE_HOURS / dividend
                            );
                        }
                        else
                        {
                            fillPie(
                                color: b,
                                startAngle: 0f,
                                angle: 360f,
                                scale: DISC_INITAL_SCALE_HOURS / dividend
                            );
                        }
                    }

                    dividend += DISC_INITAL_SCALE_HOURS;
                }
            }

            //Draw for minutes (less than 3 hours)
            else
            {
                float dividend = 1f;
                for (int i = 0; i < Math.Ceiling(Config.TimeLeft.TotalHours); i++)
                {
                    //Create a colored pen based on what hour we are drawing
                    using (SolidBrush b = new SolidBrush(_colors[i % _colors.Length]))
                    {
                        if (i == Math.Floor(Config.TimeLeft.TotalHours))
                        {
                            fillPie(
                                color: b,
                                startAngle: (DateTime.Now.Minute + DateTime.Now.Second / 60f) / 60f * 360f,
                                angle: (float)(Config.RealTimeLeft.TotalMinutes % 60) / 60f * 360f,
                                scale: DISC_INITAL_SCALE / dividend
                            );
                        }
                        else
                        {
                            fillPie(
                                color: b,
                                startAngle: 0f,
                                angle: 360f,
                                scale: DISC_INITAL_SCALE / dividend
                            );
                        }
                    }

                    dividend += DISC_DIVIDEND_INCREMENT;
                }
            }

            //Draw for seconds (less than 12 minutes)
            if (Config.TimeLeft.TotalMinutes < 12)
            {
                using (Pen p = new Pen(_colors[10 % _colors.Length], 2f) { DashStyle = DashStyle.Dash })
                {
                    drawPie(
                        color: p,
                        startAngle: 0f,
                        angle: (float)Config.RealTimeLeft.TotalMinutes / 12f * 360f,
                        scale: DISC_INITAL_SCALE
                    );
                }
            }
        }

        /// <summary>
        /// Draws the number overlays
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawNumbers(PaintEventArgs e)
        {
            Point center = new Point(
                x: squareArea.Left + (squareArea.Width / 2),
                y: squareArea.Top + (squareArea.Height / 2)
            );

            //Draw number at hour hand
            {
                PointF p = GetPointAtAngle(
                    origin: center,
                    length: (int)(NUMBER_HOUR_DISTANCE_SCALE * squareArea.Width / 2),
                    angle: CalculateAngle(DateTime.Now.Hour % 12 + DateTime.Now.Minute / 60f, 12) + NUMBER_HOUR_DEGREE_OFFSET
                );

                byte s = (byte)(Math.Min(byte.MaxValue, Math.Max((Config.TimeLeft.TotalHours - 1), 0) / (1 / 6f) * byte.MaxValue));
                using (Brush brush = new SolidBrush(Color.FromArgb(s, Color.White)))
                {
                    e.Graphics.DrawString(
                        s: Config.TimeLeft.Hours.ToString(),
                        font: Font,
                        brush: brush,
                        point: p,
                        format: new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
                    );
                }
            }


            //Draw number at minute hand
            {
                PointF p = GetPointAtAngle(
                    origin: center,
                    length: (int)(NUMBER_MINUTE_DISTANCE_SCALE * squareArea.Width / 2),
                    angle: CalculateAngle(DateTime.Now.Minute + DateTime.Now.Second / 60f, 60) + NUMBER_MINUTE_DEGREE_OFFSET
                );

                //Calculates transparency
                byte s = (byte)(Math.Min(byte.MaxValue, Math.Max((Config.TimeLeft.TotalMinutes - 1), 0) / 5f * byte.MaxValue));
                using (Brush brush = new SolidBrush(Color.FromArgb(s, Color.White)))
                {
                    e.Graphics.DrawString(
                        s: Config.TimeLeft.Minutes.ToString("00"),
                        font: Font,
                        brush: brush,
                        point: p,
                        format: new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
                    );
                }
            }


            //Draw number at second hand
            {
                PointF p = GetPointAtAngle(
                    origin: center,
                    length: (int)(NUMBER_SECOND_DISTANCE_SCALE * squareArea.Width / 2),
                    angle: CalculateAngle(DateTime.Now.Second + DateTime.Now.Millisecond / 1000f, 60) + NUMBER_SECOND_DEGREE_OFFSET
                );

                //Calculates transparency
                byte s = (byte)(Config.RealTimeLeft.Milliseconds / 1000f * 255);
                using (Brush brush = new SolidBrush(Color.FromArgb(s, Color.White)))
                {
                    e.Graphics.DrawString(
                        s: Config.TimeLeft.Seconds.ToString("00"),
                        font: Font,
                        brush: brush,
                        point: p,
                        format: new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
                    );
                }
            }
        }

        /// <summary>
        /// Draws a hand at the provided angle with the provided pens and scale
        /// </summary>
        /// <param name="e"></param>
        /// <param name="scale"></param>
        /// <param name="angle"></param>
        /// <param name="fillPen"></param>
        /// <param name="borderPen"></param>
        protected virtual void OnDrawHand(PaintEventArgs e, float scale, float angle, Pen fillPen, Pen borderPen)
        {
            Point center = new Point(
                x: squareArea.Left + squareArea.Width / 2,
                y: squareArea.Top + squareArea.Height / 2
            );

            PointF anglePoint = GetPointAtAngle(center, (int)(scale * squareArea.Width) / 2, angle);

            //Draws the border
            if (borderPen != null)
                e.Graphics.DrawLine(borderPen, center, anglePoint);

            //Draws the fill
            e.Graphics.DrawLine(fillPen, center, anglePoint);
        }

        /// <summary>
        /// Handles the click of the control, generating a new color scheme
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            _colors = Config.ColorScheme.GenerateMany(12, Config.MasterRandom).ToArray();
            Invalidate();
        }

        /// <summary>
        /// Gets the point at a particular distance away from an orgin in a particular direction (using angle)
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="length"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        protected static PointF GetPointAtAngle(Point origin, int length, float angle)
        {
            return new PointF(
                x: origin.X + (float)Math.Cos(ToRadiants(angle)) * length,
                y: origin.Y + (float)Math.Sin(ToRadiants(angle)) * length
            );
        }

        /// <summary>
        /// Converts an angle to radiants
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        protected static float ToRadiants(double angle)
        {
            return (float)((Math.PI / 180) * angle);
        }

        /// <summary>
        /// Calculates the angle of a would-be hand when configured to be represent a value of a maximum value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        protected static float CalculateAngle(float value, float maxValue)
        {
            return (value / maxValue * 360) - 90;
        }

        /// <summary>
        /// Invalidates the control when the countdown ticks
        /// </summary>
        /// <param name="span"></param>
        /// <param name="isOvertime"></param>
        public void OnCountdownTick(TimeSpan span, bool isOvertime)
        {
            Invalidate();
        }
    }
}
