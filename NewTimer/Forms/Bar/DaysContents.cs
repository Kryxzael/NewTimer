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
            autoLabel1.GetText = () => Config.GetTimeLeft().Days.ToString();
            autoLabel2.GetText = () => "." + Config.GetDecimals(Config.GetTimeLeft().TotalDays, 5).ToString("00000") + " " + (Math.Abs(Math.Floor(Config.GetTimeLeft().TotalDays)) == 1 ? "day" : "days");
        }
    }
}
