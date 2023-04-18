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

namespace NewTimer.Forms.Circle
{
    public partial class FullContents : UserControl, ICountdown
    {
        public FullContents(bool simpleMode)
        {
            InitializeComponent();

            overwatchCircle2.ProgressMode = simpleMode;
        }

        public void OnCountdownTick(TimeSpan span, TimeSpan secondSpan, bool isOvertime)
        {
            overwatchCircle2.BackColor = Globals.GlobalBackColor;

            foreach (Control i in Controls)
            {
                i.ForeColor = Globals.GlobalForeColor;
            }

            FullH.Text = Globals.PrimaryTimer.TimeLeft.Hours.ToString("00");
            FullM.Text = Globals.PrimaryTimer.TimeLeft.Minutes.ToString("00");
            FullS.Text = Globals.PrimaryTimer.TimeLeft.Seconds.ToString("00");

            FullTotalH.Text = Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalHours).ToString("00");
            FullTotalM.Text = Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalMinutes).ToString("000");
            FullTotalS.Text = Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalSeconds).ToString("0000000");

            FullFracH.Text = Globals.GetDecimals(Globals.PrimaryTimer.TimeLeft.TotalHours, 3).ToString("000");
            FullFracM.Text = Globals.GetDecimals(Globals.PrimaryTimer.TimeLeft.TotalMinutes, 2).ToString("00");
        }
    }
}
