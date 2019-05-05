using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.Schemes
{
    public class SchemeSingle : ColorScheme
    {
        public override string Name { get; }
        public Color BaseColor { get; }
        private float BaseColorHue { get; }

        public SchemeSingle(string name, Color baseColor)
        {
            Name = name;
            BaseColor = baseColor;
            BaseColorHue = BaseColor.GetHue();
        }

        public override Color GenerateOne(Random rng)
        {
            float v = rng.Next(50, 150) / 100f;
            float s = BaseColor.GetSaturation() == 0f ? 0f : rng.Next(50, 100) / 100f;

            return ColorConverter.HsvToRgb(BaseColorHue, s, v);
        }

        public override IEnumerable<Color> GenerateMany(int count, Random rng)
        {
            float lastBrgt = 0f;
            for (int i = 0; i < count; i++)
            {
                Color c;
                int attempts = 0;
                do
                {
                    c = GenerateOne(rng);
                } while (Math.Abs(c.GetBrightness() - lastBrgt) < 0.2f && ++attempts < 20);

                lastBrgt = c.GetBrightness();
                yield return c;
            }
            
        }
    }
}
