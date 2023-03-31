using CleanNodeTree;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserConsoleLib;

namespace NewTimer.Commands
{
    public class LoadTimer : Command
    {
        public override string Name { get; } = "loadtimer";
        public override string HelpDescription { get; } = "Overwrites the primary timer with the contents of a preset file";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Path");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            Globals.PrimaryTimer.Deserialize(HierarchyNode.FromFile(args[0]));
        }
    }
}
