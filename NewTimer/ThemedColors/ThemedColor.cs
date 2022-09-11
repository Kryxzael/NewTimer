
using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.ThemedColors
{
    /// <summary>
    /// Represents a set of colors to use for different Windows UI themes
    /// </summary>
    public struct ThemedColor
    {
        //Try to limit how often we fetch the registry
        private static DateTime _lastRegistryAccessTime;
        private static int _lastRegistryKey;

        /// <summary>
        /// Gets the color to use when the system is in light appearance
        /// </summary>
        public Color Light { get; }

        /// <summary>
        /// Gets the color to use when the system is in dark appearance
        /// </summary>
        public Color Dark { get; }

        /// <summary>
        /// Gets the color to use given current system settings
        /// </summary>
        public Color Current
        {
            get
            {
                if ((DateTime.Now - _lastRegistryAccessTime).Seconds >= 10f)
                {
                    _lastRegistryKey = (int)Registry.GetValue(
                        keyName: "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize",
                        valueName: "AppsUseLightTheme",
                        defaultValue: 0
                    );

                    _lastRegistryAccessTime = DateTime.Now;
                }

                if (_lastRegistryKey == 0)
                    return Dark;

                return Light;
            }
        }

        /// <summary>
        /// Creates a new themed color with the provided light and dark colors
        /// </summary>
        /// <param name="light"></param>
        /// <param name="dark"></param>
        public ThemedColor(Color light, Color dark)
        {
            Light = light;
            Dark = dark;
        }

        /// <summary>
        /// Gets the color to use given current system settings. Shorthand for <see cref="Current"/>
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator Color(ThemedColor a)
        {
            return a.Current;
        }
    }
}
