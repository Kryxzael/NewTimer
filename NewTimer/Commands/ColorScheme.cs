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
                .Add("Index", Range.From(0).To(Globals.ColorSchemes.GetUpperBound(0)), true).Or()
                .Add("R", Range.From(0).To(255), true).Add("G", Range.From(0).To(255), true).Add("B", Range.From(0).To(255), true).Or()
                .Add("sync", "sync");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            //Read the color scheme
            if (args.Count == 0)
            {
                target.WriteLine("[" + Globals.ColorSchemes.TakeWhile(i => i != Globals.PrimaryTimer.ColorScheme).Count() + "] " + Globals.PrimaryTimer.ColorScheme.Name);
                return;
            }

            //Write the color scheme
            else if (args.Count == 1)
            {
                if (args[0] == "sync")
                {
                    foreach (TimeSpan i in Globals.PrimaryTimer.BarSettings.Keys)
                    {
                        Globals.PrimaryTimer.BarSettings[i].FillColor     = Globals.SecondaryTimer.BarSettings[i].FillColor;
                        Globals.PrimaryTimer.BarSettings[i].OverflowColor = Globals.SecondaryTimer.BarSettings[i].OverflowColor;
                    }

                    Array.Copy(Globals.SecondaryTimer.AnalogColors, Globals.PrimaryTimer.AnalogColors, Globals.PrimaryTimer.AnalogColors.Length);
                    Globals.PrimaryTimer.MicroViewColor = Globals.SecondaryTimer.MicroViewColor;

                    target.WriteLine("Synchronized color schemes");
                    return;
                }
                else
                {
                    Globals.PrimaryTimer.ColorScheme = Globals.ColorSchemes[args.ToInt(0)];
                }
            }

            //Write color directly
            else
            {
                Globals.PrimaryTimer.ColorScheme = new Schemes.SchemeCustom(
                    name: "$custom_scheme", 
                    looptype: Schemes.SchemeCustom.LoopType.Ceiling, 
                    colors: System.Drawing.Color.FromArgb(
                        red:   args.ToInt(0), 
                        green: args.ToInt(1), 
                        blue:  args.ToInt(2)
                    )
                );
            }

            Globals.PrimaryTimer.ColorizeTimerBar();
            target.WriteLine("Color scheme has been updated to '" + Globals.PrimaryTimer.ColorScheme.Name + "'");
        }
    }
}
