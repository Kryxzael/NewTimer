using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserConsoleLib;

namespace NewTimer.Commands
{
    /// <summary>
    /// Command that freezes the timer
    /// </summary>
    public class Freeze : Command
    {
        public override string Name => "freeze";
        public override string HelpDescription => "Freezes or unfreezes the timer";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            Globals.PrimaryTimer.Paused = !Globals.PrimaryTimer.Paused;

            target.WriteLine(Globals.PrimaryTimer.Paused ? "Frozen" : "Unfrozen");
        }
    }
}
