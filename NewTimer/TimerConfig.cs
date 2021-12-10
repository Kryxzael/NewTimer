using Bars;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer
{
    /// <summary>
    /// Encapsulates all settings of a timer
    /// </summary>
    public class TimerConfig
    {
        /// <summary>
        /// Gets or sets whether the timer is not currently tracking a target time. Also referred to as 'idle'
        /// </summary>
        public bool InFreeMode { get; set; } = true;

        /// <summary>
        /// Gets the time the timer was started at
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the time the timer targets
        /// </summary>
        public DateTime Target { get; set; } = new DateTime(2017, 4, 7, 15, 05, 0); //Keeping this lol

        /// <summary>
        /// The color scheme that is selected. This value is set when the timer starts
        /// </summary>
        public ColorScheme ColorScheme { get; set; }

        /// <summary>
        /// Gets or sets whether the timer should stop when it reaches zero
        /// </summary>
        public bool StopAtZero { get; set; }

        /// <summary>
        /// Gets the time to display on the timer. This is value will start incrementing once the target time has passed
        /// </summary>
        /// <returns></returns>
        public TimeSpan TimeLeft
        {
            get
            {
                if (InFreeMode)
                    return default;

                if (DateTime.Now > Target)
                {
                    if (StopAtZero)
                        return default;

                    return DateTime.Now - Target;
                }


                return Target - DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the real time left until the target time. This value will go negative after the target time
        /// </summary>
        /// <returns></returns>
        public TimeSpan RealTimeLeft
        {
            get
            {
                if (InFreeMode)
                    return default;

                return Target - DateTime.Now;
            }
        }

        /// <summary>
        /// Has the target time passed?
        /// </summary>
        public bool Overtime
        {
            get
            {
                if (InFreeMode)
                    return false;

                return DateTime.Now > Target;
            }

        }

        /// <summary>
        /// Gets the configuration settings that the time bar will use. The key is the minimum unit time that will be used to apply the settings
        /// </summary>
        public Dictionary<TimeSpan, BarSettings> BarSettings = new Dictionary<TimeSpan, BarSettings>()
        {
            /*  1y */ { new TimeSpan(365, 0, 0, 0), CreateBarSettings(1, 1)},

            /* 30d */ { new TimeSpan(30, 0, 0, 0), CreateBarSettings(30, 30) },
            /*  7d */ { new TimeSpan(7, 0, 0, 0), CreateBarSettings(7, 7) },
            /*  1d */ { new TimeSpan(1, 0, 0, 0), CreateBarSettings(1, 1) },

            /*  3h */ { new TimeSpan(12, 0, 0), CreateBarSettings(12, 3) },
            /*  2h */ { new TimeSpan(6, 0, 0), CreateBarSettings(6, 2) },
            /*  1h */ { new TimeSpan(1, 0, 0), CreateBarSettings(1, 1) },

            /* 15m */ { new TimeSpan(0, 30, 0), CreateBarSettings(30, 15) },
            /*  5m */ { new TimeSpan(0, 10, 0), CreateBarSettings(10, 5) },

            /*  1m */ { new TimeSpan(0, 1, 0), CreateBarSettings(1, 1) },
            /* 10s */ { new TimeSpan(0, 0, 10), CreateBarSettings(10, 10) },
            /*  1s */ { new TimeSpan(0, 0, 0), CreateBarSettings(1, 1) }
        };

        /// <summary>
        /// Populates the bar control with a colors from the current color scheme
        /// </summary>
        public void ColorizeTimerBar()
        {
            //Get colors
            Color[] color = ColorScheme.GenerateMany(BarSettings.Count, Globals.MasterRandom).ToArray();

            //Apply colors to bar settings
            for (int i = 0; i < BarSettings.Count - 1; i++)
            {
                //Apply fill color
                BarSettings.Values.ElementAt(BarSettings.Count - (i + 1)).FillColor = color[i];

                //Apply overflow color
                BarSettings.Values.ElementAt(BarSettings.Count - (i + 1)).OverflowColor = color[i + 1];
            }
        }

        /// <summary>
        /// Creates a bar setting object with the given minValue and interval
        /// </summary>
        /// <param name="maxValue"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        private static BarSettings CreateBarSettings(float maxValue, int interval)
        {
            return new BarSettings(maxValue, interval, Color.White, Color.White, 3);
        }
    }
}
