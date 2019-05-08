using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.Schemes
{
    /// <summary>
    /// A color scheme that generates pseudo-random colors
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
                //Generate a candidate and store it and its saturation
                Color clr = Color.FromArgb(RandByte(rng), RandByte(rng), RandByte(rng));
                float sat = clr.GetSaturation();

                //If the saturation is too high or too low or the color is too dark: Discard it
                if (sat > 0.8f || sat < 0.2f || clr.GetBrightness() < 0.2f)
                {
                    continue;
                }

                //Good candidate. Return it
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
            Color lastColor = default(Color);


            for (int i = 0; i < count; i++)
            {
                Color c;

                //Generatea unique random color.
                do
                {
                    c = GenerateOne(rng);
                } while (
                    //First color gets automatic pass
                    lastColor != default(Color) && 

                    //Colors must not be similar in hue, saturation and brigtness
                    Math.Abs(c.GetHue() - lastColor.GetHue()) < 10 && 
                    Math.Abs(c.GetSaturation() - lastColor.GetSaturation()) < 0.1f &&
                    Math.Abs(c.GetBrightness() - lastColor.GetBrightness()) < 0.1f
                );

                lastColor = c;
                yield return c;
            }
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
