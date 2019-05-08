using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.FormParts.Setup
{
    /// <summary>
    /// A knob designed to display its value as the hour component of a datetime, respecting Config.Use24HourSelector
    /// </summary>
    public class KnobHour : KnobDual
    {
        protected override string GetValueString()
        {
            //Use the normal format for 24 hour time
            if (Config.Use24HourSelector)
            {
                base.GetValueString();
            }

            /*
             * 12 hour time is enabled
             */

            //00 is 12 a.m.
            if (Value == 0)
            {
                return "12a";
            }

            //Anything between 0 and 12 can be treated as a.m. time
            else if (Value < 12)
            {
                return Value + "a";
            }

            //12 is 12 p.m.
            else if (Value == 12)
            {
                return "12p";
            }

            //Anything else is p.m. time
            else
            {
                return (Value - 12) + "p";
            }

        }
    }
}
