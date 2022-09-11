
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.ThemedColors
{
    /// <summary>
    /// A pen featuring a themed color set
    /// </summary>
    public class ThemedPen : ThemedColorContainer<Pen>, IDisposable
    {
        public float Width { get; }
        public LineCap StartCap { get; }
        public LineCap EndCap { get; }
        public DashStyle DashStyle { get; }

        /// <summary>
        /// Creates a new themed pen with the provided color
        /// </summary>
        /// <param name="color"></param>
        public ThemedPen(ThemedColor color, float width, LineCap startCap, LineCap endCap, DashStyle dashStyle) : base(color)
        {
            Width = width;
            StartCap = startCap;
            EndCap = endCap;
            DashStyle = dashStyle;
        }

        /// <summary>
        /// Creates a new themed pen with the provided colors
        /// </summary>
        /// <param name="color"></param>
        public ThemedPen(Color light, Color dark, float width, LineCap startCap, LineCap endCap, DashStyle dashStyle) : this(new ThemedColor(light, dark), width, startCap, endCap, dashStyle)
        {  }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        protected override Pen CreateNewInstance() => new Pen(Color)
        {
            Width = Width,
            StartCap = StartCap,
            EndCap = EndCap,
            DashStyle = DashStyle
        };

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="oldInstance"></param>
        protected override void OnDestroyOldInstance(Pen oldInstance)
        {
            oldInstance.Dispose();
        }

        ~ThemedPen()
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
