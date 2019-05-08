using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserConsoleLib;

namespace NewTimer.Commands
{
    /// <summary>
    /// Command that gets or sets the color scheme by index
    /// </summary>
    public class ColorScheme : Command
    {
        public override string Name => "colorscheme";
        public override string HelpDescription => "Gets or sets the color scheme index";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Or()
                .Add("Index", Range.From(0).To(Config.ColorSchemes.GetUpperBound(0)), true);
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            //Read the color scheme
            if (args.Count == 0)
            {
                target.WriteLine("[" + Config.ColorSchemes.TakeWhile(i => i != Config.ColorScheme).Count() + "] " + Config.ColorScheme.Name);
            }

            //Write the color scheme
            else
            {
                Config.ColorScheme = Config.ColorSchemes[args.ToInt(0)];
                Config.ColorizeTimerBar();
                target.WriteLine("Color scheme has been updated to '" + Config.ColorScheme.Name + "'");
            }
        }
    }
}
