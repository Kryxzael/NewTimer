using Bars;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//This file is a mess. Sorry

namespace NewTimer.FormParts
{
    class OverwatchCircle : UserControl, ICountdown
    {
        private int _interval;
        private float _maxValue;
        private Color _fillColor;
        private Color _overflowColor;
        private float _value;

        [Bindable(false)]
        public Color[] Colors { get; set; } = Config.ColorScheme.GenerateMany(9, Config.MasterRandom).ToArray();

        public OverwatchCircle()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override bool DoubleBuffered
        {
            get
            {
                return true;
            }
        }

        protected virtual string GetStringForSegment(int segmentValue)
        {
            if (Config.TimeLeft.TotalMinutes < 1) return segmentValue + "s";
            else if (Config.TimeLeft.TotalHours < 1) return segmentValue + "m";
            else if (Config.TimeLeft.TotalDays < 1) return segmentValue + "h";
            else if (Config.TimeLeft.TotalDays < 365) return segmentValue + "d";
            else return segmentValue + "y";
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(Config.Overtime ? Config.GlobalOvertimeColor : Config.GlobalBackColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Brush brshBG, brshFG, brshOF, brshTrack;
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle dBounds;
            {
                int wh = Math.Min(e.ClipRectangle.Width, e.ClipRectangle.Height);
                int xy = 0;
                dBounds = new Rectangle(xy, xy, wh, wh);
            }
            Rectangle edgeBounds = new Rectangle(dBounds.X + dBounds.Width / 12, dBounds.Y + dBounds.Height / 12, dBounds.Width - dBounds.Width / 6, dBounds.Height - dBounds.Width / 6);

            brshBG = new SolidBrush(Config.Overtime ? Config.GlobalOvertimeColor : Config.GlobalBackColor);
            brshFG = new SolidBrush(_fillColor);
            brshOF = new SolidBrush(_overflowColor);
            brshTrack = new SolidBrush(Color.FromArgb(0x22, 0x22, 0x22));

            g.FillEllipse(brshTrack, dBounds);
            

            if (_value > 0)
            {
                if (_value > _maxValue)
                {
                    g.FillPie(brshOF, dBounds, -90, _value / RoundUpByInterval(_value, _interval, 1f) * 360f);
                    g.FillPie(brshFG, dBounds, -90, _maxValue / RoundUpByInterval(_value, _interval, 1f) * 360);
                }
                else
                {
                    g.FillPie(brshFG, dBounds, -90, _value / _maxValue * 360);   
                }
            }
            else
            {
                if (Math.Abs(_value) > _maxValue)
                {
                    g.FillEllipse(Brushes.OrangeRed, dBounds);
                    g.FillPie(Brushes.Yellow, dBounds, -90, _maxValue / _value * 360);
                }
                else
                {
                    g.FillPie(Brushes.Yellow, dBounds, -90, _value / _maxValue * 360);
                }
            }

            float localMaxValue = RoundUpByInterval(Math.Max(_maxValue, Math.Abs(_value)), _interval, 1f);

            for (int i = 0; i < localMaxValue; i++)
            {
                if (i % _interval == 0)
                {
                    g.FillPie(brshBG, dBounds, -90.5f + i / localMaxValue * 360, 2);
                }
            }

            g.FillEllipse(brshBG, edgeBounds);

            for (int i = _interval; i < localMaxValue; i++)
            {
                if (i % _interval == 0)
                {
                    g.DrawString(GetStringForSegment(i), SystemFonts.DefaultFont, brshFG, OriginAngleAndDistanceToPoint(new PointF(dBounds.Width / 2 - 10, dBounds.Height / 2), AngleToRadiants(-90.5f + i / localMaxValue * 360), dBounds.Width / 2 - dBounds.Width / 7), new StringFormat() { LineAlignment = StringAlignment.Center });
                }
            }

            brshBG.Dispose();
            brshFG.Dispose();
            brshOF.Dispose();

        }

        float AngleToRadiants(float ang)
        {
            return (float)(ang * Math.PI / 180f);
        }

        PointF OriginAngleAndDistanceToPoint(PointF origin, float rad, float dist)
        {
            return new PointF((float)(origin.X + dist * Math.Cos(rad)), (float)(origin.Y + dist * Math.Sin(rad)));
        }

        protected override void OnClick(EventArgs e)
        {
            Config.ColorizeTimerBar();
        }

        public void OnCountdownTick(TimeSpan span, bool isOvertime)
        {
            _value = TimerBar.GetNewValue(span);

            //Apply the correct bar settings for the current time left
            ApplySettings(Config.BarSettings.First(i => i.Key <= span).Value);

            Refresh();
        }

        /// <summary>
        /// Applies a bar-settings instance to this bar
        /// </summary>
        /// <param name="settings"></param>
        public void ApplySettings(BarSettings settings)
        {
            _interval = settings.Interval;
            _maxValue = settings.MaxValue;
            _fillColor = settings.FillColor;
            _overflowColor = settings.OverflowColor;

            Refresh();

        }

        //This implementation sucks
        private static float RoundUpByInterval(float value, float interval, float smoothTime)
        {
            for (float i = 0;; i += interval)
            {
                if (i > value)
                {
                    float difference = i - value;

                    if (difference < smoothTime && false)
                    {
                        float start = i - interval;
                        float end = i;
                        float lerpBy = (value - start) / (end - start);

                        return start * (1 - lerpBy) + end * lerpBy;
                    }

                    return i;
                }
            }
        }
    }
}
