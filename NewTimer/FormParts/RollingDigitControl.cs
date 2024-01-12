using NewTimer.ThemedColors;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.FormParts
{
    /// <summary>
    /// Shows a number as an analog rolling digit
    /// </summary>
    public class RollingDigitControl : UserControl, ICountdown
    {
        //Omegahack
        private static ThemedColor _theme = new ThemedColor(Color.White, Color.Black);

        private DateTimeDigit _digit;

        /// <summary>
        /// Gets or sets the value to display
        /// </summary>
        [Browsable(false)]
        public Func<TimeSpan, double> GetValue { get; set; } = _ => 0;

        /// <summary>
        /// Gets whether the secondary timer should be tracked instead of the primary
        /// </summary>
        public bool TrackSecondaryTimer { get; set; }

        /// <summary>
        /// Gets or sets the style of the digit
        /// </summary>
        public DateTimeDigit Digit
        {
            get => _digit;
            set
            {
                _digit = value;
                Invalidate();
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public RollingDigitControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            const int RESOLUTION = 64;

            base.OnPaint(e);

            using (Bitmap baseImage = GetBaseImage())
            using (Bitmap unscaled = new Bitmap(RESOLUTION, RESOLUTION))
            {
                using (Graphics unscaledGraphics = Graphics.FromImage(unscaled))
                {
                    double value;

                    if ((!TrackSecondaryTimer && Globals.PrimaryTimer.InFreeMode) || (TrackSecondaryTimer && Globals.SecondaryTimer.InFreeMode))
                        value = GetValue(DateTime.Now.TimeOfDay);

                    else if (!TrackSecondaryTimer)
                        value = GetValue(Globals.PrimaryTimer.TimeLeft);

                    else
                        value = GetValue(Globals.SecondaryTimer.TimeLeft);

                    //The time between 2x and 0x is smaller than 10. So we need to speed it up
                    if (Digit == DateTimeDigit.HourTens && value >= 2)
                        value = 2 + (value - 2) * 10 / 4f;


                    unscaledGraphics.DrawImageUnscaledAndClipped(
                        image: baseImage,
                        rect: new Rectangle(
                            x: 0,
                            y: (int)(-value * RESOLUTION),
                            width: RESOLUTION,
                            height: baseImage.Height
                        )
                    );
                }

                e.Graphics.DrawImage(unscaled, new Rectangle(default, Size));
            }
        }

        /// <summary>
        /// Gets the full texture used for the digit in the provided digit mode
        /// </summary>
        /// <param name="digit"></param>
        /// <returns></returns>
        private Bitmap GetBaseImage()
        {
            bool dark = _theme.Current == _theme.Dark;

            switch (Digit)
            {
                case DateTimeDigit.HourTens:
                    return dark ? Properties.Resources.from0to2dark : Properties.Resources.from0to2;

                case DateTimeDigit.HourUnits:
                    if ((TrackSecondaryTimer ? Globals.SecondaryTimer : Globals.PrimaryTimer).TimeLeft.Hours >= 20)
                        return dark ? Properties.Resources.from0to3dark : Properties.Resources.from0to3;

                    return dark ? Properties.Resources.from0to9dark : Properties.Resources.from0to9;

                case DateTimeDigit.SecondTens:
                case DateTimeDigit.MinuteTens:
                    return dark ? Properties.Resources.from0to6dark : Properties.Resources.from0to6;

                default:
                    return dark ? Properties.Resources.from0to9dark : Properties.Resources.from0to9;
            }
        }

        public void OnCountdownTick(TimeSpan span, TimeSpan secondarySpan, bool isOvertime)
        {
            Invalidate();
        }

        /// <summary>
        /// Represents a digit on the clock that can be represented
        /// </summary>
        public enum DateTimeDigit
        {
            /// <summary>
            /// The tens digit of the hour
            /// </summary>
            HourTens,

            /// <summary>
            /// The units digit of the hour
            /// </summary>
            HourUnits,

            /// <summary>
            /// The tens digit of the minute
            /// </summary>
            MinuteTens,

            /// <summary>
            /// The units digit of the minute
            /// </summary>
            MinutesUnits,

            /// <summary>
            /// The tens digit of the second
            /// </summary>
            SecondTens,

            /// <summary>
            /// The units digit of the second
            /// </summary>
            SecondsUnits,

            /// <summary>
            /// The hundreds digit of the day
            /// </summary>
            DayHundreds,

            /// <summary>
            /// The tens digit of the day
            /// </summary>
            DayTens,

            /// <summary>
            /// The units digit of the day
            /// </summary>
            DayUnits,
        }
    }
}
