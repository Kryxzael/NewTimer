using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.FormParts.Setup
{
    /// <summary>
    /// A knob that displays it's value with two digits (leading zeros)
    /// </summary>
    public class KnobDual : Knob
    {
        protected override string GetValueString()
        {
            return Value.ToString("00");
        }
    }
}
