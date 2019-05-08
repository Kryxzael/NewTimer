using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.Forms.Bar
{
    /// <summary>
    /// The default timer window
    /// </summary>
    [DesignerCategory("")]
    public class Bar : TimerFormBase
    {
        public override Control BarTabContents()
        {
            return new BarContents();
        }

        public override Control DaysTabContents()
        {
            return new DaysContents();
        }

        public override Control FullTabContents()
        {
            return new FullContents();
        }

        public override string GetBarTabName()
        {
            return "Bar";
        }
    }
}
