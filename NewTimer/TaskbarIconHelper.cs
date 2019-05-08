using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer
{
    public static class TaskbarHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private extern static bool DestroyIcon(IntPtr handle);

        /// <summary>
        /// Represents the states a taskbar icon's progress can have
        /// </summary>
        public enum TaskbarStates
        {
            /// <summary>
            /// Disables the taskbar icon progress
            /// </summary>
            NoProgress = 0,

            /// <summary>
            /// Makes the taskbar icon progress pulse green, indicating an unquantifiable progress is being made
            /// </summary>
            Indeterminate = 0x1,

            /// <summary>
            /// Makes the taskbar icon progress green, indicating an active process
            /// </summary>
            Normal = 0x2,

            /// <summary>
            /// Makes the taskbar icon progress red, indicating an error or confict
            /// </summary>
            Error = 0x4,

            /// <summary>
            /// Makes the taskbar icon progress yellow, indicating a deliberate pause
            /// </summary>
            Paused = 0x8
        }

        [ComImportAttribute()]
        [GuidAttribute("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        private interface ITaskbarList3
        {
            // ITaskbarList
            [PreserveSig]
            void HrInit();
            [PreserveSig]
            void AddTab(IntPtr hwnd);
            [PreserveSig]
            void DeleteTab(IntPtr hwnd);
            [PreserveSig]
            void ActivateTab(IntPtr hwnd);
            [PreserveSig]
            void SetActiveAlt(IntPtr hwnd);

            // ITaskbarList2
            [PreserveSig]
            void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

            // ITaskbarList3
            [PreserveSig]
            void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);
            [PreserveSig]
            void SetProgressState(IntPtr hwnd, TaskbarStates state);
        }

        [GuidAttribute("56FDF344-FD6D-11d0-958A-006097C9A090")]
        [ClassInterfaceAttribute(ClassInterfaceType.None)]
        [ComImportAttribute()]
        private class TaskbarInstance
        {
        }

        private static ITaskbarList3 taskbarInstance = (ITaskbarList3)new TaskbarInstance();
        private static bool taskbarSupported = Environment.OSVersion.Version >= new Version(6, 1);


        /// <summary>
        /// Set the color of the progress indicator behind a window's icon on the taskbar
        /// </summary>
        /// <param name="windowHandle">Pointer to the handle of the window you want to update</param>
        /// <param name="taskbarState">The state you want to update to</param>
        public static void SetState(IntPtr windowHandle, TaskbarStates taskbarState)
        {
            if (taskbarSupported) taskbarInstance.SetProgressState(windowHandle, taskbarState);
        }

        /// <summary>
        /// Set the percentage of the progress indicator behind a window's icon on the taskbar
        /// </summary>
        /// <param name="windowHandle">Pointer to the handle of the window you want to update</param>
        /// <param name="progressValue">The progress to set</param>
        /// <param name="progressMax">The maximum progress of the window</param>
        public static void SetValue(IntPtr windowHandle, double progressValue, double progressMax)
        {
            if (taskbarSupported) taskbarInstance.SetProgressValue(windowHandle, (ulong)progressValue, (ulong)progressMax);
        }

        /// <summary>
        /// Creates a pie chart-esc icon and returns it as a bitmap
        /// </summary>
        /// <param name="bounds">The area to draw the pie into</param>
        /// <param name="primary">The color to use for odd-numbered loops</param>
        /// <param name="secondary">The color to use for even-numbered loops</param>
        /// <param name="primaryBG">The color used for the background disk of odd-numbered loops</param>
        /// <param name="secondaryBG">The color used for the background disk of even-numbered loops</param>
        /// <param name="value">The amount of full turns the pie has done</param>
        /// <param name="startAngle">The starting angle of the pie</param>
        /// <param name="ccw">Does the pie go counter-clockwise?</param>
        /// <returns></returns>
        public static Bitmap CreatePie(Rectangle bounds, Color primary, Color secondary, Color primaryBG, Color secondaryBG, float value, int startAngle, bool ccw)
        {
            //Create bitmap to store image in
            Bitmap bmp = new Bitmap(bounds.Width, bounds.Height);

            //Calculate the area of the smaller background disk
            RectangleF smallPieRect = RectangleF.Inflate(bounds, -0.2f * bounds.Width, -0.2f * bounds.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            using (Brush b1 = new SolidBrush(primary))
            using (Brush b2 = new SolidBrush(secondary))
            using (Brush b1bg = new SolidBrush(primaryBG))
            using (Brush b2bg = new SolidBrush(secondaryBG))
            using (Pen pen = new Pen(Color.Black, 3))
            {
                //Draw the background disk if the value is greater than one
                if (value >= 1)
                {
                    g.FillEllipse(
                        brush: Math.Floor(value) % 2 == 0 ? b2bg : b1bg, //Alternating between two colors
                        rect: smallPieRect
                    );

                    g.DrawEllipse(pen, smallPieRect);
                }

                //Draw the pie's fill
                g.FillPie(
                    brush: Math.Floor(value) % 2 == 0 ? b2 : b1, //Alternating between two colors
                    rect: bounds,
                    startAngle: startAngle,
                    sweepAngle: (ccw ? -1 : 1) * Config.GetDecimals(value, 3) / 1000f * 360
                );

                //Draw the pie's border
                g.DrawPie(
                    pen: pen, 
                    rect: Rectangle.Inflate(bounds, -1, -1), 
                    startAngle: startAngle, 
                    sweepAngle: (ccw ? -1 : 1) * Config.GetDecimals(value, 3) / 1000f * 360
                );

            }

            //Return result
            return bmp;
        }

        /// <summary>
        /// The last icon created
        /// </summary>
        private static Icon _lastIcon;

        /// <summary>
        /// Creates an icon object from a bitmap
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Icon CreateIconFromBitmap(Bitmap src)
        {
            //Destroys buffered icon because Icon.FromHandle() creates an icon in unamanged memory, 
            //meaning that garbage collection will not occur on it.
            //we have to do this manualy
            DisposeIcon();

            //Gets the new icon
            _lastIcon = Icon.FromHandle(src.GetHicon());
            return _lastIcon;
        }

        /// <summary>
        /// Disposes the last icon
        /// </summary>
        public static void DisposeIcon()
        {
            if (_lastIcon != null)
            {
                DestroyIcon(_lastIcon.Handle);
            }
        }
    }
}
