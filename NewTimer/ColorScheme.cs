using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer
{
    /// <summary>
    /// Represents a color scheme that can be applied to the timer bar
    /// </summary>
    public abstract class ColorScheme
    {
        /// <summary>
        /// Gets the name of the color scheme
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Generates a single color
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public abstract Color GenerateOne(Random rng);

        /// <summary>
        /// Generates a set amount of colors
        /// </summary>
        /// <param name="count"></param>
        /// <param name="rng"></param>
        /// <returns></returns>
        public virtual IEnumerable<Color> GenerateMany(int count, Random rng)
        {
            for (int i = 0; i < count; i++)
            {
                yield return GenerateOne(rng);
            }
        }

        public override string ToString() => Name;
    }
}
