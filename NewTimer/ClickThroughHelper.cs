using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer
{
    /// <summary>
    /// Contains external functions for making a window click-through
    /// </summary>
    public static class ClickThroughHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        const int GWL_EXSTYLE       =     -20;
        const int WS_EX_LAYERED     = 0x80000;
        const int WS_EX_TRANSPARENT = 0x00020;

        /// <summary>
        /// Sets the click-through-ness of the window
        /// </summary>
        /// <param name="window"></param>
        /// <param name="clickThrough"></param>
        public static void SetClickThrough(Form window, bool clickThrough)
        {
            int exStyles = GetWindowLong(window.Handle, GWL_EXSTYLE);

            if (clickThrough)
                exStyles |= WS_EX_LAYERED | WS_EX_TRANSPARENT;

            else
                exStyles &= ~(WS_EX_LAYERED | WS_EX_TRANSPARENT);

            SetWindowLong(window.Handle, GWL_EXSTYLE, exStyles);
        }
    }
}
