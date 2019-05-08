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

        // Used to keep timer frozen
        private System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer() { Interval = 10 };
        private System.Diagnostics.Stopwatch _deltaTime = new System.Diagnostics.Stopwatch();

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        public Freeze()
        {
            _timer.Tick += OnTimerTick;
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            _deltaTime.Restart();
            target.WriteLine((_timer.Enabled = !_timer.Enabled) ? "Frozen" : "Unfrozen");
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            Config.Target += _deltaTime.Elapsed;
            _deltaTime.Restart();
        }
    }
}
