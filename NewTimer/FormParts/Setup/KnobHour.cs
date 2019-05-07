using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.FormParts.Setup
{
    public class KnobHour : Knob
    {
        protected override string GetValueString()
        {
            if (Config.Use24HourSelector)
            {
                return Value.ToString("00");
            }

            if (Value == 0)
            {
                return "12a";
            }
            else if (Value < 12)
            {
                return Value + "a";
            }
            else if (Value == 12)
            {
                return "12p";
            }
            else
            {
                return (Value - 12) + "p";
            }

        }
    }
}
