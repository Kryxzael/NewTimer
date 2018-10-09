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

        public enum TaskbarStates
        {
            NoProgress = 0,
            Indeterminate = 0x1,
            Normal = 0x2,
            Error = 0x4,
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

        public static void SetState(IntPtr windowHandle, TaskbarStates taskbarState)
        {
            if (taskbarSupported) taskbarInstance.SetProgressState(windowHandle, taskbarState);
        }

        public static void SetValue(IntPtr windowHandle, double progressValue, double progressMax)
        {
            if (taskbarSupported) taskbarInstance.SetProgressValue(windowHandle, (ulong)progressValue, (ulong)progressMax);
        }

        public static Bitmap CreatePie(Rectangle bounds, Color primary, Color secondary, Color primaryBG, Color secondaryBG, float value, int startAngle, bool ccw)
        {
            Bitmap bmp = new Bitmap(bounds.Width, bounds.Height);
            RectangleF smallPieRect = RectangleF.Inflate(bounds, -0.2f * bounds.Width, -0.2f * bounds.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            using (Brush b1 = new SolidBrush(primary))
            using (Brush b2 = new SolidBrush(secondary))
            using (Brush b1bg = new SolidBrush(primaryBG))
            using (Brush b2bg = new SolidBrush(secondaryBG))
            using (Pen pen = new Pen(Color.Black, 3))
            {
                if (value >= 1)
                {
                    g.FillEllipse(
                        brush: Math.Floor(value) % 2 == 0 ? b2bg : b1bg,
                        rect: smallPieRect
                    );

                    g.DrawEllipse(pen, smallPieRect);
                }

                g.FillPie(
                    brush: Math.Floor(value) % 2 == 0 ? b2 : b1,
                    rect: bounds,
                    startAngle: startAngle,
                    sweepAngle: (ccw ? -1 : 1) * Config.GetDecimals(value, 3) / 1000f * 360
                );

                g.DrawPie(pen, Rectangle.Inflate(bounds, -1, -1), startAngle, (ccw ? -1 : 1) * Config.GetDecimals(value, 3) / 1000f * 360);

            }

            return bmp;
        }


        private static Icon _lastIcon;
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

        public static void DisposeIcon()
        {
            if (_lastIcon != null)
            {
                DestroyIcon(_lastIcon.Handle);
            }
        }
    }
}
