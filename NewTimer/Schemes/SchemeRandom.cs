using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.Schemes
{
    /// <summary>
    /// Factory class for generating random colors
    /// </summary>
    public class SchemeRandom : ColorScheme
    {
        public override string Name { get => "Random"; }

        /// <summary>
        /// Generates a random color
        /// </summary>
        /// <returns></returns>
        public override Color GenerateOne(Random rng)
        {
            while (true)
            {
                Color clr = Color.FromArgb(RandByte(rng), RandByte(rng), RandByte(rng));

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
        public override IEnumerable<Color> GenerateMany(int count, Random rng)
        {
            Color[] _ = new Color[count];

            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    Color clr = GenerateOne(rng);

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

        private IEnumerable<Color> _previewColors;
        public override IEnumerable<Color> GenerateForPreview(int count, Random rng)
        {
            if (_previewColors == null || _previewColors.Count() != count)
            {
                _previewColors = GenerateMany(count, rng);
            }

            return _previewColors;
        }

        /// <summary>
        /// Returns a random number between 0 and 255
        /// </summary>
        /// <returns></returns>
        private static int RandByte(Random rng) => rng.Next(0x100);
    }
}
