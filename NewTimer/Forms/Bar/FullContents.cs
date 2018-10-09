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
        public FullContents()
        {
            InitializeComponent();
            FullH.ForeColor = Config.GlobalForeColor;
            FullM.ForeColor = Config.GlobalForeColor;
            FullS.ForeColor = Config.GlobalForeColor;

            FullTotalH.ForeColor = Config.GlobalForeColor;
            FullTotalM.ForeColor = Config.GlobalForeColor;
            FullTotalS.ForeColor = Config.GlobalForeColor;

            FullFracM.ForeColor = Config.GlobalForeColor;
            FullFracM.ForeColor = Config.GlobalForeColor;

            FullH.HighlightColor = ColorTranslator.FromHtml("#ff8888");
            FullM.HighlightColor = ColorTranslator.FromHtml("#88ccff");
            FullS.HighlightColor = ColorTranslator.FromHtml("#ffdd88");

            FullTotalH.HighlightColor = FullH.HighlightColor;
            FullTotalM.HighlightColor = FullM.HighlightColor;
            FullTotalS.HighlightColor = FullS.HighlightColor;

            FullFracH.HighlightColor = FullH.HighlightColor;
            FullFracM.HighlightColor = FullM.HighlightColor;

            FullH.LeadingZerosColor = Config.GlobalGrayedColor;
            FullM.LeadingZerosColor = Config.GlobalGrayedColor;
            FullS.LeadingZerosColor = Config.GlobalGrayedColor;

            FullTotalH.LeadingZerosColor = Config.GlobalGrayedColor;
            FullTotalM.LeadingZerosColor = Config.GlobalGrayedColor;
            FullTotalS.LeadingZerosColor = Config.GlobalGrayedColor;

            FullFracH.LeadingZerosColor = Config.GlobalGrayedColor;
            FullFracM.LeadingZerosColor = Config.GlobalGrayedColor;
        }

        public void OnCountdownTick(TimeSpan span, bool isOvertime)
        {
            FullH.Text = span.Hours.ToString("00");
            FullH.RenderLeadingZeros = span.TotalDays >= 1;

            FullM.Text = span.Minutes.ToString("00");
            FullM.RenderLeadingZeros = span.TotalHours >= 1;

            FullS.Text = span.Seconds.ToString("00");
            FullS.RenderLeadingZeros = span.TotalMinutes >= 1;

            FullTotalH.Text = Math.Floor(span.TotalHours) >= 100 ? "BIG" : Math.Floor(span.TotalHours).ToString("00");
            FullTotalM.Text = Math.Floor(span.TotalMinutes) >= 1000 ? "BIG" : Math.Floor(span.TotalMinutes).ToString("000");
            FullTotalS.Text = Math.Floor(span.TotalSeconds).ToString("0000000");

            FullFracH.Text = Config.GetDecimals(span.TotalHours, 3).ToString("000");
            FullFracH.RenderLeadingZeros = span.TotalHours >= 1;

            FullFracM.Text = Config.GetDecimals(span.TotalMinutes, 2).ToString("00");
            FullFracM.RenderLeadingZeros = span.TotalMinutes >= 1;

            //Set progress
            FullH.Progress = (isOvertime ? ReversedTimeLeft() : Config.GetTimeLeft()).Hours  / 24f;
            FullM.Progress = (isOvertime ? ReversedTimeLeft() : Config.GetTimeLeft()).Minutes / 60f;
            FullS.Progress = (isOvertime ? ReversedTimeLeft() : Config.GetTimeLeft()).Seconds / 60f;

            FullTotalH.Progress = FullH.Progress;
            FullTotalM.Progress = FullM.Progress;
            FullTotalS.Progress = FullS.Progress;

            FullFracH.Progress = Config.GetDecimals((float)(isOvertime ? ReversedTimeLeft() : Config.GetTimeLeft()).TotalHours, 3) / 1000f;
            FullFracM.Progress = Config.GetDecimals((float)(isOvertime ? ReversedTimeLeft() : Config.GetTimeLeft()).TotalMinutes, 3) / 1000f;
        }

        private TimeSpan ReversedTimeLeft()
        {
            return new TimeSpan(1000, 0, 0, 0) - Config.GetTimeLeft();
        }
    }
}
