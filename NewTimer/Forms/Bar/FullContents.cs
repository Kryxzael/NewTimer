using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewTimer.FormParts;

namespace NewTimer.Forms.Bar
{
    public partial class FullContents : UserControl, ICountdown
    {
        private Point _secondaryHourDefaultPosition;
        private Point _secondaryHourAlternatePosition = new Point(74, 131);

        private Point _secondaryDayDefaultPosition;
        private Point _secondaryDayAlternatePosition = new Point(45, 128);

        public FullContents()
        {
            InitializeComponent();

            _secondaryHourDefaultPosition = Full2ndH.Location;
            _secondaryDayDefaultPosition = Full2ndD.Location;

            //Sets foreground colors to comply with global settings
            FullD.ForeColor = Globals.GlobalForeColor;
            FullH.ForeColor = Globals.GlobalForeColor;
            FullM.ForeColor = Globals.GlobalForeColor;
            FullS.ForeColor = Globals.GlobalForeColor;

            Full2ndD.ForeColor = Globals.GlobalSecondaryForeColor;
            Full2ndH.ForeColor = Globals.GlobalSecondaryForeColor;
            Full2ndM.ForeColor = Globals.GlobalSecondaryForeColor;
            Full2ndS.ForeColor = Globals.GlobalSecondaryForeColor;

            FullTotalH.ForeColor = Globals.GlobalForeColor;
            FullTotalM.ForeColor = Globals.GlobalForeColor;
            FullTotalS.ForeColor = Globals.GlobalForeColor;

            FullFracM.ForeColor = Globals.GlobalForeColor;
            FullFracM.ForeColor = Globals.GlobalForeColor;
        }

        /// <summary>
        /// Handler: Updates contents
        /// </summary>
        /// <param name="span"></param>
        /// <param name="isOvertime"></param>
        public void OnCountdownTick(TimeSpan span, TimeSpan secondSpan, bool isOvertime)
        {
            /*
             * Set fore colors
             */
            //Main display
            FullD.ForeColor = Globals.GlobalForeColor;
            FullH.ForeColor = Globals.GlobalForeColor;
            FullM.ForeColor = Globals.GlobalForeColor;
            FullS.ForeColor = Globals.GlobalForeColor;

            Full2ndD.ForeColor = Globals.GlobalSecondaryForeColor;
            Full2ndH.ForeColor = Globals.GlobalSecondaryForeColor;
            Full2ndM.ForeColor = Globals.GlobalSecondaryForeColor;
            Full2ndS.ForeColor = Globals.GlobalSecondaryForeColor;

            //Integer portions of total hours, minutes and seconds
            FullTotalH.ForeColor = Globals.GlobalForeColor;
            FullTotalM.ForeColor = Globals.GlobalForeColor;
            FullTotalS.ForeColor = Globals.GlobalForeColor;

            //Fraction portions of total hours and minutes
            FullFracH.ForeColor = Globals.GlobalForeColor;
            FullFracM.ForeColor = Globals.GlobalForeColor;

            //Headers
            lblTotalHours.ForeColor = Globals.GlobalForeColor;
            lblTotalMinutes.ForeColor = Globals.GlobalForeColor;
            lblTotalSeconds.ForeColor = Globals.GlobalForeColor;

            /*
             * Set dynamic fill colors
             */

            //Main display
            FullD.HighlightColor = Globals.DaysColor;
            FullH.HighlightColor = Globals.HoursColor;
            FullM.HighlightColor = Globals.MinutesColor;
            FullS.HighlightColor = Globals.SecondsColor;

            Full2ndD.HighlightColor = Globals.DaysColor;
            Full2ndH.HighlightColor = Globals.HoursColor;
            Full2ndM.HighlightColor = Globals.MinutesColor;
            Full2ndS.HighlightColor = Globals.SecondsColor;

            //Integer portions of total hours, minutes and seconds
            FullTotalH.HighlightColor = FullH.HighlightColor;
            FullTotalM.HighlightColor = FullM.HighlightColor;
            FullTotalS.HighlightColor = FullS.HighlightColor;

            //Fraction portions of total hours and minutes
            FullFracH.HighlightColor = FullH.HighlightColor;
            FullFracM.HighlightColor = FullM.HighlightColor;

            /*
             * Set color of leading zeros
             */

            //Main display
            FullD.LeadingZerosColor = Color.Transparent;
            FullH.LeadingZerosColor = Globals.GlobalGrayedColor;
            FullM.LeadingZerosColor = Globals.GlobalGrayedColor;
            FullS.LeadingZerosColor = Globals.GlobalGrayedColor;

            Full2ndD.LeadingZerosColor = Color.Transparent;
            Full2ndH.LeadingZerosColor = Globals.GlobalGrayedColor;
            Full2ndM.LeadingZerosColor = Globals.GlobalGrayedColor;
            Full2ndS.LeadingZerosColor = Globals.GlobalGrayedColor;

            //Integer portions of total hours, minutes and seconds
            FullTotalH.LeadingZerosColor = Globals.GlobalGrayedColor;
            FullTotalM.LeadingZerosColor = Globals.GlobalGrayedColor;
            FullTotalS.LeadingZerosColor = Globals.GlobalGrayedColor;

            //Fraction portions of total hours and minutes
            FullFracH.LeadingZerosColor = Globals.GlobalGrayedColor;
            FullFracM.LeadingZerosColor = Globals.GlobalGrayedColor;

            /*
             * Set animation settings
             */
            FullD.RollUp = Globals.PrimaryTimer.Overtime || Globals.PrimaryTimer.InFreeMode;
            FullH.RollUp = Globals.PrimaryTimer.Overtime || Globals.PrimaryTimer.InFreeMode;
            FullM.RollUp = Globals.PrimaryTimer.Overtime || Globals.PrimaryTimer.InFreeMode;
            FullS.RollUp = Globals.PrimaryTimer.Overtime || Globals.PrimaryTimer.InFreeMode;

            Full2ndD.RollUp = Globals.SecondaryTimer.Overtime || Globals.SecondaryTimer.InFreeMode;
            Full2ndH.RollUp = Globals.SecondaryTimer.Overtime || Globals.SecondaryTimer.InFreeMode;
            Full2ndM.RollUp = Globals.SecondaryTimer.Overtime || Globals.SecondaryTimer.InFreeMode;
            Full2ndS.RollUp = Globals.SecondaryTimer.Overtime || Globals.SecondaryTimer.InFreeMode;

            //Integer portions of total hours, minutes and seconds
            FullTotalH.RollUp = Globals.PrimaryTimer.Overtime || Globals.PrimaryTimer.InFreeMode;
            FullTotalM.RollUp = Globals.PrimaryTimer.Overtime || Globals.PrimaryTimer.InFreeMode;
            FullTotalS.RollUp = Globals.PrimaryTimer.Overtime || Globals.PrimaryTimer.InFreeMode;

            //Fraction portions of total hours and minutes
            FullFracH.RollUp = Globals.PrimaryTimer.Overtime || Globals.PrimaryTimer.InFreeMode;
            FullFracM.RollUp = Globals.PrimaryTimer.Overtime || Globals.PrimaryTimer.InFreeMode;

            /*
             * Sets the text
             */
            //Main days
            FullD.Text = span.Days >= 1000 ? "  ---" : span.Days.ToString("000");
            FullH.RenderLeadingZeros = true;

            //Main hours
            FullH.Text = Globals.PrimaryTimer.InFreeMode ? DateTime.Now.Hour.ToString("00") : span.Hours.ToString("00");
            FullH.RenderLeadingZeros = Globals.PrimaryTimer.InFreeMode || span.TotalDays >= 1;

            //Main minutes
            FullM.Text = Globals.PrimaryTimer.InFreeMode ? DateTime.Now.Minute.ToString("00") : span.Minutes.ToString("00");
            FullM.RenderLeadingZeros = Globals.PrimaryTimer.InFreeMode || span.TotalHours >= 1;

            //Main seconds
            FullS.Text = Globals.PrimaryTimer.InFreeMode ? DateTime.Now.Second.ToString("00") : span.Seconds.ToString("00");
            FullS.RenderLeadingZeros = Globals.PrimaryTimer.InFreeMode || span.TotalMinutes >= 1;

            
            if (!Globals.SecondaryTimer.InFreeMode)
            {
                Full2ndD.Visible = true;
                Full2ndH.Visible = true;

                if (Globals.PrimaryTimer.TimeLeft.Seconds == Globals.SecondaryTimer.TimeLeft.Seconds 
                    && Globals.PrimaryTimer.Overtime == Globals.SecondaryTimer.Overtime
                )
                {
                    if (Globals.PrimaryTimer.TimeLeft.Minutes == Globals.SecondaryTimer.TimeLeft.Minutes)
                        Full2ndM.Visible = false;

                    else
                        Full2ndM.Visible = true;

                    Full2ndS.Visible = false;
                }
                else
                {
                    Full2ndM.Visible = true;
                    Full2ndS.Visible = true;
                }

                if (!Full2ndM.Visible && !Full2ndS.Visible)
                {
                    Full2ndH.Location = _secondaryHourAlternatePosition;
                    Full2ndD.Location = _secondaryDayAlternatePosition;
                }
                else 
                {
                    Full2ndH.Location = _secondaryHourDefaultPosition;
                    Full2ndD.Location = _secondaryDayDefaultPosition;
                }
                

                //2nd days
                Full2ndD.Text = Globals.SecondaryTimer.TimeLeft.Days.ToString("000");
                Full2ndH.RenderLeadingZeros = false;

                //2nd hours
                Full2ndH.Text = Globals.SecondaryTimer.TimeLeft.Hours.ToString("00");
                Full2ndH.RenderLeadingZeros = Globals.SecondaryTimer.TimeLeft.TotalDays >= 1;

                //2nd minutes
                Full2ndM.Text = Globals.SecondaryTimer.TimeLeft.Minutes.ToString("00");
                Full2ndM.RenderLeadingZeros = Globals.SecondaryTimer.TimeLeft.TotalHours >= 1;

                //2nd seconds
                Full2ndS.Text = Globals.SecondaryTimer.TimeLeft.Seconds.ToString("00");
                Full2ndS.RenderLeadingZeros = Globals.SecondaryTimer.TimeLeft.TotalMinutes >= 1;
            }
            else
            {
                Full2ndD.Visible = false;
                Full2ndH.Visible = false;
                Full2ndM.Visible = false;
                Full2ndS.Visible = false;
            }

            //Total hours
            FullTotalH.RenderLeadingZeros = false;
            FullTotalH.Text = Math.Floor(span.TotalHours) >= 10000 ? "----" : Math.Floor(span.TotalHours).ToString("0000");
            FullFracH.Text  = Math.Floor(span.TotalHours) >= 10000 ? "---"  : Globals.GetDecimals(span.TotalHours, 3).ToString("000");
            FullFracH.RenderLeadingZeros = span.TotalHours >= 1;

            //Total minutes
            FullTotalM.RenderLeadingZeros = false;
            FullTotalM.Text = Math.Floor(span.TotalMinutes) >= 10000 ? "   ----" : Math.Floor(span.TotalMinutes).ToString("0000");
            FullFracM.Text  = Math.Floor(span.TotalMinutes) >= 10000 ? "  --"    : Globals.GetDecimals(span.TotalMinutes, 2).ToString("00");
            FullFracM.RenderLeadingZeros = span.TotalMinutes >= 1;

            //Total seconds
            FullTotalS.Text = Math.Floor(span.TotalSeconds) >= 10000000 ? "-----------" : Math.Floor(span.TotalSeconds).ToString("0000000");
            FullTotalS.RenderLeadingZeros = false;

            /*
             * Set fill effects
             */
            //Main days
            FullD.Progress    = (float)(Globals.PrimaryTimer.Overtime   ? ReversedTimeLeft(false) : Globals.PrimaryTimer.TimeLeft).TotalDays   / 7f;
            Full2ndD.Progress = (float)(Globals.SecondaryTimer.Overtime ? ReversedTimeLeft(true)  : Globals.SecondaryTimer.TimeLeft).TotalDays / 7f;

            //Main hours
            FullH.Progress    = (Globals.PrimaryTimer.Overtime   ? ReversedTimeLeft(false) : Globals.PrimaryTimer.TimeLeft).Hours   / 24f;
            Full2ndH.Progress = (Globals.SecondaryTimer.Overtime ? ReversedTimeLeft(true)  : Globals.SecondaryTimer.TimeLeft).Hours / 24f;

            //Main minutes
            FullM.Progress    = (Globals.PrimaryTimer.Overtime   ? ReversedTimeLeft(false) : Globals.PrimaryTimer.TimeLeft).Minutes   / 60f;
            Full2ndM.Progress = (Globals.SecondaryTimer.Overtime ? ReversedTimeLeft(true)  : Globals.SecondaryTimer.TimeLeft).Minutes / 60f;

            //Main seconds
            FullS.Progress    = (Globals.PrimaryTimer.Overtime   ? ReversedTimeLeft(false) : Globals.PrimaryTimer.TimeLeft).Seconds   / 60f;
            Full2ndS.Progress = (Globals.SecondaryTimer.Overtime ? ReversedTimeLeft(true)  : Globals.SecondaryTimer.TimeLeft).Seconds / 60f;


            //Total hours
            FullTotalH.Progress = FullH.Progress;
            FullFracH.Progress = Globals.GetDecimals((float)(isOvertime ? ReversedTimeLeft(false) : Globals.PrimaryTimer.TimeLeft).TotalHours, 3) / 1000f;

            //Total minutes
            FullTotalM.Progress = FullM.Progress;
            FullFracM.Progress = Globals.GetDecimals((float)(isOvertime ? ReversedTimeLeft(false) : Globals.PrimaryTimer.TimeLeft).TotalMinutes, 3) / 1000f;

            //Total seconds
            FullTotalS.Progress = FullS.Progress;

            /*
             * Set Leading Zero values
             */

            checkBlinkFor(Globals.PrimaryTimer,
                FullH,
                FullM,
                FullS,
                FullTotalH,
                FullTotalM,
                FullTotalS,
                FullFracH,
                FullFracM
            );

            checkBlinkFor(Globals.SecondaryTimer,
                Full2ndH,
                Full2ndM,
                Full2ndS
            );

            void checkBlinkFor(TimerConfig timer, params LabelGrayedLeadingZeros[] controls)
            {
                const int BLINK_FREQ = 50;
                const int BLINK_FREQ_STOPPED = 1000;

                if (timer.TimeLeft.TotalSeconds < 1.0 && timer.Overtime)
                {
                    if (timer.StopAtZero && DateTime.Now.Millisecond % BLINK_FREQ_STOPPED < BLINK_FREQ_STOPPED / 2)
                        blink();

                    else if (!timer.StopAtZero && DateTime.Now.Millisecond % BLINK_FREQ < BLINK_FREQ / 2)
                        blink();


                    void blink()
                    {
                        foreach (LabelGrayedLeadingZeros i in controls)
                            i.RenderLeadingZeros = true;
                    }
                }
            }
        }

        private TimeSpan ReversedTimeLeft(bool useSecondary)
        {
            return new TimeSpan(1000, 0, 0, 0) - (useSecondary ? Globals.SecondaryTimer.TimeLeft : Globals.PrimaryTimer.TimeLeft);
        }
    }
}
