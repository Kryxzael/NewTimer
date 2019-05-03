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
            { new TimeSpan(360, 0, 0, 0), CreateBarSettings(1, 1)}, //1 year

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
            new Schemes.SchemeRandom()
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
                BarSettings.Values.ElementAt(i + 1).FillColor = color[i + 1];
                BarSettings.Values.ElementAt(i + 1).OverflowColor = color[i];
            }
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        /// <param name="target">Target time</param>
        /// <param name="closingForm">Form that will be closed, this should be the settings form</param>
        public static void StartTimer(DateTime target, Form closingForm = null)
        {
            Target = target;

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
