using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserConsoleLib;

namespace NewTimer.Commands
{
    /// <summary>
    /// Changes between 12- and 24-hour clock in idle mode
    /// </summary>
    public class HourMode : Command
    {
        public override string Name => "hourmode";
        public override string HelpDescription => "Changed between 12-hour and 24-hour time in idle mode";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin()
                .Or().Add("Mode", "12", "24");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (args.Count == 0)
            {
                target.WriteLine("Currently using " + (Properties.Settings.Default.use24h ? "24" : "12") + "-hour mode");
                return;
            }

            switch (args[0])
            {
                case "12":
                    Properties.Settings.Default.use24h = false;
                    target.WriteLine("Clock set to 12-hour mode");
                    break;
                case "24":
                    Properties.Settings.Default.use24h = true;
                    target.WriteLine("Clock set to 24-hour mode");
                    break;
            }

            Properties.Settings.Default.Save();
        }
    }
}
