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
        public FullContents()
        {
            InitializeComponent();
        }

        public void OnCountdownTick(TimeSpan span, bool isOvertime)
        {
            overwatchCircle2.BackColor = Config.GlobalBackColor;

            FullH.Text = Config.TimeLeft.Hours.ToString("00");
            FullM.Text = Config.TimeLeft.Minutes.ToString("00");
            FullS.Text = Config.TimeLeft.Seconds.ToString("00");

            FullTotalH.Text = Config.TimeLeft.TotalHours.ToString("00");
            FullTotalM.Text = Config.TimeLeft.TotalMinutes.ToString("000");
            FullTotalS.Text = Config.TimeLeft.TotalSeconds.ToString("0000000");

            FullFracH.Text = ((Math.Ceiling(Config.TimeLeft.TotalHours) - Config.TimeLeft.TotalHours) * 1000).ToString("000");
            FullFracM.Text = ((Math.Ceiling(Config.TimeLeft.TotalMinutes) - Config.TimeLeft.TotalMinutes) * 100).ToString("00");
        }
    }
}
