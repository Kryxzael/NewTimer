using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserConsoleLib;

namespace NewTimer.Commands
{
    public class SaveTimer : Command
    {
        public override string Name { get; } = "savetimer";
        public override string HelpDescription { get; } = "Saves the primary timer as a preset file";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Path").Or()
                .Add("Type", "target", "duration").Add("Path");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            bool saveAsDuration = false;
            string path;

            if (args.Count == 1)
            {
                path = args[0];
            }
            else
            {
                saveAsDuration = args[0] == "duration";
                path = args[1];
            }

            Globals.PrimaryTimer.Serialize().ToFile(path);
            target.WriteLine("File saved");
        }
    }
}
