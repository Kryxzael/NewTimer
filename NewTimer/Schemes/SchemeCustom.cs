using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.Schemes
{
    /// <summary>
    /// Color scheme that gets its colors from a predefined list
    /// </summary>
    public class SchemeCustom : ColorScheme
    {
        public override string Name { get; }

        /// <summary>
        /// The list of colors to use
        /// </summary>
        public List<Color> Colors { get; }

        /// <summary>
        /// The way the scheme loops through its colors
        /// </summary>
        public LoopType Loop { get; set; }

        /// <summary>
        /// The rules for how a custom scheme will handle reaching the end of its color list
        /// </summary>
        public enum LoopType
        {
            //The last color will be repeated indefinitely
            Ceiling,

            //The list will wrap around and start over at the first item
            Sawtooth,

            //The list will reverse and go the other way
            Triangle
        }

        /// <summary>
        /// Creates a new custom color scheme
        /// </summary>
        /// <param name="name">Name of scheme</param>
        /// <param name="looptype">Loop type of scheme</param>
        /// <param name="colors">The colors the scheme will use</param>
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
            //Yield the given amount of colors
            for (int i = 0; i < count; i++)
            {
                //Branch based on loop type
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
            //Creates an abbreviation of the color set whose resolution is determined by the amount of preview items
            for (int i = 0; i < count; i++)
            {
                yield return Colors[(int)((float)i / count * Colors.Count)];
            }
        }
    }
}
