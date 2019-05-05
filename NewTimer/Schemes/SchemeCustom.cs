using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.Schemes
{
    public class SchemeCustom : ColorScheme
    {
        public override string Name { get; }
        public List<Color> Colors { get; }
        public LoopType Loop { get; set; }

        public enum LoopType
        {
            Ceiling,
            Sawtooth,
            Triangle
        }

        public SchemeCustom(string name, LoopType looptype, params Color[] colors)
        {
            Name = name;
            Colors = colors.ToList();
            Loop = looptype;
        }

        public override Color GenerateOne(Random rng)
        {
            /*
             * This function will return a random color from the set. To use the set as intended, use GenerateMany()
             */

            return Colors[rng.Next(Colors.Count)];
        }

        public override IEnumerable<Color> GenerateMany(int count, Random rng)
        {
            for (int i = 0; i < count; i++)
            {
                switch (Loop)
                {
                    case LoopType.Ceiling:
                        yield return Colors[Math.Min(Colors.Count - 1, i)];
                        break;
                    case LoopType.Sawtooth:
                        yield return Colors[i % Colors.Count];
                        break;
                    case LoopType.Triangle:
                        if (Math.Floor((float)i / Colors.Count) % 2 == 0)
                        {
                            yield return Colors[i % Colors.Count];
                        }
                        else
                        {
                            yield return Colors[Colors.Count - (i % Colors.Count) - 1];
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override IEnumerable<Color> GenerateForPreview(int count, Random rng)
        {
            for (int i = 0; i < count; i++)
            {
                yield return Colors[(int)((float)i / count * Colors.Count)];
            }
        }
    }
}
