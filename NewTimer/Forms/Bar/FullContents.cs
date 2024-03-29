﻿using System;
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
        public FullContents()
        {
            InitializeComponent();

            //Sets foreground colors to comply with global settings
            FullD.ForeColor = Globals.GlobalForeColor;
            FullH.ForeColor = Globals.GlobalForeColor;
            FullM.ForeColor = Globals.GlobalForeColor;
            FullS.ForeColor = Globals.GlobalForeColor;

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

            //Integer portions of total hours, minutes and seconds
            FullTotalH.LeadingZerosColor = Globals.GlobalGrayedColor;
            FullTotalM.LeadingZerosColor = Globals.GlobalGrayedColor;
            FullTotalS.LeadingZerosColor = Globals.GlobalGrayedColor;

            //Fraction portions of total hours and minutes
            FullFracH.LeadingZerosColor = Globals.GlobalGrayedColor;
            FullFracM.LeadingZerosColor = Globals.GlobalGrayedColor;

            /*
             * Sets the text
             */
            //Main days
            FullD.Text = span.Days.ToString("00");
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


            //Total hours
            FullTotalH.Text = Math.Floor(span.TotalHours) >= 100 ? "BIG" : Math.Floor(span.TotalHours).ToString("00");
            FullTotalH.RenderLeadingZeros = false;
            FullFracH.Text = Globals.GetDecimals(span.TotalHours, 3).ToString("000");
            FullFracH.RenderLeadingZeros = span.TotalHours >= 1;

            //Total minutes
            FullTotalM.Text = Math.Floor(span.TotalMinutes) >= 1000 ? "BIG" : Math.Floor(span.TotalMinutes).ToString("000");
            FullTotalM.RenderLeadingZeros = false;
            FullFracM.Text = Globals.GetDecimals(span.TotalMinutes, 2).ToString("00");
            FullFracM.RenderLeadingZeros = span.TotalMinutes >= 1;

            //Total seconds
            FullTotalS.Text = Math.Floor(span.TotalSeconds).ToString("0000000");
            FullTotalS.RenderLeadingZeros = false;

            /*
             * Set fill effects
             */
            //Main days
            FullD.Progress = (float)(isOvertime ? ReversedTimeLeft() : Globals.PrimaryTimer.TimeLeft).TotalDays / 7f;

            //Main hours
            FullH.Progress = (isOvertime ? ReversedTimeLeft() : Globals.PrimaryTimer.TimeLeft).Hours  / 24f;

            //Main minutes
            FullM.Progress = (isOvertime ? ReversedTimeLeft() : Globals.PrimaryTimer.TimeLeft).Minutes / 60f;

            //Main seconds
            FullS.Progress = (isOvertime ? ReversedTimeLeft() : Globals.PrimaryTimer.TimeLeft).Seconds / 60f;


            //Total hours
            FullTotalH.Progress = FullH.Progress;
            FullFracH.Progress = Globals.GetDecimals((float)(isOvertime ? ReversedTimeLeft() : Globals.PrimaryTimer.TimeLeft).TotalHours, 3) / 1000f;

            //Total minutes
            FullTotalM.Progress = FullM.Progress;
            FullFracM.Progress = Globals.GetDecimals((float)(isOvertime ? ReversedTimeLeft() : Globals.PrimaryTimer.TimeLeft).TotalMinutes, 3) / 1000f;

            //Total seconds
            FullTotalS.Progress = FullS.Progress;

            /*
             * Set Leading Zero values
             */
            const int BLINK_FREQ = 50;
            const int BLINK_FREQ_STOPPED = 1000;

            if (Globals.PrimaryTimer.TimeLeft.TotalSeconds < 1.0 && Globals.PrimaryTimer.Overtime)
            {
                if (Globals.PrimaryTimer.StopAtZero && DateTime.Now.Millisecond % BLINK_FREQ_STOPPED < BLINK_FREQ_STOPPED / 2)
                    blink();

                else if (!Globals.PrimaryTimer.StopAtZero && DateTime.Now.Millisecond % BLINK_FREQ < BLINK_FREQ / 2)
                    blink();
                

                void blink()
                {
                    FullH.RenderLeadingZeros
                        = FullM.RenderLeadingZeros
                        = FullS.RenderLeadingZeros
                        = FullTotalH.RenderLeadingZeros
                        = FullTotalM.RenderLeadingZeros
                        = FullTotalS.RenderLeadingZeros
                        = FullFracH.RenderLeadingZeros
                        = FullFracM.RenderLeadingZeros
                        = true;
                    }
            }

            
        }

        private TimeSpan ReversedTimeLeft()
        {
            return new TimeSpan(1000, 0, 0, 0) - Globals.PrimaryTimer.TimeLeft;
        }
    }
}
