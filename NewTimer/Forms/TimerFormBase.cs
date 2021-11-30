﻿using NewTimer.Commands;
using NewTimer.FormParts;
using NewTimer.Forms.Bar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserConsoleLib;

namespace NewTimer.Forms
{
    /// <summary>
    /// Base class for any timer form
    /// </summary>
    public partial class TimerFormBase : Form
    {
        /// <summary>
        /// The main loop timer
        /// </summary>
        private Timer _updateTimer = new Timer();

        /// <summary>
        /// The last minute a taskbar/statusbar icon was created. Used to prevent creating a new taskbar icon every frame
        /// </summary>
        private int _lastMinuteIconWasCreated = 61;

        /// <summary>
        /// Has the overtime's dark red background color already been applied?
        /// </summary>
        private bool _overtimeBackColorSet = false;

        /// <summary>
        /// The console associated with this window
        /// </summary>
        private UserConsole _console;

        /// <summary>
        /// Gets or sets the text the user is currently typing when the window is selected
        /// </summary>
        private string InvisibleInputText { get; set; } = "";
        private DateTime LastInvisibileInputTextUpdateTime { get; set; }

        protected override bool DoubleBuffered { get => true; }

        public TimerFormBase()
        {
            InitializeComponent();

            //Normalize the colors of the form to comply with global settings
            BackColor = Config.GlobalBackColor;
            ForeColor = Config.GlobalForeColor;
            tabs.BackColor = Color.Transparent;

            //Position window to the bottom right corner of the screen
            const int POS_OFFSET_X = 58;
            const int POS_OFFSET_Y = 9;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(Screen.FromControl(this).WorkingArea.Right - Size.Width + POS_OFFSET_X, Screen.FromControl(this).WorkingArea.Bottom - Size.Height + POS_OFFSET_Y);

            //Set the window to be topmost
            TopMost = true;

            //Various initializations
            InitializeCustomControls();
            InitializeAutoLabels();
            SetColors();
            KeyPreview = true;

            //Initialize the timer
            _updateTimer.Interval = 50;
            _updateTimer.Tick += UpdateICountdowns;
            _updateTimer.Tick += UpdateTaskbarAndIcon;
        }

        /// <summary>
        /// Loads the contents of the 'Full' 'Days' and 'Bar' tabs
        /// </summary>
        private void InitializeCustomControls()
        {
            setControlDefaults(new Bar.FullContents(), tabFull);
            setControlDefaults(new ClockControl(), tabAnalog);
            setControlDefaults(new Circle.FullContents(simpleMode: false), tabCircle);
            setControlDefaults(new Circle.FullContents(simpleMode: true), tabCircleSimple);
            setControlDefaults(new TimerBar(), tabBarOnly);

            //Normalizes settings for the given control. This function returns its input
            /* local */ Control setControlDefaults(Control ctrl, Control parent)
            {
                ctrl.Parent = parent;                
                ctrl.ForeColor = Config.GlobalForeColor;
                ctrl.Padding = new Padding(0);

                ctrl.Dock = DockStyle.Fill;
                return ctrl;
            }

        }

        /// <summary>
        /// Loads the contents of standard autolabels
        /// </summary>
        private void InitializeAutoLabels()
        {
            //'Time' panel
            TimeOnlyTime.GetText = () => Config.RealTimeLeft.ToString("h':'mm':'ss");

            //'Hours' panel
            HoursOnlyHour.GetText = () => Math.Floor(Config.TimeLeft.TotalHours).ToString();
            HoursOnlyFraction.GetText = () => "." + Config.GetDecimals(Config.TimeLeft.TotalHours, 5).ToString("00000");
            HoursOnlyTitle.GetText = () => Math.Abs(Math.Floor(Config.TimeLeft.TotalHours)) == 1 ? "hour" : "hours";

            //'Minutes' panel
            MinutesOnlyMinutes.GetText = () => Math.Floor(Config.TimeLeft.TotalMinutes).ToString();
            MinutesOnlyFraction.GetText = () => "." + Config.GetDecimals(Config.TimeLeft.TotalMinutes, 3).ToString("000");
            MinutesOnlyTitle.GetText = () => Math.Abs(Math.Floor(Config.TimeLeft.TotalMinutes)) == 1 ? "minute" : "minutes";

            //'Seconds' panel
            SecondsOnlySecond.GetText = () => Math.Floor(Config.RealTimeLeft.TotalSeconds).ToString();
            SecondsOnlyTitle.GetText = () => Math.Abs(Math.Floor(Config.RealTimeLeft.TotalSeconds)) == 1 ? "second" : "seconds";
        }

        /// <summary>
        /// Normalizes the colors of all controls
        /// </summary>
        private void SetColors()
        {
            foreach (Control i in tabs.Controls)
            {
                i.BackColor = Config.GlobalBackColor;
                i.ForeColor = Config.GlobalForeColor;
            }
        }

        /// <summary>
        /// Recursively find and update every ICountdown control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateICountdowns(object sender, EventArgs e)
        {
            if (LastInvisibileInputTextUpdateTime != default && (DateTime.Now - LastInvisibileInputTextUpdateTime).TotalSeconds > 2.0)
            {
                InvisibleInputText                = "";
                LastInvisibileInputTextUpdateTime = default;
            }
                

            //Update ICountdowns
            recursiveUpdate(this);

            //Recursively updates every ICountdown control in the given control
            /* local */ void recursiveUpdate(Control c)
            {
                if (c is ICountdown ic)
                {
                    ic.OnCountdownTick(Config.TimeLeft, Config.Overtime);
                }

                foreach (Control i in c.Controls)
                {
                    recursiveUpdate(i);
                }
            }            
        }

        /// <summary>
        /// Regenerates the window icon and set the taskbar status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTaskbarAndIcon(object sender, EventArgs e)
        {
            //Sets the taskbar progress of this window
            TaskbarUtility.SetProgress(this);

            //Sets the title of the window
            TaskbarUtility.SetTitle(this);

            //A new window icon must be created. This happens once per minute
            if (_lastMinuteIconWasCreated != Config.RealTimeLeft.Minutes)
            {
                _lastMinuteIconWasCreated = Config.RealTimeLeft.Minutes;

                //Create new pie
                Bitmap pie = TaskbarHelper.CreatePie(
                    bounds: new Rectangle(0, 0, 64, 64),
                    primary: Properties.Resources.clrUpper.GetPixel(Config.TimeLeft.Minutes, 0),
                    secondary: Properties.Resources.clrLower.GetPixel(Config.TimeLeft.Minutes, 0),
                    primaryBG: Properties.Resources.clrToGreen.GetPixel(Config.TimeLeft.Minutes, 0),
                    secondaryBG: Properties.Resources.clrToRed.GetPixel(Config.TimeLeft.Minutes, 0),
                    value: (float)Config.TimeLeft.TotalMinutes / 60f,
                    startAngle: (int)Math.Floor(Config.Target.Minute / 60f * 360 - 90),
                    ccw: !Config.Overtime
                );

                //Update icon
                Icon = TaskbarHelper.CreateIconFromBitmap(pie);
                pie.Dispose();
            }

            //We have reached overtime. Make background red. This happens only once
            if (Config.Overtime && !_overtimeBackColorSet)
            {
                foreach (Control i in tabs.Controls)
                {
                    i.BackColor = Config.GlobalOvertimeColor;
                }

                _overtimeBackColorSet = true;
            }
            else if (!Config.Overtime && _overtimeBackColorSet)
            {
                foreach (Control i in tabs.Controls)
                {
                    i.BackColor = Config.GlobalBackColor;
                }

                _overtimeBackColorSet = false;
            }
        }

        /// <summary>
        /// Handler: Starts the timer when the window is shown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _updateTimer.Start();
            OnTabSelected(null, null);
        }

        /// <summary>
        /// Handler: Closes the application when the window is closed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Exit();
        }

        /// <summary>
        /// Handler: Handles the tabs being switched
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTabSelected(object sender, TabControlEventArgs e)
        {
            //Locks window size if the 'full' tab is selected
            if (tabs.SelectedTab == tabFull || tabs.SelectedTab == tabCircle)
            {
                MaximumSize = new Size(335, 310);
                MinimumSize = MaximumSize;
                MaximizeBox = false;

                if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                }

            }

            //Unlocks window size for other windows
            else
            {
                MaximumSize = new Size();
                MinimumSize = new Size();
                MaximizeBox = true;
            }

            OnResize(new EventArgs());
        }

        /// <summary>
        /// Handler: Opens the terminal if the f12 key is pressed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    InvisibleInputText += "0";
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    InvisibleInputText += "1";
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    InvisibleInputText += "2";
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    InvisibleInputText += "3";
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    InvisibleInputText += "4";
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    InvisibleInputText += "5";
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    InvisibleInputText += "6";
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    InvisibleInputText += "7";
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    InvisibleInputText += "8";
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    InvisibleInputText += "9";
                    break;

                case Keys.Delete:
                    Config.Target = DateTime.Now;
                    return;

                case Keys.Pause:
                    ConsoleInterface nul = new ConsoleInterface();
                    Command.GetByType<Freeze>().Execute(new string[0], nul);
                    return;

                case Keys.PageUp:
                    Config.Target = Config.Target.AddDays(1.0);
                    return;

                case Keys.PageDown:
                    Config.Target = Config.Target.AddDays(-1.0);
                    return;

                case Keys.Insert:
                    Config.StopAtZero = !Config.StopAtZero;
                    MessageBox.Show(Config.StopAtZero ? "End Mode: Stop" : "End Mode: Continue");
                    return;

                case Keys.F1:
                    MessageBox.Show(string.Join(Environment.NewLine, 
                        "F1: Help",
                        "F12: Console",
                        "Del: Reset to zero",
                        "Ins: Change end mode",
                        "Pause: Freeze/Unfreeze",
                        "0930: Set target to 09:30 (Must be in 24h format)",
                        "Shift + 0930: Set countdown to 9 minutes and 30 seconds",
                        "Ctrl + 0930: Set countdown to 9 hours and 30 minutes",
                        "Page Up: Add 1 day to target",
                        "Page Dn: Subtract 1 day from to target"
                    ), "Keyboard Shortcuts");
                    return;

                case Keys.F12:
                    if (_console == null || _console.IsDisposed)
                        _console = new UserConsole();

                    _console.Show();
                    _console.BringToFront();
                    return;
            }

            if (InvisibleInputText.Length == 4)
            {
                int a = int.Parse(InvisibleInputText.Substring(0, 2), NumberStyles.Integer, CultureInfo.InvariantCulture);
                int b = int.Parse(InvisibleInputText.Substring(2, 2), NumberStyles.Integer, CultureInfo.InvariantCulture);

                //Duration
                if (e.Shift || e.Alt)
                {
                    int hour;
                    int minute;
                    int second;

                    if (e.Alt)
                    {
                        hour = a;
                        minute = b;
                        second = 0;
                    }
                    else
                    {
                        hour   = 0;
                        minute = a;
                        second = b;
                    }

                    TimeSpan newTarget = new TimeSpan(hour, minute, second);
                    Config.Target = DateTime.Now + newTarget;
                }

                //Target
                else
                {
                    int hour   = a;
                    int minute = b;

                    if (hour < 24 && minute < 60)
                        Config.Target = DateTime.Today.AddHours(hour).AddMinutes(minute);
                }

                LastInvisibileInputTextUpdateTime = default;
                InvisibleInputText = "";
            }
            else
            {
                LastInvisibileInputTextUpdateTime = DateTime.Now;
            }

            
        }
    }
}
