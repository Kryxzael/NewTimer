using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.FormParts
{
    class OverwatchCircle : UserControl
    {
        [Bindable(false)]
        public Color[] Colors { get; set; } = ColorFactory.GenerateMany(9);

        public OverwatchCircle()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        public override Color ForeColor
        {
            get
            {
                TimeSpan tl = Config.GetTimeLeft();

                if (tl.TotalSeconds < 10)
                {
                    return Color.White;
                }
                else if (tl.TotalMinutes < 1)
                {
                    return Colors[0];
                }
                else if (tl.TotalMinutes < 10)
                {
                    return Colors[1];
                }
                else if (tl.TotalMinutes < 30)
                {
                    return Colors[2];
                }
                else if (tl.TotalMinutes < 60 + 15)
                {
                    return Colors[3];
                }
                else if (tl.TotalDays < 1)
                {
                    return Colors[4];
                }
                else if (tl.TotalDays < 21)
                {
                    return Colors[5];
                }
                else if (tl.TotalDays < 60)
                {
                    return Colors[6];
                }

                return Colors[7];
            }
            set
            {

            }
        }


        public Color OverflowColor
        {
            get
            {
                TimeSpan tl = Config.GetTimeLeft();

                if (tl.TotalSeconds < 10)
                {
                    return Colors[0];
                }
                else if (tl.TotalMinutes < 1)
                {
                    return Colors[1];
                }
                else if (tl.TotalMinutes < 10)
                {
                    return Colors[2];
                }
                else if (tl.TotalMinutes < 30)
                {
                    return Colors[3];
                }
                else if (tl.TotalMinutes < 60 + 15)
                {
                    return Colors[4];
                }
                else if (tl.TotalDays < 1)
                {
                    return Colors[5];
                }
                else if (tl.TotalDays < 21)
                {
                    return Colors[6];
                }
                else if (tl.TotalDays < 60)
                {
                    return Colors[7];
                }

                return Colors[8];
            }
        }

        public float Value
        {
            get
            {
                TimeSpan tl = Config.GetTimeLeft();

                if (tl.TotalMinutes < 1)
                {
                    return (float)Math.Floor(tl.TotalSeconds);
                }
                else if (tl.TotalDays < 1)
                {
                    return (float)tl.TotalMinutes;
                }
                return (float)tl.TotalDays;
            }
        }

        public float MaxValue
        {
            get
            {
                TimeSpan tl = Config.GetTimeLeft();

                if (tl.TotalSeconds < 10)
                {
                    return 1;
                }
                else if (tl.TotalMinutes < 1)
                {
                    return 10;
                }
                else if (tl.TotalMinutes < 10)
                {
                    return 1;
                }
                else if (tl.TotalMinutes < 30)
                {
                    return 10;
                }
                else if (tl.TotalMinutes < 60 + 15)
                {
                    return 30;
                }
                else if (tl.TotalDays < 1)
                {
                    return 60 + 15;
                }
                else if (tl.TotalDays < 21)
                {
                    return 1;
                }
                else if (tl.TotalDays < 60)
                {
                    return 21;
                }

                return 60;
            }
        }

        public int StepValue
        {
            get
            {
                TimeSpan tl = Config.GetTimeLeft();

                if (tl.TotalSeconds < 10)
                {
                    return 1;
                }
                else if (tl.TotalMinutes < 1)
                {
                    return 10;
                }
                else if (tl.TotalMinutes < 10)
                {
                    return 1;
                }
                else if (tl.TotalMinutes < 30)
                {
                    return 5;
                }
                else if (tl.TotalMinutes < 60 + 15)
                {
                    return 15;
                }
                else if (tl.TotalDays < 1)
                {
                    return 60;
                }
                else if (tl.TotalDays < 21)
                {
                    return 1;
                }
                else if (tl.TotalDays < 60)
                {
                    return 7;
                }

                return 30;
            }
        }

        protected override bool DoubleBuffered
        {
            get
            {
                return true;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Brush brshBG, brshFG, brshOF;
            Graphics g = e.Graphics;

            Rectangle dBounds;
            {
                int wh = Math.Min(e.ClipRectangle.Width, e.ClipRectangle.Height);
                int xy = 0;
                dBounds = new Rectangle(xy, xy, wh, wh);
            }
            Rectangle edgeBounds = new Rectangle(dBounds.X + dBounds.Width / 12, dBounds.Y + dBounds.Height / 12, dBounds.Width - dBounds.Width / 6, dBounds.Height - dBounds.Width / 6);

            brshBG = new SolidBrush(BackColor);
            brshFG = new SolidBrush(ForeColor);
            brshOF = new SolidBrush(OverflowColor);

            g.FillEllipse(Brushes.LightGray, dBounds);
            

            if (Value > 0)
            {
                if (Value > MaxValue)
                {
                    g.FillEllipse(brshOF, dBounds);
                    g.FillPie(brshFG, dBounds, -90, MaxValue / Value * 360);
                }
                else
                {
                    g.FillPie(brshFG, dBounds, -90, Value / MaxValue * 360);   
                }
            }
            else
            {
                if (Math.Abs(Value) > MaxValue)
                {
                    g.FillEllipse(Brushes.OrangeRed, dBounds);
                    g.FillPie(Brushes.Yellow, dBounds, -90, MaxValue / Value * 360);
                }
                else
                {
                    g.FillPie(Brushes.Yellow, dBounds, -90, Value / MaxValue * 360);
                }
            }

            float localMaxValue = Math.Max(MaxValue, Math.Abs(Value));

            for (int i = 0; i < localMaxValue; i++)
            {
                if (i % StepValue == 0)
                {
                    g.FillPie(Brushes.Silver, dBounds, -90.5f + i / localMaxValue * 360, 2);
                }
            }

            g.FillEllipse(brshBG, edgeBounds);

            for (int i = StepValue; i < localMaxValue; i++)
            {
                if (i % StepValue == 0)
                {
                    g.DrawString(i.ToString(), SystemFonts.DefaultFont, brshFG, OriginAngleAndDistanceToPoint(new PointF(dBounds.Width / 2 - 10, dBounds.Height / 2), AngleToRadiants(-90.5f + i / localMaxValue * 360), dBounds.Width / 2 - dBounds.Width / 7), new StringFormat() { LineAlignment = StringAlignment.Center });
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
            Colors = ColorFactory.GenerateMany(9);
        }
    }
}
