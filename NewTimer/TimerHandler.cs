using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer
{
    public class TimerHandler : IDisposable
    {
        public Form Form { get; }
        Timer t = new Timer() { Interval = 1000 };

        public TimerHandler(Form frm)
        {
            Form = frm;
            t.Tick += (s, e) => SearchAndUpdateControl(Form);
            Form.Shown += (s, e) =>
            {
                Start();
                Form.Location = new System.Drawing.Point(Screen.PrimaryScreen.WorkingArea.Right - Form.Width, Screen.PrimaryScreen.WorkingArea.Bottom - Form.Height);
                Form.TopMost = true;
                Form.ShowIcon = false;
                Form.ShowInTaskbar = false;
                Form.Text = "Timer";
            };
            Form.FormClosed += (s, e) => Application.Exit();

        }

        public void Start()
        {
            t.Start();
            SearchAndUpdateControl(Form);
        }

        public void Stop()
        {
            t.Stop();
        }

        void SearchAndUpdateControl(Control ctrl)
        {
            foreach (Control i in ctrl.Controls)
            {
                SearchAndUpdateControl(i);
            }

            if (ctrl is FormParts.ICountdown)
            {
                (ctrl as FormParts.ICountdown).OnCountdownTick(Config.GetTimeLeft(), Config.Overtime);
            }
        }

        public void Dispose()
        {
            t.Dispose();
        }
    }
}
