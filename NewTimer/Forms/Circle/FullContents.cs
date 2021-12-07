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

        public void OnCountdownTick(TimeSpan span, bool isOvertime)
        {
            overwatchCircle2.BackColor = Globals.GlobalBackColor;

            FullH.Text = Globals.PrimaryTimer.TimeLeft.Hours.ToString("00");
            FullM.Text = Globals.PrimaryTimer.TimeLeft.Minutes.ToString("00");
            FullS.Text = Globals.PrimaryTimer.TimeLeft.Seconds.ToString("00");

            FullTotalH.Text = Globals.PrimaryTimer.TimeLeft.TotalHours.ToString("00");
            FullTotalM.Text = Globals.PrimaryTimer.TimeLeft.TotalMinutes.ToString("000");
            FullTotalS.Text = Globals.PrimaryTimer.TimeLeft.TotalSeconds.ToString("0000000");

            FullFracH.Text = ((Math.Ceiling(Globals.PrimaryTimer.TimeLeft.TotalHours) - Globals.PrimaryTimer.TimeLeft.TotalHours) * 1000).ToString("000");
            FullFracM.Text = ((Math.Ceiling(Globals.PrimaryTimer.TimeLeft.TotalMinutes) - Globals.PrimaryTimer.TimeLeft.TotalMinutes) * 100).ToString("00");
        }
    }
}
