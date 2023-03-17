using NewTimer.FormParts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserConsoleLib;

namespace NewTimer.Commands
{
    /// <summary>
    /// Changes the micro-view unit selector of the current timer
    /// </summary>
    public class Unit : Command
    {
        public override string Name { get; } = "unit";
        public override string HelpDescription { get; } = "Changes the units used by the micro-view timer";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Units", MicroView.MicroViewUnitSelector.All.Select(i => i.ID).ToArray());
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            Globals.PrimaryTimer.MicroViewUnit = MicroView.MicroViewUnitSelector.All.Single(i => i.ID == args[0]);
        }
    }
}
