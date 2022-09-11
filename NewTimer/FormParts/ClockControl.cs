using NewTimer.FormParts;
using NewTimer.ThemedColors;

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
        //private Dictionary<TimerConfig, Color[]> _colors = new Dictionary<TimerConfig, Color[]>();

        private static readonly Func<Color, Brush>[] _primaryBrushGenerators = new Func<Color, Brush>[] 
        { 
            i => new SolidBrush(i) 
        };

        private static readonly Func<Color, Brush>[] _secondaryBrushGenerators = new Func<Color, Brush>[] 
        { 
            i => new HatchBrush(HatchStyle.BackwardDiagonal, i, Color.Transparent),
            i => new HatchBrush(HatchStyle.ForwardDiagonal, i, Color.Transparent),
            i => new HatchBrush(HatchStyle.Cross, i, Color.Transparent), 
        };

        private bool _showDottedHourHand;

        /*
         * Constants and settings
         */

        //Background
        private static readonly ThemedSolidBrush FRAME_BRUSH = new ThemedSolidBrush(ColorTranslator.FromHtml("#999"), ColorTranslator.FromHtml("#333"));
        private static readonly ThemedSolidBrush BG_BRUSH = new ThemedSolidBrush(ColorTranslator.FromHtml("#ccc"), ColorTranslator.FromHtml("#222"));
        private static readonly ThemedSolidBrush BG_TRUE_BRUSH = new ThemedSolidBrush(Globals.GlobalBackColor);

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
        private static readonly ThemedColor COLOR_HAND = new ThemedColor(ColorTranslator.FromHtml("#333"), ColorTranslator.FromHtml("#BBB"));
        private static readonly ThemedColor COLOR_HAND_WEAK = new ThemedColor(ColorTranslator.FromHtml("#555"), ColorTranslator.FromHtml("#555"));
        private static readonly ThemedColor COLOR_HAND_BORDER = new ThemedColor(ColorTranslator.FromHtml("#222"), ColorTranslator.FromHtml("#222"));
        private static readonly ThemedColor COLOR_NUMBER = new ThemedColor(Color.Black, Color.White);

        //Hand and number pens and brushes
        private static readonly ThemedPen PEN_BORDER_NORMAL = new ThemedPen(COLOR_HAND_BORDER, 7f, LineCap.Round, LineCap.ArrowAnchor, DashStyle.Solid);
        private static readonly ThemedPen PEN_FILL_NORMAL = new ThemedPen(COLOR_HAND, 5f, LineCap.Round, LineCap.ArrowAnchor, DashStyle.Solid);

        private static readonly ThemedPen PEN_BORDER_ROUND = new ThemedPen(COLOR_HAND_BORDER, 7f, LineCap.Round, LineCap.RoundAnchor, DashStyle.Solid);
        private static readonly ThemedPen PEN_FILL_ROUND = new ThemedPen(COLOR_HAND, 5f, LineCap.Round, LineCap.RoundAnchor, DashStyle.Solid);

        private static readonly ThemedPen PEN_BORDER_THIN = new ThemedPen(COLOR_HAND_BORDER, 4f, LineCap.Round, LineCap.Round, DashStyle.Solid);
        private static readonly ThemedPen PEN_FILL_THIN = new ThemedPen(COLOR_HAND, 2f, LineCap.Round, LineCap.Round, DashStyle.Solid);

        private static readonly ThemedPen PEN_BORDER_THIN_ROUND = new ThemedPen(COLOR_HAND_BORDER, 4f, LineCap.Round, LineCap.RoundAnchor, DashStyle.Solid);
        private static readonly ThemedPen PEN_FILL_THIN_ROUND = new ThemedPen(COLOR_HAND, 2f, LineCap.Round, LineCap.RoundAnchor, DashStyle.Solid);

        private static readonly ThemedPen PEN_DOTTED = new ThemedPen(COLOR_HAND_WEAK, 4f, LineCap.Round, LineCap.Round, DashStyle.Dot);
        private static readonly ThemedPen PEN_DOTTED_THIN = new ThemedPen(COLOR_HAND_WEAK, 2f, LineCap.Round, LineCap.Round, DashStyle.Dot);

        private static readonly ThemedPen PEN_DOTTED_ARROW = new ThemedPen(COLOR_HAND_WEAK, 4f, LineCap.Round, LineCap.Round, DashStyle.Dot);
        private static readonly ThemedPen PEN_DOTTED_ARROW_THIN = new ThemedPen(COLOR_HAND_WEAK, 2f, LineCap.Round, LineCap.Round, DashStyle.Dot);

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
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            //Only draw this when not in free mode
            if (!Globals.PrimaryTimer.InFreeMode)
                OnDrawBackCircle(e, Globals.PrimaryTimer, _primaryBrushGenerators, true);

            if (!Globals.SecondaryTimer.InFreeMode)
                OnDrawBackCircle(e, Globals.SecondaryTimer, _secondaryBrushGenerators, false);

            //Same for numbers
            if (!Globals.PrimaryTimer.InFreeMode)
                OnDrawNumbers(e);

            OnDrawMinuteHand(e);
            OnDrawSecondHand(e);
            OnDrawHourHand(e);

            //Once again, do not draw anything more in free mode
            if (Globals.PrimaryTimer.InFreeMode)
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
            if (!Globals.PrimaryTimer.InFreeMode)
            {
                //Calculates the start and end positions
                float target = getSegmentPosition(Globals.PrimaryTimer.Target);
                int i = getSegmentPosition(DateTime.Now);

                //Goes from the start to the target
                do
                {
                    i = (i + 1) % 60;

                    Pen pen;

                    //Uses a big pen for intervals of 15 minutes
                    if (i % 15 == 0)
                        pen = new Pen(Globals.PrimaryTimer.AnalogColors[0], FRAME_MARK_BIG_PEN.Width) { EndCap = LineCap.Round };

                    //Uses a medium pen for intervals of 5 minutes
                    else if (i % 5 == 0)
                        pen = new Pen(Globals.PrimaryTimer.AnalogColors[0], FRAME_MARK_PEN.Width) { EndCap = LineCap.Round };

                    //Uses a small pen for intervals of 1 minutes
                    else
                        pen = new Pen(Globals.PrimaryTimer.AnalogColors[0], FRAME_MARK_SMALL_PEN.Width) { EndCap = LineCap.Round };

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

            e.Graphics.FillEllipse(Globals.PrimaryTimer.RealTimeLeft.TotalMinutes < 1f ? BG_TRUE_BRUSH : BG_BRUSH, nonFrameArea);
        }

        /// <summary>
        /// Draws the hour hand
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawHourHand(PaintEventArgs e)
        {
            Pen fillPen, borderPen;

            if (Globals.SwapHandPriorities)
            {
                fillPen = PEN_DOTTED_ARROW;
                borderPen = null;
            }
            else
            {
                fillPen = PEN_FILL_NORMAL;
                borderPen = PEN_BORDER_NORMAL;
            }

            OnDrawHand(
                e: e,
                scale: HOUR_HAND_SCALE, 
                angle: CalculateAngle(DateTime.Now.Hour % 12 + DateTime.Now.Minute / 60f, 12), 
                fillPen: fillPen, 
                borderPen: borderPen
            );
        }

        /// <summary>
        /// Draws the minute hand
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawMinuteHand(PaintEventArgs e)
        {
            Pen fillPen, borderPen;

            if (Globals.SwapHandPriorities)
            {
                fillPen = PEN_DOTTED_ARROW;
                borderPen = null;
            }
            else
            {
                fillPen = PEN_FILL_NORMAL;
                borderPen = PEN_BORDER_NORMAL;
            }

            OnDrawHand(
                e: e,
                scale: MINUTE_HAND_SCALE, 
                angle: CalculateAngle(DateTime.Now.Minute + DateTime.Now.Second / 60f, 60), 
                fillPen: fillPen, 
                borderPen: borderPen
            );
        }

        /// <summary>
        /// Draws the seconds hand
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawSecondHand(PaintEventArgs e)
        {
            Pen fillPen, borderPen;

            if (Globals.SwapHandPriorities)
            {
                fillPen = PEN_DOTTED_ARROW_THIN;
                borderPen = null;
            }
            else
            {
                fillPen = PEN_FILL_THIN;
                borderPen = PEN_BORDER_THIN;
            }

            OnDrawHand(
                e: e, 
                scale: SECOND_HAND_SCALE, 
                angle: CalculateAngle(DateTime.Now.Second + DateTime.Now.Millisecond / 1000f, 60), 
                fillPen: fillPen, 
                borderPen: borderPen
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
                if (Globals.PrimaryTimer.TimeLeft.Hours < 1)
                    return;

                //If there all of a sudden are more than one hour left, we want to show the hand again
                else
                    _showDottedHourHand = true;

            }

            Pen fillPen, borderPen;

            if (Globals.SwapHandPriorities)
            {
                fillPen = PEN_FILL_ROUND;
                borderPen = PEN_BORDER_ROUND;
            }
            else
            {
                fillPen = PEN_DOTTED;
                borderPen = null;
            }

            //Draw hand
            OnDrawHand(
                e: e,
                scale: HOUR_HAND_SCALE, 
                angle: CalculateAngle((float)Globals.PrimaryTimer.TimeLeft.TotalHours % 12, 12), 
                fillPen: fillPen, 
                borderPen: borderPen
            );
        }

        /// <summary>
        /// Draws the minutes left hand
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawMinutesLeftHand(PaintEventArgs e)
        {
            Pen fillPen, borderPen;

            if (Globals.SwapHandPriorities)
            {
                fillPen = PEN_FILL_ROUND;
                borderPen = PEN_BORDER_ROUND;
            }
            else
            {
                fillPen = PEN_DOTTED;
                borderPen = null;
            }

            OnDrawHand(
                e: e,
                scale: MINUTE_HAND_SCALE, 
                angle: CalculateAngle((float)Globals.PrimaryTimer.TimeLeft.TotalMinutes, 60), 
                fillPen: fillPen, 
                borderPen: borderPen
            );
        }

        /// <summary>
        /// Draws the seconds left hand
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawSecondsLeftHand(PaintEventArgs e)
        {
            //Only show the seconds left hand if the target's seconds component is non-zero
            if (Globals.PrimaryTimer.Target.Second == 0)
                return;

            Pen fillPen, borderPen;

            if (Globals.SwapHandPriorities)
            {
                fillPen = PEN_FILL_THIN_ROUND;
                borderPen = PEN_BORDER_THIN_ROUND;
            }
            else
            {
                fillPen = PEN_DOTTED_THIN;
                borderPen = null;
            }

            //Draw hand
            OnDrawHand(
                e: e, 
                scale: SECOND_HAND_SCALE, 
                angle: CalculateAngle((float)Globals.PrimaryTimer.TimeLeft.TotalSeconds, 60), 
                fillPen: fillPen, 
                borderPen: borderPen
            );
        }

        /// <summary>
        /// Draws the background pie
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawBackCircle(PaintEventArgs e, TimerConfig timer, Func<Color, Brush>[] createBrush, bool createLastCountdownEffects)
        {
            /*
             * Creates a filled pie
             */
            /* local */ void fillPie(Brush color, float startAngle, float angle, float scale, float centerOffset)
            {
                Rectangle area = new Rectangle(
                    x: squareArea.X + (int)(squareArea.Width * (1 - scale) / 2f),
                    y: squareArea.Y + (int)(squareArea.Height * (1 - scale) / 2f),
                    width: Math.Max(1, (int)(squareArea.Width * scale)),
                    height: Math.Max(1, (int)(squareArea.Height * scale))
                );

                Region region = new Region();
                GraphicsPath path = new GraphicsPath();
                path.AddPie(area, -90 + startAngle, angle);

                region.MakeEmpty();
                region.Union(path);

                if (centerOffset > 0)
                {
                    GraphicsPath exclPath = new GraphicsPath();
                    Rectangle exclArea = new Rectangle(
                        x: squareArea.X + (int)(squareArea.Width * (1 - centerOffset) / 2f),
                        y: squareArea.Y + (int)(squareArea.Height * (1 - centerOffset) / 2f),
                        width: Math.Max(1, (int)(squareArea.Width * centerOffset)),
                        height: Math.Max(1, (int)(squareArea.Height * centerOffset))
                    );

                    exclPath.AddEllipse(exclArea);
                    region.Exclude(exclPath);
                    exclPath.Dispose();
                }

                e.Graphics.FillRegion(color, region);
                region.Dispose();
                path.Dispose();
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

            //Color scheme to use
            Color[] colors = timer.AnalogColors;

            //Create the final-minute color shift
            if (createLastCountdownEffects && timer.TimeLeft.TotalMinutes < 1f && !timer.Overtime)
            {
                fillPie(
                    color: BG_BRUSH,
                    startAngle: (DateTime.Now.Second + DateTime.Now.Millisecond / 1000f) / 60f * 360f,
                    angle: (float)timer.RealTimeLeft.TotalSeconds / 60f * 360f,
                    scale: 1 - BG_FRAME_SCALE,
                    centerOffset: 0f
                );
            }

            /*
             * Draw pies
             */

            //Draw for days (more than 1 day)
            if (timer.TimeLeft.TotalDays >= 1)
            {
                float dividend = 1f;
                for (int i = 0; i < Math.Ceiling(timer.TimeLeft.TotalDays / 12); i++)
                {
                    //Create a colored pen based on what days we are drawing
                    using (Brush b = createBrush[i % createBrush.Length](colors[(i + 6) % colors.Length]))
                    {
                        if (i == Math.Floor(timer.TimeLeft.TotalDays / 12))
                        {
                            fillPie(
                                color: b,
                                startAngle: 0f,
                                angle: (float)timer.TimeLeft.TotalDays % 12f * 360f / 12f,
                                scale: DISC_INITAL_SCALE / dividend,
                                centerOffset: DISC_INITAL_SCALE_HOURS
                            );
                        }
                        else
                        {
                            fillPie(
                                color: b,
                                startAngle: 0f,
                                angle: 360f,
                                scale: DISC_INITAL_SCALE / dividend,
                                centerOffset: DISC_INITAL_SCALE_HOURS
                            );
                        }
                    }

                    dividend += DISC_INITAL_SCALE_HOURS;
                }

                //Draw lines segmenting the bar
                using (Pen p = new Pen(Color.FromArgb(0x7F, Color.Silver), 1.5f) { DashStyle = DashStyle.Dash })
                {
                    for (int a = 0; a < Math.Min(12, timer.TimeLeft.TotalDays); a++)
                    {
                        Point center = new Point(squareArea.X + squareArea.Width / 2, squareArea.Y + squareArea.Height / 2);
                        PointF startPoint = GetPointAtAngle(center, (int)(DISC_INITAL_SCALE_HOURS * squareArea.Width / 2), CalculateAngle(a * (timer.Overtime ? -1 : 1), 12));
                        PointF endPoint   = GetPointAtAngle(center, (int)(DISC_INITAL_SCALE       * squareArea.Width / 2), CalculateAngle(a * (timer.Overtime ? -1 : 1), 12));

                        e.Graphics.DrawLine(p, startPoint, endPoint);
                    }
                }

                //Draw week indicators
                fillPie(Brushes.Gray, 7 / 12f * 360f, 1f, DISC_INITAL_SCALE, DISC_INITAL_SCALE_HOURS);

                if (timer.TimeLeft.TotalDays >= 7f)
                    fillPie(Brushes.Gray, 2 / 12f * 360f, 1f, DISC_INITAL_SCALE, DISC_INITAL_SCALE_HOURS);

                if (timer.TimeLeft.TotalDays >= 14f)
                    fillPie(Brushes.Gray, 9 / 12f * 360f, 1f, DISC_INITAL_SCALE, DISC_INITAL_SCALE_HOURS);

                //fillPie(BG_BRUSH, 0f, 360f, DISC_INITAL_SCALE_HOURS);
            }

            //Draw for hours (more than 3 hours)
            else if (timer.TimeLeft.TotalHours >= 3)
            {
                float dividend = 1f;
                for (int i = 0; i < Math.Ceiling(timer.TimeLeft.TotalDays * 2); i++)
                {
                    //Create a colored pen based on what hours we are drawing
                    using (Brush b = createBrush[i % createBrush.Length](colors[(i + 4) % colors.Length]))
                    {
                        if (i == Math.Floor(timer.TimeLeft.TotalDays * 2))
                        {
                            fillPie(
                                color: b,
                                startAngle: ((DateTime.Now.Hour % 12) + DateTime.Now.Minute / 60f) / 12f * 360f,
                                angle: (float)(timer.RealTimeLeft.TotalHours % 12) / 12f * 360f,
                                scale: DISC_INITAL_SCALE_HOURS / dividend,
                                centerOffset: 0f
                            );
                        }
                        else
                        {
                            fillPie(
                                color: b,
                                startAngle: 0f,
                                angle: 360f,
                                scale: DISC_INITAL_SCALE_HOURS / dividend,
                                centerOffset: 0f
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
                for (int i = 0; i < Math.Ceiling(timer.TimeLeft.TotalHours); i++)
                {
                    //Create a colored pen based on what hour we are drawing
                    using (Brush b = createBrush[i % createBrush.Length](colors[i % colors.Length]))
                    {
                        if (i == Math.Floor(timer.TimeLeft.TotalHours))
                        {
                            fillPie(
                                color: b,
                                startAngle: (DateTime.Now.Minute + DateTime.Now.Second / 60f) / 60f * 360f,
                                angle: (float)(timer.RealTimeLeft.TotalMinutes % 60) / 60f * 360f,
                                scale: DISC_INITAL_SCALE / dividend,
                                centerOffset: 0f
                            );
                        }
                        else
                        {
                            fillPie(
                                color: b,
                                startAngle: 0f,
                                angle: 360f,
                                scale: DISC_INITAL_SCALE / dividend,
                                centerOffset: 0f
                            );
                        }
                    }

                    dividend += DISC_DIVIDEND_INCREMENT;
                }
            }

            //Draw for seconds (less than 12 minutes)
            if (createLastCountdownEffects && timer.TimeLeft.TotalMinutes < 12 && !(timer.Overtime && timer.StopAtZero))
            {
                using (Pen p = new Pen(colors[10 % colors.Length], 2f) { DashStyle = DashStyle.Dash })
                {
                    drawPie(
                        color: p,
                        startAngle: 0f,
                        angle: (float)timer.RealTimeLeft.TotalMinutes / 12f * 360f,
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
                    length: NUMBER_HOUR_DISTANCE_SCALE * squareArea.Width / 2,
                    angle: CalculateAngle(DateTime.Now.Hour % 12 + DateTime.Now.Minute / 60f, 12) + NUMBER_HOUR_DEGREE_OFFSET
                );

                byte s = (byte)(Math.Min(byte.MaxValue, Math.Max((Globals.PrimaryTimer.TimeLeft.TotalHours - 1), 0) / (1 / 6f) * byte.MaxValue));
                using (Brush brush = new SolidBrush(Color.FromArgb(s, COLOR_NUMBER)))
                {
                    e.Graphics.DrawString(
                        s: Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalHours).ToString(),
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
                    length: NUMBER_MINUTE_DISTANCE_SCALE * squareArea.Width / 2,
                    angle: CalculateAngle(DateTime.Now.Minute + DateTime.Now.Second / 60f, 60) + NUMBER_MINUTE_DEGREE_OFFSET
                );

                //Calculates transparency
                byte s = (byte)(Math.Min(byte.MaxValue, Math.Max((Globals.PrimaryTimer.TimeLeft.TotalMinutes - 1), 0) / 5f * byte.MaxValue));
                using (Brush brush = new SolidBrush(Color.FromArgb(s, COLOR_NUMBER)))
                {
                    e.Graphics.DrawString(
                        s: Globals.PrimaryTimer.TimeLeft.Minutes.ToString("00"),
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
                    length: NUMBER_SECOND_DISTANCE_SCALE * squareArea.Width / 2,
                    angle: CalculateAngle(DateTime.Now.Second + DateTime.Now.Millisecond / 1000f, 60f) + NUMBER_SECOND_DEGREE_OFFSET
                );

                //Calculates transparency
                byte s = (byte)(Globals.PrimaryTimer.RealTimeLeft.Milliseconds / 1000f * 255);
                using (Brush brush = new SolidBrush(Color.FromArgb(s, COLOR_NUMBER)))
                {
                    e.Graphics.DrawString(
                        s: Globals.PrimaryTimer.TimeLeft.Seconds.ToString("00"),
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

            Point localMousePos = PointToClient(MousePosition);
            Point center = new Point(Width / 2, Height / 2);

            //Only change palette if the user clicks the actual clock
            if (GetDistance(localMousePos, center) < squareArea.Width / 2) //Height would work here as well
            {
                Globals.PrimaryTimer.ColorizeTimerBar();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets the point at a particular distance away from an origin in a particular direction (using angle)
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="length"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        protected static PointF GetPointAtAngle(Point origin, float length, float angle)
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
        /// Gets the distance between a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected static double GetDistance(Point a, Point b) 
        {
            return Math.Sqrt((Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2)));
        }

        /// <summary>
        /// Invalidates the control when the countdown ticks
        /// </summary>
        /// <param name="span"></param>
        /// <param name="isOvertime"></param>
        public void OnCountdownTick(TimeSpan span, TimeSpan secondSpan, bool isOvertime)
        {
            Invalidate();
        }
    }
}
