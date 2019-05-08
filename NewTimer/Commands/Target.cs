using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserConsoleLib;

namespace NewTimer.Commands
{
    /// <summary>
    /// Command that gets or sets the target time
    /// </summary>
    public class Target : Command
    {
        public override string Name => "target";
        public override string HelpDescription => "Gets or sets the target time of the timer";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Or().AddTrailing("New value");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            //Set target
            if (args.Count >= 1)
            {
                try
                {
                    Config.Target = DateTime.Parse(args.JoinEnd(0));
                    target.WriteLine("Target time has been updated");
                }
                catch (Exception)
                {
                    ThrowGenericError("Cannot convert input to a valid DateTime", ErrorCode.ARGUMENT_INVALID);
                }
            }

            //Read target
            target.WriteLine(Config.Target.ToString());
        }
    }
}
