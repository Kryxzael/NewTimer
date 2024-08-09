using Bars;

using CleanNodeTree;

using NewTimer.FormParts;
using NewTimer.Properties;

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
        private bool _paused;
        private int? _freeNumber;
        private DateTime _target = new DateTime(2017, 4, 7, 15, 05, 0); //Keeping this lol

        /// <summary>
        /// Gets or sets the time-left value the timer had the last time it was paused. If the timer is unpaused, this value is irrelevant
        /// </summary>
        private TimeSpan LastPauseRealTimeLeft { get; set; }

        /// <summary>
        /// Gets or sets whether the timer is not currently tracking a target time. Also referred to as 'idle'
        /// </summary>
        public bool InFreeMode { get; set; } = true;

        /// <summary>
        /// Gets whether the last time-set input of this timer a duration input
        /// </summary>
        public bool LastInputWasDuration { get; set; }

        /// <summary>
        /// Gets the time the timer was started at
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the time the timer targets
        /// </summary>
        public DateTime Target
        {
            get
            {
                if (Paused)
                    return DateTime.Now.Add(LastPauseRealTimeLeft);

                return _target;
            }
            set
            {
                _target = value;
                LastPauseRealTimeLeft = _target - DateTime.Now;
            }
        }

        /// <summary>
        /// A free number the user can adjust themselves as a counter
        /// </summary>
        public int? FreeNumber
        {
            get
            {
                return _freeNumber;
            }
            set
            {
                _freeNumber = Math.Max(Math.Min(value ?? 0, 99_000_000), 0);
            }
        }

        /// <summary>
        /// Copies all color and color-scheme information from the provided timer
        /// </summary>
        /// <param name="secondaryTimer"></param>
        public void CopyColorInfoFrom(TimerConfig secondaryTimer, bool invert = false)
        {
            foreach (TimeSpan i in BarSettings.Keys)
            {
                BarSettings destSetting = BarSettings[i];
                BarSettings srcSetting  = secondaryTimer.BarSettings[i];

                destSetting.FillColor     = srcSetting.FillColor;
                destSetting.OverflowColor = srcSetting.OverflowColor;

                if (invert) 
                {
                    destSetting.FillColor     = invertColor(destSetting.FillColor);
                    destSetting.OverflowColor = invertColor(destSetting.OverflowColor);
                }
            }

            Array.Copy(secondaryTimer.AnalogColors, AnalogColors, AnalogColors.Length);
            MicroViewColor = secondaryTimer.MicroViewColor;

            if (invert)
            {
                for (int i = 0; i < AnalogColors.Length; i++)
                    AnalogColors[i] = invertColor(AnalogColors[i]);

                MicroViewColor = invertColor(MicroViewColor);
            }

            Color invertColor(Color c)
            {
                return Color.FromArgb(c.A, 0xFF - c.R, 0xFF - c.G, 0xFF - c.B);
            }
        }

        /// <summary>
        /// The color scheme that is selected. This value is set when the timer starts
        /// </summary>
        public ColorScheme ColorScheme { get; set; }

        /// <summary>
        /// Gets or sets whether this timer has been configured to never show its minute disk in analog mode (showing hour disks in its stead)
        /// </summary>
        public bool HybridDiskMode { get; set; }

        /// <summary>
        /// Gets or sets the micro-view unit to use for the timer
        /// </summary>
        public MicroView.MicroViewUnitSelector MicroViewUnit { get; set; } = MicroView.MicroViewUnitSelector.MostAccurate;

        /// <summary>
        /// Gets whether the timer is currently frozen
        /// </summary>
        public bool Paused 
        {
            get => _paused;
            set
            {
                if (value && !Paused)
                    LastPauseRealTimeLeft = RealTimeLeft;

                else if (!value && Paused)
                    Target = DateTime.Now.Add(LastPauseRealTimeLeft);


                _paused = value;
            }
        }



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
                TimeSpan output;

                if (InFreeMode)
                    return default;

                if (DateTime.Now > Target)
                {
                    if (StopAtZero)
                        return default;

                    output = DateTime.Now - Target;
                }
                else
                {
                    output = Target - DateTime.Now;
                }

                //Have had occurrences surrounding near-zero TimeLeft values returning values less than zero
                //Probably a rounding imprecision. Trying to mitigate this here
                if (output < default(TimeSpan))
                    return default;

                return output;
                
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
        /// Keeps the colors to use for the analog clocks
        /// </summary>
        public Color[] AnalogColors { get; private set; }

        /// <summary>
        /// Keeps the color of the micro-view
        /// </summary>
        public Color MicroViewColor { get; set; }

        /// <summary>
        /// Gets the configuration settings that the time bar will use. The key is the minimum unit time that will be used to apply the settings
        /// </summary>
        public Dictionary<TimeSpan, BarSettings> BarSettings = new Dictionary<TimeSpan, BarSettings>()
        {
            /*  1y */ { new TimeSpan(365, 0, 0, 0), CreateBarSettings(1, 1)},

            /* 30d */ { new TimeSpan(30, 0, 0, 0), CreateBarSettings(30, 30) },
            /*  7d */ { new TimeSpan(7, 0, 0, 0), CreateBarSettings(7, 7) },
            /*  1d */ { new TimeSpan(1, 0, 0, 0), CreateBarSettings(1, 1) },

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
        public void Recolorize()
        {
            //Get colors
            Color[] color = ColorScheme.GenerateMany(BarSettings.Count + 1, Globals.MasterRandom).ToArray();
            AnalogColors = color;
            MicroViewColor = ColorScheme.GenerateOne(Globals.MasterRandom);

            //Apply colors to bar settings
            for (int i = 0; i < BarSettings.Count; i++)
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

        /// <summary>
        /// Serializes the state of the timer
        /// </summary>
        /// <returns></returns>
        public HierarchyNode Serialize()
        {
            HierarchyNode root = new HierarchyNode("Timer");
            root.Add("Idle", InFreeMode);

            if (LastInputWasDuration)
                root.Add(TimeSpanToNode(RealTimeLeft, "Duration"));

            else
                root.Add(DateTimeToNode(Target, "Target"));

            root.Add(DateTimeToNode(StartTime, "StartTime"));
            root.Add("StopAtZero", StopAtZero);

            HierarchyNode colorNode = root.Add("ColorInfo");
            colorNode.Add("ColorScheme", ColorScheme.Name);
            colorNode.Add("AnalogColors").AddRange(AnalogColors.Select(i => ColorTranslator.ToHtml(i)));
            colorNode.Add("MicroViewColor", ColorTranslator.ToHtml(MicroViewColor));

            HierarchyNode barColorNode = colorNode.Add("BarColorInfo");
            barColorNode.Add("Fill").AddRange(BarSettings.Values.Select(i => ColorTranslator.ToHtml(i.FillColor)));
            barColorNode.Add("Overflow").AddRange(BarSettings.Values.Select(i => ColorTranslator.ToHtml(i.OverflowColor)));

            root.Add("MicroViewUnit", MicroViewUnit.ID);
            root.Add("UseArrows", HybridDiskMode);

            return root;

            HierarchyNode DateTimeToNode(DateTime dt, string nodeName)
            {
                HierarchyNode dateTimeNode = new HierarchyNode(nodeName);
                dateTimeNode.Add("Year", dt.Year);
                dateTimeNode.Add("Month", dt.Month);
                dateTimeNode.Add("Day", dt.Day);
                dateTimeNode.Add("Hour", dt.Hour);
                dateTimeNode.Add("Minute", dt.Minute);
                dateTimeNode.Add("Second", dt.Second);
                dateTimeNode.Add("Milli", dt.Millisecond);

                return dateTimeNode;

            }

            HierarchyNode TimeSpanToNode(TimeSpan ts, string nodeName)
            {
                HierarchyNode timeSpanNode = new HierarchyNode(nodeName);
                timeSpanNode.Add("Days", ts.Days);
                timeSpanNode.Add("Hours", ts.Hours);
                timeSpanNode.Add("Minutes", ts.Minutes);
                timeSpanNode.Add("Seconds", ts.Seconds);
                timeSpanNode.Add("Millis", ts.Milliseconds);

                return timeSpanNode;
            }
        }

        /// <summary>
        /// Deserializes a hierarchy node and populates the timer config with its values
        /// </summary>
        /// <param name="root"></param>
        public void Deserialize(HierarchyNode root)
        {
            InFreeMode = root["Idle"].Boolean;

            if (root.HasChild("Target"))
            {
                Paused = false;
                Target = nodeToDateTime(root["Target"]);
                LastInputWasDuration = false;
            }
            else if (root.HasChild("Duration"))
            {
                Paused = true;
                Target = DateTime.Now + nodeToTimeSpan(root["Duration"]);
                LastInputWasDuration = true;
            }

            StartTime = nodeToDateTime(root["StartTime"]);
            StopAtZero = root["StopAtZero"].Boolean;

            ColorScheme = Globals.ColorSchemes.SingleOrDefault(i => i.Name == root["ColorInfo;ColorScheme"].String) ?? ColorScheme;
            AnalogColors = root["ColorInfo;AnalogColors"].Children.Select(i => ColorTranslator.FromHtml(i.Name)).ToArray();
            MicroViewColor = ColorTranslator.FromHtml(root["ColorInfo;MicroViewColor"].String);

            {
                for (int i = 0; i < BarSettings.Count; i++)
                {
                    BarSettings.Values.ElementAt(i).FillColor = ColorTranslator.FromHtml(root["ColorInfo;BarColorInfo;Fill"].Children[i].Name);
                    BarSettings.Values.ElementAt(i).OverflowColor = ColorTranslator.FromHtml(root["ColorInfo;BarColorInfo;Overflow"].Children[i].Name);
                }
            }
            

            MicroViewUnit = MicroView.MicroViewUnitSelector.All.Single(i => i.ID == root["MicroViewUnit"].String);
            HybridDiskMode = root["UseArrows"].Boolean;

            DateTime nodeToDateTime(HierarchyNode node)
            {
                return new DateTime(
                    node["Year"].Int, 
                    node["Month"].Int, 
                    node["Day"].Int, 
                    node["Hour"].Int, 
                    node["Minute"].Int, 
                    node["Second"].Int, 
                    node["Milli"].Int
                );
            }

            TimeSpan nodeToTimeSpan(HierarchyNode node)
            {
                return new TimeSpan(
                    node["Days"].Int,
                    node["Hours"].Int,
                    node["Minutes"].Int,
                    node["Seconds"].Int,
                    node["Millis"].Int
                );
            }
        }
    }
}
