using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.Forms.Circle
{
    [DesignerCategory("")]
    public class Circle : TimerFormBase
    {
        public override Control BarTabContents()
        {
            return new CircleContents();
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
            return "Cir";
        }
    }
}
