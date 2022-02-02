using NewTimer.Commands;
using NewTimer.FormParts;
using NewTimer.Forms.Bar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
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
        private bool _freeModeBackColorSet = false;

        /// <summary>
        /// The console associated with this window
        /// </summary>
        private UserConsole _console;

        /// <summary>
        /// Whether the window is currently translucent and non-corporeal
        /// </summary>
        private bool translucencyEnabled;

        /// <summary>
        /// Allows holding a modifier to temporarily override the translucency the window
        /// </summary>
        private bool? translucencyOverride;

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
            BackColor = Globals.GlobalBackColor;
            ForeColor = Globals.GlobalForeColor;
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
            setControlDefaults(new ClockControl(), analogSplitContainer.Panel1);
            setControlDefaults(new TimerBar(), analogSplitContainer.Panel2);
            setControlDefaults(new Circle.FullContents(simpleMode: false), tabCircle);
            setControlDefaults(new Circle.FullContents(simpleMode: true), tabCircleSimple);
            setControlDefaults(new TimerBar() { Height = 50, TrackSecondaryTimer = true }, tabBarOnly).Dock = DockStyle.Bottom;
            setControlDefaults(new TimerBar(), tabBarOnly);

            //Designer doesn't allow the panel to be this small, so setting it here
            analogSplitContainer.SplitterDistance = 222;

            //Normalizes settings for the given control. This function returns its input
            /* local */ Control setControlDefaults(Control ctrl, Control parent)
            {
                ctrl.Parent = parent;                
                ctrl.ForeColor = Globals.GlobalForeColor;
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
            TimeOnlyTime.GetText = () => Globals.PrimaryTimer.InFreeMode ? DateTime.Now.ToLongTimeString() : Globals.PrimaryTimer.RealTimeLeft.ToString("h':'mm':'ss");

            //'Hours' panel
            HoursOnlyHour.GetText = () => Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalHours).ToString();
            HoursOnlyFraction.GetText = () => "." + Globals.GetDecimals(Globals.PrimaryTimer.TimeLeft.TotalHours, 5).ToString("00000");
            HoursOnlyTitle.GetText = () => Math.Abs(Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalHours)) == 1 ? "hour" : "hours";

            //'Minutes' panel
            MinutesOnlyMinutes.GetText = () => Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalMinutes).ToString();
            MinutesOnlyFraction.GetText = () => "." + Globals.GetDecimals(Globals.PrimaryTimer.TimeLeft.TotalMinutes, 3).ToString("000");
            MinutesOnlyTitle.GetText = () => Math.Abs(Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalMinutes)) == 1 ? "minute" : "minutes";

            //'Seconds' panel
            SecondsOnlySecond.GetText = () => Math.Floor(Globals.PrimaryTimer.RealTimeLeft.TotalSeconds).ToString();
            SecondsOnlyTitle.GetText = () => Math.Abs(Math.Floor(Globals.PrimaryTimer.RealTimeLeft.TotalSeconds)) == 1 ? "second" : "seconds";
        }

        /// <summary>
        /// Normalizes the colors of all controls
        /// </summary>
        private void SetColors()
        {
            foreach (Control i in tabs.Controls)
            {
                i.BackColor = Globals.GlobalBackColor;
                i.ForeColor = Globals.GlobalForeColor;
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

            int modifierCount = 0;

            if (ModifierKeys.HasFlag(Keys.Control))
                modifierCount++;

            if (ModifierKeys.HasFlag(Keys.Shift))
                modifierCount++;

            if (ModifierKeys.HasFlag(Keys.Alt))
                modifierCount++;

            if (modifierCount > 0)
            {
                if (translucencyEnabled)
                {
                    

                    if (Bounds.Contains(MousePosition) || modifierCount >= 2)
                        TempOverrideTranslucencyMode();

                    else
                        StopTempOverrideTranslucencyMode();
                }
                else
                {
                    TempOverrideTranslucencyMode();
                }
                
            }
                

            else
                StopTempOverrideTranslucencyMode();


            //Update ICountdowns
            recursiveUpdate(this);

            //Recursively updates every ICountdown control in the given control
            /* local */ void recursiveUpdate(Control c)
            {
                if (c is ICountdown ic)
                {
                    ic.OnCountdownTick(Globals.PrimaryTimer.TimeLeft, Globals.SecondaryTimer.TimeLeft, Globals.PrimaryTimer.Overtime);
                }

                foreach (Control i in c.Controls)
                {
                    recursiveUpdate(i);
                }
            }     
        }

        /// <summary>
        /// Overrides the translucency mode of the window without updating the internal flag
        /// </summary>
        private void TempOverrideTranslucencyMode()
        {
            if (translucencyOverride != null)
                return;

            translucencyOverride = !translucencyEnabled;
            ToggleWindowTranslucencyMode();
        }

        /// <summary>
        /// Stops the current temp override of the translucency mode
        /// </summary>
        private void StopTempOverrideTranslucencyMode()
        {
            if (translucencyOverride == null)
                return;

            translucencyOverride = null;
            translucencyEnabled = !translucencyEnabled; //This should counteract the toggle part of the toggle function
            ToggleWindowTranslucencyMode();
        }

        /// <summary>
        /// Toggles whether the window is translucent and non-corporeal
        /// </summary>
        public void ToggleWindowTranslucencyMode()
        {
            if (translucencyOverride != null)
            {
                Opacity = translucencyOverride.Value ? Globals.OPACITY_TRANSLUCENT : Globals.OPACITY_NORMAL;
                ClickThroughHelper.SetClickThrough(this, translucencyOverride.Value);
            }
            else
            {
                translucencyEnabled = !translucencyEnabled;
                Opacity = translucencyEnabled ? Globals.OPACITY_TRANSLUCENT : Globals.OPACITY_NORMAL;
                ClickThroughHelper.SetClickThrough(this, translucencyEnabled);
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
            if (_lastMinuteIconWasCreated != Globals.PrimaryTimer.RealTimeLeft.Minutes)
            {
                _lastMinuteIconWasCreated = Globals.PrimaryTimer.RealTimeLeft.Minutes;

                //Create new pie
                Bitmap pie = TaskbarHelper.CreatePie(
                    bounds:      new Rectangle(0, 0, 64, 64),
                    primary:     Properties.Resources.clrUpper  .GetPixel(Globals.PrimaryTimer.TimeLeft.Minutes, 0),
                    secondary:   Properties.Resources.clrLower  .GetPixel(Globals.PrimaryTimer.TimeLeft.Minutes, 0),
                    primaryBG:   Properties.Resources.clrToGreen.GetPixel(Globals.PrimaryTimer.TimeLeft.Minutes, 0),
                    secondaryBG: Properties.Resources.clrToRed  .GetPixel(Globals.PrimaryTimer.TimeLeft.Minutes, 0),
                    value:       (float)Globals.PrimaryTimer.TimeLeft.TotalMinutes / 60f,
                    startAngle:  (int)Math.Floor(Globals.PrimaryTimer.Target.Minute / 60f * 360 - 90),
                    ccw:         !Globals.PrimaryTimer.Overtime
                );

                //Update icon
                Icon = TaskbarHelper.CreateIconFromBitmap(pie);
                pie.Dispose();
            }

            //Set background
            if (Globals.PrimaryTimer.InFreeMode && !_freeModeBackColorSet)
            {
                foreach (Control i in tabs.Controls)
                {
                    i.BackColor = Globals.GlobalFreeModeBackColor;
                }

                _overtimeBackColorSet = false;
                _freeModeBackColorSet = true;
            }
            else if (Globals.PrimaryTimer.Overtime && !_overtimeBackColorSet)
            {
                foreach (Control i in tabs.Controls)
                {
                    i.BackColor = Globals.GlobalOvertimeColor;
                }

                _overtimeBackColorSet = true;
                _freeModeBackColorSet = false;
            }
            else if ((!Globals.PrimaryTimer.Overtime && !Globals.PrimaryTimer.InFreeMode) && (_overtimeBackColorSet || _freeModeBackColorSet))
            {
                foreach (Control i in tabs.Controls)
                {
                    i.BackColor = Globals.GlobalBackColor;
                }

                _overtimeBackColorSet = false;
                _freeModeBackColorSet = false;
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
            ConsoleInterface nullOutput = new ConsoleInterface();

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
                    if (e.Shift)
                        Globals.PrimaryTimer.InFreeMode = !Globals.PrimaryTimer.InFreeMode;
                    else
                        Globals.PrimaryTimer.Target = DateTime.Now;

                    return;

                case Keys.Pause:
                    Command.GetByType<Freeze>().Execute(new string[0], nullOutput);
                    return;

                case Keys.PageUp:
                    Globals.PrimaryTimer.Target = Globals.PrimaryTimer.Target.AddDays(1.0);
                    return;

                case Keys.PageDown:
                    Globals.PrimaryTimer.Target = Globals.PrimaryTimer.Target.AddDays(-1.0);
                    return;

                case Keys.Insert:
                    Globals.PrimaryTimer.StopAtZero = !Globals.PrimaryTimer.StopAtZero;
                    MessageBox.Show(Globals.PrimaryTimer.StopAtZero ? "End Mode: Stop" : "End Mode: Continue");
                    return;

                case Keys.Return:
                    Globals.SwapTimers();
                    return;

                case Keys.F1:
                    MessageBox.Show(string.Join(Environment.NewLine, 
                        "F1: Help",
                        "F10: Collapse/Uncollapse",
                        "F11: Translucency Mode",
                        "F12: Console",
                        "Del: Reset to zero",
                        "Shift + Del: Idle Mode",
                        "Enter: Swap Primary/Secondary Timer",
                        "Ins: Change end mode",
                        "Pause: Freeze/Unfreeze",
                        "0930: Set target to 09:30 (Must be in 24h format)",
                        "Shift + 0930: Set countdown to 9 minutes and 30 seconds",
                        "Alt + 0930: Set countdown to 9 hours and 30 minutes",
                        "Page Up: Add 1 day to target",
                        "Page Dn: Subtract 1 day from to target"
                    ), "Keyboard Shortcuts");
                    return;

                case Keys.F11:
                    ToggleWindowTranslucencyMode();
                    break;

                case Keys.F10:
                    const int COLLAPSE_HEIGHT = 90;

                    if (tabs.SelectedIndex == 4)
                    {
                        int lastHeigh = Height;
                        tabs.SelectedIndex = 0;
                        Top -= (Height - lastHeigh);

                        //Found this solution on stack
                        tabs.Appearance = TabAppearance.Normal;
                        tabs.ItemSize = new Size(30, 18);
                    }
                    else
                    {
                        tabs.SelectedIndex = 4;
                        Top += (Height - COLLAPSE_HEIGHT);
                        Height = COLLAPSE_HEIGHT;

                        tabs.Appearance = TabAppearance.FlatButtons;
                        tabs.ItemSize = new Size(0, 1);
                    }

                    
                    break;

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
                    Globals.PrimaryTimer.Target = DateTime.Now + newTarget;
                    Globals.PrimaryTimer.InFreeMode = false;
                }

                //Target
                else
                {
                    int hour   = a;
                    int minute = b;

                    if (hour < 24 && minute < 60)
                    {
                        Globals.PrimaryTimer.Target = DateTime.Today.AddHours(hour).AddMinutes(minute);
                        Globals.PrimaryTimer.InFreeMode = false;
                    }
                        
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
