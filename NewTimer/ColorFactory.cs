using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer
{
    /// <summary>
    /// Factory class for generating random colors
    /// </summary>
    public static class ColorFactory
    {
        /// <summary>
        /// Random number generator
        /// </summary>
        static Random Random = new Random();

        /// <summary>
        /// Generates a random color
        /// </summary>
        /// <returns></returns>
        public static Color GenerateOne()
        {
            while (true)
            {
                Color clr = Color.FromArgb(RandByte(), RandByte(), RandByte());

                float sat = clr.GetSaturation();

                if (sat > 0.8 || sat < 0.2)
                {
                    continue;
                }

                float brgt = clr.GetBrightness();
                if (brgt < 0.2)
                {
                    continue;
                }

                return clr;
            }
        }

        /// <summary>
        /// Generates a set of random colors where the colors differ from each other
        /// </summary>
        /// <param name="count">The amount of colors to generate</param>
        /// <returns></returns>
        public static Color[] GenerateMany(int count)
        {
            Color[] _ = new Color[count];

            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    Color clr = GenerateOne();

                    float hue = clr.GetHue();
                    foreach (Color o in _)
                    {
                        float localHue = o.GetHue();

                        if (hue > localHue - 10 && hue < localHue + 10)
                        {
                            continue;
                        }
                    }

                    _[i] = clr;
                    break;
                }
            }

            return _;

        }

        /// <summary>
        /// Returns a random number between 0 and 255
        /// </summary>
        /// <returns></returns>
        private static int RandByte()
        {
            int r = Random.Next(0x100);
            return r;
        }

        /// <summary>
        /// Translates the hue of a color by a specific amount of degrees in the color spectre
        /// </summary>
        /// <param name="color"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Color TranslateColor(Color color, int degrees)
        {
            Console.WriteLine(((int)(color.GetHue() * 360) + degrees) % 360);
            return Properties.Resources.Spectre.GetPixel(((int)(color.GetHue() * 360) + degrees) % 360, 0);
        }
    }
}
