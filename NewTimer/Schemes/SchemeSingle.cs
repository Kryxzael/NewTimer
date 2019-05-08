using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.Schemes
{
    /// <summary>
    /// A color scheme that produces variations of the same hue
    /// </summary>
    public class SchemeSingle : ColorScheme
    {
        public override string Name { get; }

        //The color to use as a base
        public Color BaseColor { get; }

        //The hue of the base color
        private float BaseColorHue { get; }

        /// <summary>
        /// Creates a new color scheme
        /// </summary>
        /// <param name="name">Name of scheme</param>
        /// <param name="baseColor">Base color of scheme</param>
        public SchemeSingle(string name, Color baseColor)
        {
            Name = name;
            BaseColor = baseColor;
            BaseColorHue = BaseColor.GetHue();
        }

        public override Color GenerateOne(Random rng)
        {
            //Produces a value value between 50% and 150%
            float v = rng.Next(50, 150) / 100f;

            //Produces a saturation value between 50% and 100% (Or 0% if the base color is grayscale)
            float s = BaseColor.GetSaturation() == 0f ? 0f : rng.Next(50, 100) / 100f;

            //Apply color transformation and return result
            return ColorConverter.HsvToRgb(BaseColorHue, s, v);
        }

        public override IEnumerable<Color> GenerateMany(int count, Random rng)
        {
            //Store the brightness of the last color
            float lastBrgt = 0f;
            for (int i = 0; i < count; i++)
            {
                Color c;
                int attempts = 0;

                //Generate a unique saturation/value pair. Capped at 20 attempts to reduce risk of long loops
                do
                {
                    //Create candidate
                    c = GenerateOne(rng);
                } while (Math.Abs(c.GetBrightness() - lastBrgt) < 0.2f && ++attempts < 20);

                //Store last brightness
                lastBrgt = c.GetBrightness();
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
    }
}
