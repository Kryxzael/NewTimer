using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTimer.FormParts
{
    public interface ICountdown
    {
        /// <summary>
        /// Called to update display of countdown item
        /// </summary>
        /// <param name="span"></param>
        void OnCountdownTick(TimeSpan span, bool isOvertime);
    }
}
