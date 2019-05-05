using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.Schemes
{
    public class SchemeGradient : ColorScheme
    {
        public override string Name { get; }

        public Color ColorA { get; set; }
        public Color ColorB { get; set; }

        public int GradientLength { get; set; }

        public SchemeGradient(string name, Color a, Color b, int gradientLength = int.MaxValue)
        {
            ColorA = a;
            ColorB = b;
            GradientLength = gradientLength;
            Name = name;
        }

        public override Color GenerateOne(Random rng)
        {
            /*
             * Keep in mind this will get a random color in the range without any pattern
             */

            return Lerp(ColorA, ColorB, rng.NextDouble());
        }

        public override IEnumerable<Color> GenerateMany(int count, Random rng)
        {
            for (int i = 0; i < count; i++)
            {
                yield return Lerp(ColorA, ColorB, Math.Min(1, (float)i / Math.Min(count, GradientLength)));
            }
        }

        private int Lerp(int a, int b, double t) => (int)(a * (1 - t) + b * t);

        private Color Lerp(Color a, Color b, double t) => Color.FromArgb(
            alpha: Lerp(a.A, b.A, t),
            red: Lerp(a.R, b.R, t),
            green: Lerp(a.G, b.G, t),
            blue: Lerp(a.B, b.B, t)
            );
    }
}
