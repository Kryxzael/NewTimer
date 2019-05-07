using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.FormParts.Setup
{
    public class KnobDual : Knob
    {
        protected override string GetValueString()
        {
            return Value.ToString("00");
        }
    }
}
