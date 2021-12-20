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
        /// <summary>
        /// Sets the title of a form to a standardized format that mimics the timer
        /// </summary>
        /// <param name="f">Form to set title of</param>
        public static void SetTitle(Form f)
        {
            TimeSpan tl = Globals.PrimaryTimer.TimeLeft;
            string title;

            //Free mode, show current date and time
            if (Globals.PrimaryTimer.InFreeMode)
            {
                title = DateTime.Now.ToString();
            }

            //Less than one minute, show amount of seconds left
            else if (tl.TotalMinutes < 1)
            {
                title = NumberToWord(tl.Seconds, true) + (tl.Seconds == 1 ? " second" : " seconds");
            }

            //Less than one hour, show amount of minutes left
            else if (tl.TotalHours < 1)
            {
                if (tl.Minutes == 15)
                {
                    title = "A quarter hour";
                }
                else if (tl.Minutes == 30)
                {
                    title = "Half an hour";
                }    
                else if (tl.Minutes == 45)
                {
                    title = "Three quarter hour";
                }
                else
                {
                    title = NumberToWord(tl.Minutes, true) + (tl.Minutes == 1 ? " minute" : " minutes");
                }

                if (tl.Seconds != 0)
                    title += ", " + tl.Seconds.ToString() + " sec";
            }

            //Less than one day, show amount of hours left
            else if (tl.TotalDays < 1)
            {
                if (tl.Hours == 12)
                {
                    title = "Half a day";
                }
                else
                {
                    title = NumberToWord(tl.Hours, true) + (tl.Hours == 1 ? " hour" : " hours");
                }

                if (tl.Minutes != 0)
                    title += ", " + tl.Minutes.ToString() + " min";
            }

            //Less than one week, show amount of days left
            else if (tl.TotalDays < 7)
            {
                title = NumberToWord(tl.Days, true) + (tl.Days == 1 ? " day" : " days");

                if (tl.Hours != 0)
                    title += ", " + tl.Hours.ToString() + (tl.Hours == 1 ? " hr" : " hrs");
            }

            //Less than one year, show amount in weeks
            else if (tl.TotalDays < 360)
            {
                int weeks = tl.Days / 7;
                int excessDays = tl.Days % 7;

                title = NumberToWord(weeks, true) + (weeks == 1 ? " week" : " weeks");

                if (excessDays != 0)
                    title += ", " + excessDays.ToString() + (excessDays == 1 ? " day" : " days");
            }

            //More than a year, show a fallback string
            else
            {
                title = "A long time";
            }


            //We are in overtime, add "ago" to the end of the title
            if (Globals.PrimaryTimer.Overtime)
            {
                title += " ago";
            }

            f.Text = title;
        }

        /// <summary>
        /// Sets the taskbar icon's progress information to mimic the timer
        /// </summary>
        /// <param name="f"></param>
        public static void SetProgress(Form f)
        {
            TimeSpan tl = Globals.PrimaryTimer.TimeLeft;

            //Get handle of form
            IntPtr handle = f.Handle;

            //Free mode -> No progress state
            if (Globals.PrimaryTimer.InFreeMode)
            {
                TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.NoProgress);
                return;
            }

            //Overtime -> Indeterminate state (Value does not apply)
            if (Globals.PrimaryTimer.Overtime)
            {
                TaskbarHelper.SetState(handle, TaskbarHelper.TaskbarStates.Indeterminate);
                return;
            }

            //Buffer for storing precalculated values
            double v = 60 - tl.Minutes;

            //1m -> red, yellow and green
            if (tl.TotalMinutes < 1)
            {
                v = 60 - tl.Seconds;
                switch (tl.Seconds % 3)
                {
                    case 0:
                        setProgress(TaskbarHelper.TaskbarStates.Normal, v, 60);
                        break;
                    case 1:
                        setProgress(TaskbarHelper.TaskbarStates.Paused, v, 60);
                        break;
                    case 2:
                        setProgress(TaskbarHelper.TaskbarStates.Error, v, 60);
                        break;
                }
            }

            //1h -> green
            else if (tl.TotalHours < 1) setProgress(TaskbarHelper.TaskbarStates.Normal, v, 59);

            //2h -> yellow
            else if (tl.TotalHours < 2) setProgress(TaskbarHelper.TaskbarStates.Paused, v, 60);

            //3h -> red
            else if (tl.TotalHours < 3) setProgress(TaskbarHelper.TaskbarStates.Error, v, 60);

            //4h -> yellow, green
            else if (tl.TotalHours < 4) setProgress(tl.Seconds % 2 == 0 ?
                TaskbarHelper.TaskbarStates.Paused :
                TaskbarHelper.TaskbarStates.Normal,
                v, 60);

            //5h -> red, green
            else if (tl.TotalHours < 5) setProgress(tl.Seconds % 2 == 0 ?
                TaskbarHelper.TaskbarStates.Error :
                TaskbarHelper.TaskbarStates.Normal,
                v, 60);

            //6h -> red, yellow
            else if (tl.TotalHours < 6) setProgress(tl.Seconds % 2 == 0 ?
                TaskbarHelper.TaskbarStates.Error :
                TaskbarHelper.TaskbarStates.Paused,
                v, 60);

            //1d -> solid green
            else if (tl.TotalDays < 1) setProgress(TaskbarHelper.TaskbarStates.Normal, 1, 1);

            //7d -> solid yellow
            else if (tl.TotalDays < 7) setProgress(TaskbarHelper.TaskbarStates.Paused, 1, 1);

            //>7d -> solid red
            else setProgress(TaskbarHelper.TaskbarStates.Error, 1, 1);

            //Shorthand for TaskbarHelper.SetState and TaskbarHelper.SetValue
            /* local */ void setProgress(TaskbarHelper.TaskbarStates state, double value, double maxValue)
            {
                TaskbarHelper.SetState(handle, state);
                TaskbarHelper.SetValue(handle, value, maxValue);
            }

        }

        /// <summary>
        /// Gets the English name for the provided number if it is less than ten
        /// </summary>
        private static string NumberToWord(int a, bool capitalize)
        {
            string output;
            switch (a)
            {
                case 0:
                    output = "Zero";
                    break;
                case 1:
                    output = "One";
                    break;
                case 2:
                    output = "Two";
                    break;
                case 3:
                    output = "Three";
                    break;
                case 4:
                    output = "Four";
                    break;
                case 5:
                    output = "Five";
                    break;
                case 6:
                    output = "Six";
                    break;
                case 7:
                    output = "Seven";
                    break;
                case 8:
                    output = "Eight";
                    break;
                case 9:
                    output = "Nine";
                    break;
                case 10:
                    output = "Ten";
                    break;
                default:
                    output = a.ToString();
                    break;
            }

            if (capitalize)
                return output;

            return output.ToLower();
        }
    }
}
