using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserConsoleLib;

namespace NewTimer.Commands
{
    public class EndMode : Command
    {
        public override string Name => "endmode";
        public override string HelpDescription => "Sets what happens when the title reaches zero";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("mode", "stop", "continue");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            switch (args[0])
            {
                case "stop":
                    Globals.PrimaryTimer.StopAtZero = true;
                    target.WriteLine("End mode updated to 'Stop at Zero'");
                    break;
                case "continue":
                    Globals.PrimaryTimer.StopAtZero = false;
                    target.WriteLine("End mode updated to 'Continue Counting after Zero'");
                    break;
            }
        }
    }
}
