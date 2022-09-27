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
        /// The micro view control
        /// </summary>
        private MicroView _microView;

        /// <summary>
        /// Gets or sets the location the window will restore to when micro mode is disabled
        /// </summary>
        private Point _macroViewPosition;

        /// <summary>
        /// The last minute a taskbar/statusbar icon was created. Used to prevent creating a new taskbar icon every frame
        /// </summary>
        private int _lastMinuteIconWasCreated = 61;

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

        /// <summary>
        /// Used to control the visibility of the secondary timer bar when the window is in compact mode
        /// </summary>
        private TimerBar _secondaryFullscreenBar;

        protected override bool DoubleBuffered { get => true; }

        public TimerFormBase()
        {
            InitializeComponent();

            //Create micro view in the background
            _microView = new MicroView();
            Controls.Add(_microView);

            //Let background through
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

            setControlDefaults(new TimerBar(), tabBarOnly);
            _secondaryFullscreenBar = (TimerBar)setControlDefaults(new TimerBar() { Height = 50, TrackSecondaryTimer = true }, tabBarOnly);
            _secondaryFullscreenBar.Dock = DockStyle.Bottom;

            //Designer doesn't allow the panel to be this small, so setting it here
            analogSplitContainer.SplitterDistance = 221;

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
            TimeOnlyTime.ForeThemedColor = Globals.DaysColor;

            //'Hours' panel
            HoursOnlyHour.GetText = () => Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalHours).ToString();
            HoursOnlyFraction.GetText = () => "." + Globals.GetDecimals(Globals.PrimaryTimer.TimeLeft.TotalHours, 5).ToString("00000");
            HoursOnlyTitle.GetText = () => Math.Abs(Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalHours)) == 1 ? "hour" : "hours";
            HoursOnlyHour.ForeThemedColor = Globals.HoursColor;
            HoursOnlyFraction.ForeThemedColor = Globals.HoursColor;
            HoursOnlyTitle.ForeThemedColor = Globals.HoursColor;

            //'Minutes' panel
            MinutesOnlyMinutes.GetText = () => Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalMinutes).ToString();
            MinutesOnlyFraction.GetText = () => "." + Globals.GetDecimals(Globals.PrimaryTimer.TimeLeft.TotalMinutes, 3).ToString("000");
            MinutesOnlyTitle.GetText = () => Math.Abs(Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalMinutes)) == 1 ? "minute" : "minutes";
            MinutesOnlyMinutes.ForeThemedColor = Globals.MinutesColor;
            MinutesOnlyFraction.ForeThemedColor = Globals.MinutesColor;
            MinutesOnlyTitle.ForeThemedColor = Globals.MinutesColor;

            //'Seconds' panel
            SecondsOnlySecond.GetText = () => Math.Floor(Globals.PrimaryTimer.RealTimeLeft.TotalSeconds).ToString();
            SecondsOnlyTitle.GetText = () => Math.Abs(Math.Floor(Globals.PrimaryTimer.RealTimeLeft.TotalSeconds)) == 1 ? "second" : "seconds";
            SecondsOnlySecond.ForeThemedColor = Globals.SecondsColor;
            SecondsOnlyTitle.ForeThemedColor = Globals.SecondsColor;

            //'Text' panel
            TextOnlyMinute.GetText = () => TaskbarUtility.NumberToWord((int)Math.Floor(Globals.PrimaryTimer.RealTimeLeft.TotalMinutes), true);
            TextOnlySecond.GetText = () => TaskbarUtility.NumberToWord(Globals.PrimaryTimer.TimeLeft.Seconds, true);
            TextOnlyMinute.ForeThemedColor = Globals.TextOnlyColor;
            TextOnlySecond.ForeThemedColor = Globals.TextOnlyColor;
        }

        /// <summary>
        /// Recursively find and update every ICountdown control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateICountdowns(object sender, EventArgs e)
        {
            ForeColor = Globals.GlobalForeColor;

            Color newBackColor;

            if (Globals.PrimaryTimer.InFreeMode)
                newBackColor = Globals.GlobalFreeModeBackColor;

            else if (Globals.PrimaryTimer.Overtime)
                newBackColor = Globals.GlobalOvertimeColor;

            else
                newBackColor = Globals.GlobalBackColor;

            if (BackColor != newBackColor)
            {
                BackColor = newBackColor;
                foreach (Control i in tabs.Controls)
                {
                    i.BackColor = newBackColor;
                    i.ForeColor = newBackColor;
                }
            }

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

            //Used by the broadcaster when using invisible input numbers
            Func<string, string, string> modifierDescription = (h, m) => "Set Target: " + h + ":" + m;

            if (e.Shift)
                modifierDescription = modifierDescription = (m, s) => "Set Duration: " + m + " mins, " + s + " secs";

            else if (e.Alt)
                modifierDescription = modifierDescription = (h, m) => "Set Duration: " + h + " hrs, " + m + " mins";

            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    InvisibleInputText += "0";
                    Globals.Broadcast(modifierDescription(InvisibleInputText.PadRight(4, '?').Substring(0, 2), InvisibleInputText.PadRight(4, '?').Substring(2, 2)), InvisibleInputText);
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    InvisibleInputText += "1";
                    Globals.Broadcast(modifierDescription(InvisibleInputText.PadRight(4, '?').Substring(0, 2), InvisibleInputText.PadRight(4, '?').Substring(2, 2)), InvisibleInputText);
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    InvisibleInputText += "2";
                    Globals.Broadcast(modifierDescription(InvisibleInputText.PadRight(4, '?').Substring(0, 2), InvisibleInputText.PadRight(4, '?').Substring(2, 2)), InvisibleInputText);
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    InvisibleInputText += "3";
                    Globals.Broadcast(modifierDescription(InvisibleInputText.PadRight(4, '?').Substring(0, 2), InvisibleInputText.PadRight(4, '?').Substring(2, 2)), InvisibleInputText);
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    InvisibleInputText += "4";
                    Globals.Broadcast(modifierDescription(InvisibleInputText.PadRight(4, '?').Substring(0, 2), InvisibleInputText.PadRight(4, '?').Substring(2, 2)), InvisibleInputText);
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    InvisibleInputText += "5";
                    Globals.Broadcast(modifierDescription(InvisibleInputText.PadRight(4, '?').Substring(0, 2), InvisibleInputText.PadRight(4, '?').Substring(2, 2)), InvisibleInputText);
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    InvisibleInputText += "6";
                    Globals.Broadcast(modifierDescription(InvisibleInputText.PadRight(4, '?').Substring(0, 2), InvisibleInputText.PadRight(4, '?').Substring(2, 2)), InvisibleInputText);
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    InvisibleInputText += "7";
                    Globals.Broadcast(modifierDescription(InvisibleInputText.PadRight(4, '?').Substring(0, 2), InvisibleInputText.PadRight(4, '?').Substring(2, 2)), InvisibleInputText);
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    InvisibleInputText += "8";
                    Globals.Broadcast(modifierDescription(InvisibleInputText.PadRight(4, '?').Substring(0, 2), InvisibleInputText.PadRight(4, '?').Substring(2, 2)), InvisibleInputText);
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    InvisibleInputText += "9";
                    Globals.Broadcast(modifierDescription(InvisibleInputText.PadRight(4, '?').Substring(0, 2), InvisibleInputText.PadRight(4, '?').Substring(2, 2)), InvisibleInputText);
                    break;

                case Keys.Delete:
                    if (e.Shift)
                    {
                        Globals.PrimaryTimer.InFreeMode = !Globals.PrimaryTimer.InFreeMode;

                        if (Globals.PrimaryTimer.InFreeMode)
                            Globals.Broadcast("Idle Mode", "IDLE");

                        else
                            Globals.Broadcast("Timer Mode", "TMR");
                    }

                    else
                    {
                        Globals.PrimaryTimer.Target = DateTime.Now;
                        Globals.Broadcast("Reset", "RSET");
                    }
                        
                    return;

                case Keys.Pause:
                    Command.GetByType<Freeze>().Execute(new string[0], nullOutput);

                    if (Globals.PrimaryTimer.Paused)
                        Globals.Broadcast(null, "PAUS");
                    else
                        Globals.Broadcast("Resume", "RESU");

                    return;

                case Keys.PageUp:
                    if (e.Shift)
                    {
                        int currentIndex = Globals.ColorSchemes
                            .TakeWhile(i => i != Globals.PrimaryTimer.ColorScheme)
                            .Count();

                        if (currentIndex >= Globals.ColorSchemes.Length)
                            currentIndex = -1;

                        Globals.PrimaryTimer.ColorScheme = Globals.ColorSchemes[(currentIndex + 1) % Globals.ColorSchemes.Length];
                        Globals.PrimaryTimer.ColorizeTimerBar();
                        _microView.Recolorize();
                        Globals.Broadcast("Color Scheme: " + Globals.PrimaryTimer.ColorScheme.Name, "CL" + Globals.PrimaryTimer.ColorScheme.Name.Substring(0, 2));
                    }    
                        
                    else
                    {
                        Globals.PrimaryTimer.Target = Globals.PrimaryTimer.Target.AddDays(1.0);

                        string microBroadcast = Globals.PrimaryTimer.Target.ToString("dd''MM");
                        string broadcast = Globals.PrimaryTimer.Target.ToString("ddd MMM dd");

                        //Use common names for days around today
                        if (Globals.PrimaryTimer.Target.Date == DateTime.Today)
                        {
                            broadcast = "Today";
                            microBroadcast = "TDAY";
                        }

                        else if (Globals.PrimaryTimer.Target.Date == DateTime.Today.AddDays(1))
                        {
                            broadcast = "Tomorrow";
                            microBroadcast = "TMRW";
                        }

                        else if (Globals.PrimaryTimer.Target.Date == DateTime.Today.AddDays(-1))
                        {
                            broadcast = "Yesterday";
                            microBroadcast = "YDAY";
                        }

                        //Use day-of-the-week instead of month number if we are within one month of the target
                        else if (Math.Abs((Globals.PrimaryTimer.Target - DateTime.Now).TotalDays) < 30)
                            microBroadcast = microBroadcast.Substring(0, 2) + Globals.PrimaryTimer.Target.ToString("ddd").Substring(0, 2);

                        Globals.Broadcast("Target Day: " + broadcast, microBroadcast);
                    }

                    return;

                case Keys.PageDown:
                    if (e.Shift)
                    {
                        int currentIndex = Globals.ColorSchemes
                            .TakeWhile(i => i != Globals.PrimaryTimer.ColorScheme)
                            .Count();

                        if (currentIndex >= Globals.ColorSchemes.Length)
                            currentIndex = 1;

                        if (currentIndex - 1 < 0)
                            currentIndex = Globals.ColorSchemes.Length;

                        Globals.PrimaryTimer.ColorScheme = Globals.ColorSchemes[currentIndex - 1];
                        Globals.PrimaryTimer.ColorizeTimerBar();
                        _microView.Recolorize();
                        Globals.Broadcast("Color Scheme: " + Globals.PrimaryTimer.ColorScheme.Name, "CL" + Globals.PrimaryTimer.ColorScheme.Name.Substring(0, 2));
                    }
                    else
                    {
                        Globals.PrimaryTimer.Target = Globals.PrimaryTimer.Target.AddDays(-1.0);

                        string microBroadcast = Globals.PrimaryTimer.Target.ToString("dd''MM");
                        string broadcast = Globals.PrimaryTimer.Target.ToString("ddd MMM dd");

                        //Use common names for days around today
                        if (Globals.PrimaryTimer.Target.Date == DateTime.Today)
                        {
                            broadcast = "Today";
                            microBroadcast = "TDAY";
                        }

                        else if (Globals.PrimaryTimer.Target.Date == DateTime.Today.AddDays(1))
                        {
                            broadcast = "Tomorrow";
                            microBroadcast = "TMRW";
                        }

                        else if (Globals.PrimaryTimer.Target.Date == DateTime.Today.AddDays(-1))
                        {
                            broadcast = "Yesterday";
                            microBroadcast = "YDAY";
                        }

                        //Use day-of-the-week instead of month number if we are within one month of the target
                        else if (Math.Abs((Globals.PrimaryTimer.Target - DateTime.Now).TotalDays) < 30)
                            microBroadcast = microBroadcast.Substring(0, 2) + Globals.PrimaryTimer.Target.ToString("ddd").Substring(0, 2);

                        Globals.Broadcast("Target Day: " + broadcast, microBroadcast);
                    }
                    
                    return;

                case Keys.Insert:
                    Globals.PrimaryTimer.StopAtZero = !Globals.PrimaryTimer.StopAtZero;

                    if (Globals.PrimaryTimer.StopAtZero)
                        Globals.Broadcast("Stop At Zero", "STOP");
                    else
                        Globals.Broadcast("Continue After Zero", "CONT");

                    return;

                case Keys.Return:
                    Globals.SwapTimers();

                    if (Globals.PrimaryTimer.InFreeMode)
                        Globals.Broadcast("Second Timer New", "NEW");
                    return;

                case Keys.F1:
                    Globals.Broadcast("Help!", "HELP");
                    MessageBox.Show(string.Join(Environment.NewLine, 
                        "F1: Help",
                        "--------",
                        "F2: Swap analog clock hand emphasis",
                        "F3: Generate new colors",
                        "Shift + F3: Sync primary and secondary timers' colors",
                        "Shift + Page Up: Next color scheme",
                        "Shift + Page Dn: Previous color scheme",
                        "--------",
                        "F9: Toggle Micro Mode",
                        "F10: Collapse/Uncollapse",
                        "F11: Translucency Mode",
                        "F12: Console",
                        "--------",
                        "Shift + Del: Idle Mode",
                        "Enter: Swap Primary/Secondary Timer",
                        "Ins: Change end mode",
                        "Pause: Freeze/Unfreeze",
                        "--------",
                        "Del: Reset to zero",
                        "0930: Set target to 09:30 (Must be in 24h format)",
                        "Shift + 0930: Set countdown to 9 minutes and 30 seconds",
                        "Alt + 0930: Set countdown to 9 hours and 30 minutes",
                        "--------",
                        "Up: Add 1 minute to target",
                        "Shift + Up: Add 5 minutes to target",
                        "Alt + Up: Add 15 minutes to target",
                        "Ctrl + Up: Add 1 hour to target",
                        "Page Up: Add 1 day to target",
                        "Dn: Subtract 1 minute to target",
                        "Shift + Dn: Subtract 5 minutes to target",
                        "Alt + Dn: Subtract 15 minutes to target",
                        "Ctrl + Dn: Subtract 1 hour to target",
                        "Page Dn: Subtract 1 day from to target"
                    ), "Keyboard Shortcuts");
                    return;

                case Keys.F9:
                    //Enable micro mode
                    if (tabs.Visible)
                    {
                        tabs.SelectedIndex = 1; //Just to make sure the size isn't overridden by the Digi tab
                        tabs.Visible = false;
                        FormBorderStyle = FormBorderStyle.None;
                        Size = _microView.Size;
                        _macroViewPosition = Location;

                        //Figure out where to pin the control based on how close it is to the different corners of the screen
                        Screen containingScreen = Screen.FromControl(this);

                        int distanceFromLeft  = Math.Abs(Bounds.Left - containingScreen.WorkingArea.Left);
                        int distanceFromRight = Math.Abs(Bounds.Right - containingScreen.WorkingArea.Right);
                        int distanceFromTop = Math.Abs(Bounds.Top - containingScreen.WorkingArea.Top);
                        int distanceFromBottom = Math.Abs(Bounds.Bottom - containingScreen.WorkingArea.Bottom);

                        if (distanceFromLeft < distanceFromRight)
                        {
                            //Closest to top left
                            if (distanceFromTop < distanceFromBottom)
                            {
                                Location = containingScreen.WorkingArea.Location;
                            }

                            //Closest to bottom left
                            else
                            {
                                Location = new Point(
                                    x: containingScreen.WorkingArea.Left,
                                    y: containingScreen.WorkingArea.Bottom - Height
                                );
                            }
                        }
                        else
                        {
                            //Closest to top right
                            if (distanceFromTop < distanceFromBottom)
                            {
                                Location = new Point(
                                    x: containingScreen.WorkingArea.Right - Width,
                                    y: containingScreen.WorkingArea.Top
                                );
                            }

                            //Closest to bottom right
                            else
                            {
                                Location = new Point(
                                    x: containingScreen.WorkingArea.Right - Width,
                                    y: containingScreen.WorkingArea.Bottom - Height
                                );
                            }
                        }

                        Globals.Broadcast("Enable Micro View", "MICR");
                    }

                    //Disable micro mode
                    else
                    {
                        tabs.Visible = true;
                        FormBorderStyle = FormBorderStyle.Sizable;
                        tabs.SelectedIndex = 0;
                        Location = _macroViewPosition;

                        Globals.Broadcast("Disable Micro View", null);
                    }

                    break;

                case Keys.F11:
                    ToggleWindowTranslucencyMode();
                    Globals.Broadcast("Toggle Translucency", "TRAN");
                    break;

                case Keys.F10:
                    const int COLLAPSE_HEIGHT = 70;

                    Rectangle initialBounds = DesktopBounds;

                    if (tabs.SelectedIndex == 4)
                    {
                        //Abusing the fact that tab 0 forces a certain size
                        int lastHeigh = Height;
                        tabs.SelectedIndex = 0;
                        Top -= (Height - lastHeigh);
                        FormBorderStyle = FormBorderStyle.Sizable;

                        //Found this solution on stack
                        tabs.Appearance = TabAppearance.Normal;
                        tabs.ItemSize = new Size(30, 18);
                        _secondaryFullscreenBar.Height = 50;

                        Globals.Broadcast("Uncollapse", null);
                    }
                    else
                    {
                        tabs.SelectedIndex = 4;
                        FormBorderStyle = FormBorderStyle.None;
                        DesktopBounds = Rectangle.FromLTRB(
                            initialBounds.Left, 
                            initialBounds.Bottom - COLLAPSE_HEIGHT, 
                            initialBounds.Right, 
                            initialBounds.Bottom
                        );

                        tabs.Appearance = TabAppearance.FlatButtons;
                        tabs.ItemSize = new Size(0, 1);
                        _secondaryFullscreenBar.Height = 0;

                        Globals.Broadcast("Collapse", null);
                    }


                    break;

                case Keys.F12:
                    if (_console == null || _console.IsDisposed)
                        _console = new UserConsole();

                    _console.Show();
                    _console.BringToFront();
                    Globals.Broadcast("Console", "CONS");
                    return;

                case Keys.F2:
                    Globals.SwapHandPriorities = !Globals.SwapHandPriorities;
                    Globals.Broadcast("Swap Analog Hand Priority", "HAND");
                    return;

                case Keys.F3:
                    if (e.Shift)
                    {
                        Command.GetByType<Commands.ColorScheme>().Execute(new string[] { "sync" }, nullOutput);
                        Globals.Broadcast("Sync Color Schemes", "SYNC");
                    }
                    else
                    {
                        Globals.PrimaryTimer.ColorizeTimerBar();
                        Globals.Broadcast("Re-Colorize", "COLR");
                    }

                    _microView.Recolorize();
                    return;

                case Keys.Up:
                    if (e.Shift)
                        Globals.PrimaryTimer.Target = Globals.PrimaryTimer.Target.AddMinutes(5);

                    if (e.Alt)
                        Globals.PrimaryTimer.Target = Globals.PrimaryTimer.Target.AddMinutes(15);

                    if (e.Control)
                        Globals.PrimaryTimer.Target = Globals.PrimaryTimer.Target.AddHours(1);

                    else
                        Globals.PrimaryTimer.Target = Globals.PrimaryTimer.Target.AddMinutes(1);

                    Globals.Broadcast("Shift To: " + Globals.PrimaryTimer.Target, Globals.PrimaryTimer.Target.ToString("HH''mm"));
                    break;

                case Keys.Down:
                    if (e.Shift)
                        Globals.PrimaryTimer.Target = Globals.PrimaryTimer.Target.AddMinutes(-5);

                    if (e.Alt)
                        Globals.PrimaryTimer.Target = Globals.PrimaryTimer.Target.AddMinutes(-15);

                    if (e.Control)
                        Globals.PrimaryTimer.Target = Globals.PrimaryTimer.Target.AddHours(-1);

                    else
                        Globals.PrimaryTimer.Target = Globals.PrimaryTimer.Target.AddMinutes(-1);

                    Globals.Broadcast("Shift To: " + Globals.PrimaryTimer.Target, Globals.PrimaryTimer.Target.ToString("HH''mm"));
                    break;
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
