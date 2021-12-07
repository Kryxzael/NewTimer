using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.Forms.Bar
{
    public partial class DaysContents : UserControl
    {
        public DaysContents()
        {
            InitializeComponent();

            //Initializes autolabels
            autoLabel1.GetText = () => Globals.PrimaryTimer.TimeLeft.Days.ToString();
            autoLabel2.GetText = () => "." + Globals.GetDecimals(Globals.PrimaryTimer.TimeLeft.TotalDays, 5).ToString("00000") + " " + (Math.Abs(Math.Floor(Globals.PrimaryTimer.TimeLeft.TotalDays)) == 1 ? "day" : "days");
        }
    }
}
