using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserConsoleLib;

namespace NewTimer.Commands
{
    public class StartTime : Command
    {
        public override string Name { get; } = "starttime";
        public override string HelpDescription { get; } = "Sets the start time of the Countdown widget";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Or()
                .Add("mode", "stamp", "span").AddTrailing("timestamp/span");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (args.Count == 0)
            {
                target.WriteLine(Config.StartTime.ToString());
                return;
            }

            switch (args[0])
            {
                case "stamp":
                    try
                    {
                        Config.StartTime = DateTime.Parse(args.JoinEnd(1));
                    }
                    catch (Exception)
                    {
                        ThrowGenericError("Cannot convert input to a valid DateTime", ErrorCode.ARGUMENT_INVALID);
                    }

                    break;
                case "span":

                    try
                    {
                        string arg = args[0].ToLower();

                        if (arg.EndsWith("h"))
                        {
                            Config.StartTime = DateTime.Now.AddHours(double.Parse(args[1].TrimEnd('h'), NumberStyles.Float, CultureInfo.InvariantCulture));
                        }
                        else if (arg.EndsWith("m"))
                        {
                            Config.StartTime = DateTime.Now.AddMinutes(double.Parse(args[1].TrimEnd('m'), NumberStyles.Float, CultureInfo.InvariantCulture));
                        }
                        else if (arg.EndsWith("s"))
                        {
                            Config.StartTime = DateTime.Now.AddSeconds(double.Parse(args[1].TrimEnd('s'), NumberStyles.Float, CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            Config.StartTime = DateTime.Now + TimeSpan.Parse(args.JoinEnd(1));
                        }
                    }
                    catch (Exception)
                    {
                        ThrowGenericError("Cannot convert input to a valid TimeSpan", ErrorCode.ARGUMENT_INVALID);
                    }

                    break;

            }
        }
    }
}
