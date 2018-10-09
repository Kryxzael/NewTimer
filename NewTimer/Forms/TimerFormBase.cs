using NewTimer.FormParts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.Forms
{
    public abstract partial class TimerFormBase : Form
    {
        Timer _updateTimer = new Timer();

        protected override bool DoubleBuffered { get => true; }

        /// <summary>
        /// Gets the control that will fill the 'full' tab. The control will be fill-docked
        /// </summary>
        /// <returns></returns>
        public abstract Control FullTabContents();

        /// <summary>
        /// Gets the control that will fill the 'days' tab. The control will be fill-docked
        /// </summary>
        /// <returns></returns>
        public abstract Control DaysTabContents();

        /// <summary>
        /// Gets the control that will fill the 'bar' tab. The control will be fill-docked
        /// </summary>
        /// <returns></returns>
        public abstract Control BarTabContents();

        /// <summary>
        /// Gets the name that will be displayed on the 'bar' tab
        /// </summary>
        /// <returns></returns>
        public abstract string GetBarTabName();

        public TimerFormBase()
        {
            BackColor = Config.GlobalBackColor;
            ForeColor = Config.GlobalForeColor;
            InitializeComponent();
            tabs.BackColor = Color.Transparent;

            StartPosition = FormStartPosition.Manual;
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - Size.Width, Screen.PrimaryScreen.WorkingArea.Bottom - Size.Height);

            TopMost = true;

            InitializeCustomControls();
            InitializeAutoLabels();
            SetColors();

            _updateTimer.Interval = 10;
            _updateTimer.Tick += UpdateICountdowns;
        }

        private void InitializeCustomControls()
        {
            Control fullContents = setControlDefaults(FullTabContents(), tabFull);
            Control daysContents = setControlDefaults(DaysTabContents(), tabDaysOnly);
            Control barContents = setControlDefaults(BarTabContents(), tabBarOnly);

            /* local */ Control setControlDefaults(Control ctrl, Control parent)
            {
                ctrl.Parent = parent;                
                ctrl.ForeColor = Config.GlobalForeColor;
                ctrl.Padding = new Padding(0);

                ctrl.Dock = DockStyle.Fill;
                return ctrl;
            }

        }

        private void InitializeAutoLabels()
        {
            TimeOnlyTime.GetText = () => Config.GetRealTimeLeft().ToString("h':'mm':'ss");

            HoursOnlyHour.GetText = () => Math.Floor(Config.GetTimeLeft().TotalHours).ToString();
            HoursOnlyFraction.GetText = () => "." + Config.GetDecimals(Config.GetTimeLeft().TotalHours, 5).ToString("00000");
            HoursOnlyTitle.GetText = () => Math.Abs(Math.Floor(Config.GetTimeLeft().TotalHours)) == 1 ? "hour" : "hours";

            MinutesOnlyMinutes.GetText = () => Math.Floor(Config.GetTimeLeft().TotalMinutes).ToString();
            MinutesOnlyFraction.GetText = () => "." + Config.GetDecimals(Config.GetTimeLeft().TotalMinutes, 3).ToString("000");
            MinutesOnlyTitle.GetText = () => Math.Abs(Math.Floor(Config.GetTimeLeft().TotalMinutes)) == 1 ? "minute" : "minutes";

            SecondsOnlySecond.GetText = () => Math.Floor(Config.GetRealTimeLeft().TotalSeconds).ToString();
            SecondsOnlyTitle.GetText = () => Math.Abs(Math.Floor(Config.GetRealTimeLeft().TotalSeconds)) == 1 ? "second" : "seconds";
        }

        private void SetColors()
        {
            foreach (Control i in tabs.Controls)
            {
                i.BackColor = Config.GlobalBackColor;
                i.ForeColor = Config.GlobalForeColor;
            }
        }

        private int _lastMinuteIconWasCreated = 61; //Used to prevent creating a new taskbar icon every frame
        private void UpdateICountdowns(object sender, EventArgs e)
        {
            recursiveUpdate(this);
            TaskbarUtility.SetProgress(this);
            TaskbarUtility.SetTitle(this);

            if (_lastMinuteIconWasCreated != Config.GetRealTimeLeft().Minutes)
            {
                _lastMinuteIconWasCreated = Config.GetRealTimeLeft().Minutes;

                Bitmap pie = TaskbarHelper.CreatePie(
                    bounds: new Rectangle(0, 0, 64, 64),
                    primary: Properties.Resources.clrUpper.GetPixel(Config.GetTimeLeft().Minutes, 0),
                    secondary: Properties.Resources.clrLower.GetPixel(Config.GetTimeLeft().Minutes, 0),
                    primaryBG: Properties.Resources.clrToGreen.GetPixel(Config.GetTimeLeft().Minutes, 0),
                    secondaryBG: Properties.Resources.clrToRed.GetPixel(Config.GetTimeLeft().Minutes, 0),
                    value: (float)Config.GetTimeLeft().TotalMinutes / 60f,
                    startAngle: (int)Math.Floor(Config.Target.Minute / 60f * 360 - 90),
                    ccw: !Config.Overtime
                );

                Icon = TaskbarHelper.CreateIconFromBitmap(pie);
                pie.Dispose();
            }

            if (Config.Overtime)
            {
                foreach (Control i in tabs.Controls)
                {
                    i.BackColor = Config.GlobalOvertimeColor;
                }
            }

            /* local */ void recursiveUpdate(Control c)
            {
                if (c is ICountdown ic)
                {
                    ic.OnCountdownTick(Config.GetTimeLeft(), Config.Overtime);
                }

                foreach (Control i in c.Controls)
                {
                    recursiveUpdate(i);
                }
            }            
        }

        



        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _updateTimer.Start();
            OnTabSelected(null, null);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Exit();
        }

        private void OnTabSelected(object sender, TabControlEventArgs e)
        {
            if (tabs.SelectedTab == tabFull) //Locks window size if the 'full' tab is selected
            {
                MaximumSize = new Size(335, 310);
                MinimumSize = MaximumSize;
                MaximizeBox = false;

                if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                }

            }
            else
            {
                MaximumSize = new Size();
                MinimumSize = new Size();
                MaximizeBox = true;
            }

            OnResize(new EventArgs());
        }

        ////TODO: Have a look at this function
        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);
        //    if (tabs.SelectedTab == tabDaysOnly || tabs.SelectedTab == tabBarOnly)
        //    {
        //        Size = new Size(Width, (int)(Width * 1.119403));
        //    }
        //}
    }
}
