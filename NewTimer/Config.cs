using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bars;

namespace NewTimer
{
    public static class Config
    {
        static PrivateFontCollection privFontColl = new PrivateFontCollection();
        //public static FontFamily DigitalFont { get; }
        //static readonly string tempFontLocation = Path.GetTempPath() + "newtimerfont.tmp";

        //static Config()
        //{
        //    try
        //    {
        //        File.WriteAllBytes(tempFontLocation, Properties.Resources.deffont);
        //        privFontColl.AddFontFile(tempFontLocation);
        //        DigitalFont = privFontColl.Families[0];
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Could not create temp file!\n\rApplication will now be terminated!", "Fatal error!", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //        Application.Exit();
        //    }
        //}

        /// <summary>
        /// The single, centerelized Random Number Generator. Use this for anything random
        /// </summary>
        public static Random MasterRandom = new Random();

        /// <summary>
        /// The color that will be used as the text color for the whole application
        /// </summary>
        public static Color GlobalForeColor { get; set; } = ColorTranslator.FromHtml("#dddddd");

        /// <summary>
        /// The color that will be used as the background color for the whole application
        /// </summary>
        public static Color GlobalBackColor { get; set; } = ColorTranslator.FromHtml("#111111");

        /// <summary>
        /// The color that will be used as the background color for the whole application
        /// </summary>
        public static Color GlobalOvertimeColor { get; set; } = ColorTranslator.FromHtml("#330000");

        /// <summary>
        /// The color that will be used to gray out text
        /// </summary>
        public static Color GlobalGrayedColor { get; set; } = ColorTranslator.FromHtml("#333333");

        /// <summary>
        /// Gets or sets the time the timer targets
        /// </summary>
        public static DateTime Target { get; private set; } = new DateTime(2017, 4, 7, 15, 05, 0);

        public static ColorScheme ColorScheme { get; set; }

        /// <summary>
        /// Gets the configuration settings that the time bar will use. The key is the minimum unit time that will be used to apply the settings
        /// </summary>
        public static Dictionary<TimeSpan, BarSettings> BarSettings = new Dictionary<TimeSpan, BarSettings>()
        {
            { new TimeSpan(365, 0, 0, 0), CreateBarSettings(1, 1)}, //1 year

            { new TimeSpan(30, 0, 0, 0), CreateBarSettings(30, 30) }, //30 days
            { new TimeSpan(7, 0, 0, 0), CreateBarSettings(7, 7) }, //7 days
            { new TimeSpan(1, 0, 0, 0), CreateBarSettings(1, 1) }, //1 day

            { new TimeSpan(12, 0, 0), CreateBarSettings(12, 3) }, //3 hour
            { new TimeSpan(6, 0, 0), CreateBarSettings(6, 2) }, //2 hour

            { new TimeSpan(1, 0, 0), CreateBarSettings(1, 1) }, //1 hour

            { new TimeSpan(0, 30, 0), CreateBarSettings(30, 15) }, //15 minutes
            { new TimeSpan(0, 10, 0), CreateBarSettings(10, 5) }, //5 minutes

            { new TimeSpan(0, 1, 0), CreateBarSettings(1, 1) }, //1 minute
            { new TimeSpan(0, 0, 10), CreateBarSettings(10, 10) }, //10 seconds
            { new TimeSpan(0, 0, 0), CreateBarSettings(1, 1) }, //1 second
        };

        public static ColorScheme[] ColorSchemes { get; } =
        {
            new Schemes.SchemeRandom(),
            new Schemes.SchemeSingle("Reds", Color.Red),
            new Schemes.SchemeSingle("Oranges", Color.OrangeRed),
            new Schemes.SchemeSingle("Yellows", Color.Yellow),
            new Schemes.SchemeSingle("Greens", Color.LimeGreen),
            new Schemes.SchemeSingle("Blues", Color.Blue),
            new Schemes.SchemeSingle("Aqua", Color.Aqua),
            new Schemes.SchemeSingle("Purples", Color.Purple),
            new Schemes.SchemeSingle("Grays", Color.White),

            new Schemes.SchemeCustom("Positive", Schemes.SchemeCustom.LoopType.Ceiling, 
                /*  1s */ Color.Lime,
                /* 10s */ Color.LimeGreen,
                /*  1m */ Color.Green,
                /*  5s */ Color.GreenYellow,
                /* 15s */ Color.Gold,
                /*  1h */ Color.OrangeRed,
                /*  2h */ Color.Red,
                /*  3h */ Color.FromArgb(0xE0, 0, 0),
                /*  1d */ Color.FromArgb(0xC0, 0, 0),
                /*  7d */ Color.FromArgb(0xA0, 0, 0),
                /* 30d */ Color.FromArgb(0x80, 0, 0),
                /*  1y */ Color.FromArgb(0x40, 0, 0),
                /* >1y */ Color.FromArgb(0x20, 0, 0)
            ),

            new Schemes.SchemeCustom("Negative", Schemes.SchemeCustom.LoopType.Ceiling,
                /*  1s */ Color.DarkRed,
                /* 10s */ Color.Red,
                /*  1m */ Color.OrangeRed,
                /*  5m */ Color.Orange,
                /* 15m */ Color.Gold,
                /*  1h */ Color.YellowGreen,
                /*  2h */ Color.Green,
                /*  3h */ Color.LimeGreen,
                /*  1d */ Color.Lime,
                /*  7d */ Color.FromArgb(0x20, 0xFF, 0x20),
                /* 30d */ Color.FromArgb(0x40, 0xFF, 0x40),
                /*  1y */ Color.FromArgb(0x60, 0xFF, 0x60),
                /* >1y */ Color.FromArgb(0x80, 0xFF, 0x80)
            ),

            new Schemes.SchemeCustom("Long Pos", Schemes.SchemeCustom.LoopType.Ceiling,
                /*  1s */ Color.White,
                /* 10s */ Color.FromArgb(0xBF, 0xFF, 0xBF),
                /*  1m */ Color.FromArgb(0x8F, 0xFF, 0x8F),
                /*  5m */ Color.FromArgb(0x4F, 0xFF, 0x4F),
                /* 15m */ Color.Lime,
                /*  1h */ Color.LimeGreen,
                /*  2h */ Color.GreenYellow,
                /*  3h */ Color.Green,
                /*  1d */ Color.Gold,
                /*  7d */ Color.OrangeRed,
                /* 30d */ Color.Red,
                /*  1y */ Color.DarkRed,
                /* >1y */ Color.Black
            ),

            new Schemes.SchemeCustom("Long Neg", Schemes.SchemeCustom.LoopType.Ceiling,
                /*  1s */ Color.Black,
                /* 10s */ Color.FromArgb(0x4F, 0x00, 0x00),
                /*  1m */ Color.FromArgb(0x8F, 0x00, 0x00),
                /*  5m */ Color.FromArgb(0xBF, 0x00, 0x00),
                /* 15m */ Color.Red,
                /*  1h */ Color.FromArgb(0xFF, 0x2F, 0x00),
                /*  2h */ Color.FromArgb(0xFF, 0x4F, 0x00),
                /*  3h */ Color.FromArgb(0xFF, 0x8F, 0x00),
                /*  1d */ Color.FromArgb(0xFF, 0xBF, 0x00),
                /*  7d */ Color.Gold,
                /* 30d */ Color.Green,
                /*  1y */ Color.Lime,
                /* >1y */ Color.White
            )
        };

        private static BarSettings CreateBarSettings(float maxValue, int interval)
        {
            return new BarSettings(maxValue, interval, Color.White, Color.White);
        }

        /// <summary>
        /// Populates the bar control with a random color scheme
        /// </summary>
        internal static void RandomizeColorScheme()
        {
            Color[] color = ColorScheme.GenerateMany(BarSettings.Count, MasterRandom).ToArray();
            for (int i = 0; i < BarSettings.Count - 1; i++)
            {
                BarSettings.Values.ElementAt(BarSettings.Count - (i + 1)).FillColor = color[i];
                BarSettings.Values.ElementAt(BarSettings.Count - (i + 1)).OverflowColor = color[i + 1];
            }
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        /// <param name="target">Target time</param>
        /// <param name="closingForm">Form that will be closed, this should be the settings form</param>
        public static void StartTimer(DateTime target, ColorScheme colorScheme, Form closingForm = null)
        {
            Target = target;
            ColorScheme = colorScheme;

            if (closingForm != null)
            {
                closingForm.DialogResult = DialogResult.OK;
                closingForm.Close();
            }

            RandomizeColorScheme();
            new Forms.Bar.Bar().Show();
        }

        public static TimeSpan GetTimeLeft()
        {
            if (DateTime.Now > Target)
            {
                return DateTime.Now - Target;
            }
            return (Target - DateTime.Now);
        }

        public static TimeSpan GetRealTimeLeft()
        {
            return Target - DateTime.Now;
        }

        public static bool Overtime
        {
            get
            {
                return DateTime.Now > Target;
            }
        }

        public static int GetDecimals(double value, int significantDigits)
        {
            return (int)((value - Math.Truncate(value)) * Math.Pow(10, significantDigits));
        }
    }
}
