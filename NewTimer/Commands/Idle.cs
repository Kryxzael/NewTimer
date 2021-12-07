using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserConsoleLib;

namespace NewTimer.Commands
{
    /// <summary>
    /// Toggles free (idle) mode
    /// </summary>
    public class Idle : Command
    {
        public override string Name { get; } = "idle";
        public override string HelpDescription { get; } = "Toggles idle mode, where the timer isn't targeting any time and simply showing the time";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            Globals.PrimaryTimer.InFreeMode = !Globals.PrimaryTimer.InFreeMode;

            if (Globals.PrimaryTimer.InFreeMode)
                target.WriteLine("Idling");

            else
                target.WriteLine("Counting down");
        }
    }
}
