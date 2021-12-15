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
using NewTimer.Forms;

namespace NewTimer
{
    /// <summary>
    /// Stores configuration and other global settings
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// Will the time selector use 24-hour time?
        /// </summary>
        public static bool Use24HourSelector { get; set; } = false;

        /// <summary>
        /// Gets the current primary timer. The primary and secondary timers can be swaped with SwapTimers()
        /// </summary>
        public static TimerConfig PrimaryTimer { get; private set; } = new TimerConfig();

        /// <summary>
        /// Gets the current secondary timer. The primary and secondary timers can be swaped with SwapTimers()
        /// </summary>

        public static TimerConfig SecondaryTimer { get; private set; } = new TimerConfig();

        /*
         * Constants
         */

        /// <summary>
        /// The random number generator that is used for all randomness
        /// </summary>
        public static readonly Random MasterRandom = new Random();

        /// <summary>
        /// The color that will be used as the text color for the whole application
        /// </summary>
        public static Color GlobalForeColor { get; } = ColorTranslator.FromHtml("#dddddd");

        /// <summary>
        /// The color that will be used as the background color for the whole application
        /// </summary>
        public static Color GlobalBackColor { get; } = ColorTranslator.FromHtml("#111111");

        /// <summary>
        /// The color that will be used as the background color for the whole application
        /// </summary>
        public static Color GlobalFreeModeBackColor { get; } = ColorTranslator.FromHtml("#222222");

        /// <summary>
        /// The color that will be used as the background color for the whole application
        /// </summary>
        public static Color GlobalOvertimeColor { get; } = ColorTranslator.FromHtml("#330000");

        /// <summary>
        /// The color that will be used to gray out text
        /// </summary>
        public static Color GlobalGrayedColor { get; } = ColorTranslator.FromHtml("#333333");

        /*
         * Translucencies
         */

        /// <summary>
        /// The opacity of the main window under normal operation 
        /// </summary>
        public static float OPACITY_NORMAL = 1f;

        /// <summary>
        /// The opacity of the main window when translucency mode is enabled
        /// </summary>
        public static float OPACITY_TRANSLUCENT = 0.25f;

        /// <summary>
        /// Gets the available color schemes
        /// </summary>
        public static ColorScheme[] ColorSchemes { get; } =
        {
            //Random
            new Schemes.SchemeRandom(),

            //Single colored
            new Schemes.SchemeSingle("Reds", Color.Red),
            new Schemes.SchemeSingle("Oranges", Color.OrangeRed),
            new Schemes.SchemeSingle("Yellows", Color.Yellow),
            new Schemes.SchemeSingle("Greens", Color.LimeGreen),
            new Schemes.SchemeSingle("Blues", Color.Blue),
            new Schemes.SchemeSingle("Aqua", Color.Aqua),
            new Schemes.SchemeSingle("Purples", Color.Purple),
            new Schemes.SchemeSingle("Grays", Color.White),

            //Gauges
            new Schemes.SchemeCustom("Positive", Schemes.SchemeCustom.LoopType.Ceiling, 
                /*  1s */ Color.Lime,
                /* 10s */ Color.LimeGreen,
                /*  1m */ Color.Green,
                /*  5m */ Color.GreenYellow,
                /* 15m */ Color.Gold,
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

        /// <summary>
        /// Starts the timer
        /// </summary>
        /// <param name="target">Target time</param>
        /// <param name="closingForm">Form that will be closed, this should be the settings form</param>
        public static void StartTimer(DateTime target, ColorScheme colorScheme, bool stopAtZero, Form closingForm = null)
        {
            //Set target and color scheme
            PrimaryTimer.Target      = target;
            PrimaryTimer.StartTime   = DateTime.Now;
            PrimaryTimer.ColorScheme = colorScheme;
            PrimaryTimer.StopAtZero  = stopAtZero;
            PrimaryTimer.InFreeMode  = false;

            SecondaryTimer.Target = target;
            SecondaryTimer.StartTime = DateTime.Now;
            SecondaryTimer.ColorScheme = ColorSchemes[0];
            SecondaryTimer.StopAtZero = false;
            SecondaryTimer.InFreeMode = true;

            //Close the closing form with a result of OK so that the application doesn't exit
            if (closingForm != null)
            {
                closingForm.DialogResult = DialogResult.OK;
                closingForm.Close();
            }

            //Colorize the bar
            PrimaryTimer.ColorizeTimerBar();
            SecondaryTimer.ColorizeTimerBar();

            //Show main form
            new TimerFormBase().Show();
        }

        /// <summary>
        /// Swaps the positions of the primary and secondary timers
        /// </summary>
        public static void SwapTimers()
        {
            TimerConfig swap = PrimaryTimer;
            PrimaryTimer = SecondaryTimer;
            SecondaryTimer = swap;
        }

        /// <summary>
        /// Gets the decimals of a number and returns it as a integer. Eg: 1.25 => 25
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="significantDigits">The amount of digits to get</param>
        /// <returns></returns>
        public static int GetDecimals(double value, int significantDigits)
        {
            return (int)((value - Math.Truncate(value)) * Math.Pow(10, significantDigits));
        }
    }
}
