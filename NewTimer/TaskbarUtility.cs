using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer
{
    /// <summary>
    /// Updates the taskbar
    /// </summary>
    public static class TaskbarUtility
    {
        public static void SetTitle(Form f)
        {
            if (Config.Overtime)
            {
                f.Text = "Overtime";
            }
            else
            {
                TimeSpan tl = Config.GetTimeLeft();

                if (tl.TotalMinutes < 1)
                {
                    f.Text = tl.Seconds.ToString() + (tl.Seconds == 1 ? " second" : " seconds");
                }
                else if (tl.TotalHours < 1)
                {
                    f.Text = tl.Minutes.ToString() + (tl.Minutes == 1 ? " minute" : " minutes");
                }
                else if (tl.TotalDays < 1)
                {
                    f.Text = tl.Hours.ToString() + (tl.Hours == 1 ? " hour" : " hours");
                }
                else if (tl.TotalDays < 360)
                {
                    f.Text = tl.Days + (tl.Days == 1 ? " day" : " days");
                }
                else
                {
                    f.Text = "A long time";
                }
            }
            
        }

        public static void SetProgress(Form f)
        {
            TimeSpan tl = Config.GetTimeLeft();
            IntPtr handle = f.Handle;

            if (Config.Overtime)
            {
                TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Indeterminate);
            }
            else if (tl.TotalMinutes < 1)
            {
                //red, yellow, green
                switch (tl.Seconds % 3)
                {
                    case 0:
                        TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Normal);
                        break;
                    case 1:
                        TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Paused);
                        break;
                    case 2:
                        TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Error);
                        break;
                }

                TaskbarHelper.SetValue(handle, 60 - tl.Seconds, 60);
            }

            else if (tl.TotalHours < 1)
            {
                //green
                TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Normal);
                TaskbarHelper.SetValue(handle, 60 - tl.Minutes, 59);
            }
            else if (tl.TotalHours < 2)
            {
                //yellow
                TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Paused);
                TaskbarHelper.SetValue(handle, 60 - tl.Minutes, 60);
            }
            else if (tl.TotalHours < 3)
            {
                //red
                TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Error);
                TaskbarHelper.SetValue(handle, 60 - tl.Minutes, 60);
            }
            else if (tl.TotalHours < 4)
            {
                //yellow, green
                if (tl.Seconds % 2 == 0)
                {
                    TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Paused);
                }
                else
                {
                    TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Normal);
                }

                TaskbarHelper.SetValue(handle, 60 - tl.Minutes, 60);
            }
            else if (tl.TotalHours < 5)
            {
                //red, green
                if (tl.Seconds % 2 == 0)
                {
                    TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Error);
                }
                else
                {
                    TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Normal);
                }

                TaskbarHelper.SetValue(handle, 60 - tl.Minutes, 60);
            }
            else if (tl.TotalHours < 6)
            {
                //red, yellow
                if (tl.Seconds % 2 == 0)
                {
                    TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Error);
                }
                else
                {
                    TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Paused);
                }

                TaskbarHelper.SetValue(handle, 60 - tl.Minutes, 60);
            }
            else if (tl.TotalDays < 1)
            {
                //Green (frozen)
                TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Normal);
                TaskbarHelper.SetValue(handle, 1, 1);
            }
            else if (tl.TotalDays < 7)
            {
                //Yellow (frozen)
                TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Paused);
                TaskbarHelper.SetValue(handle, 1, 1);
            }
            else
            {
                //Red (frozen)
                TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Error);
                TaskbarHelper.SetValue(handle, 1, 1);
            }
        }
    }
}
