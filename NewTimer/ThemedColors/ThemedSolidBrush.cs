
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.ThemedColors
{
    /// <summary>
    /// Provides a SolidBrush instance of a themed color
    /// </summary>
    public class ThemedSolidBrush : ThemedColorContainer<SolidBrush>, IDisposable
    {
        /// <summary>
        /// Creates a new themed color brush with the provided color
        /// </summary>
        /// <param name="color"></param>
        public ThemedSolidBrush(ThemedColor color) : base(color)
        { }

        /// <summary>
        /// Creates a new themed color brush with the provided colors
        /// </summary>
        /// <param name="color"></param>
        public ThemedSolidBrush(Color light, Color dark) : base(new ThemedColor(light, dark))
        { }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        protected override SolidBrush CreateNewInstance()
        {
            return new SolidBrush(Color);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        protected override void OnDestroyOldInstance(SolidBrush oldInstance)
        {
            oldInstance.Dispose();
        }

        ~ThemedSolidBrush()
        {
            Dispose();
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public void Dispose()
        {
            Current.Dispose();
        }
    }
}
