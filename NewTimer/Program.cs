using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserConsoleLib;

namespace NewTimer
{
    /// <summary>
    /// Main entrypoint of application
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Boilerplate code
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Create setup window
            Forms.Setup setupForm = new Forms.Setup();

            //Register commands
            Command.Register<Commands.Target>();
            Command.Register<Commands.TimeLeft>();
            Command.Register<Commands.ColorScheme>();
            Command.Register<Commands.Freeze>();
            Command.Register<Commands.EndMode>();
            Command.Register<Commands.StartTime>();
            Command.Register<Commands.Idle>();
            Command.Register<Commands.Unit>();
            Command.Register<Commands.SaveTimer>();
            Command.Register<Commands.LoadTimer>();
            Command.Register<Commands.HourMode>();

            string startingFileName = null;
            bool noSetup = false;
            DateTime? primaryTarget = null;
            DateTime? secondaryTarget = null;
            ref DateTime? currentTarget = ref primaryTarget;

            for (int i = 0; i < args.Length; i++)
            {
                string currentArg = args[i];

                switch (currentArg)
                {
                    case "-n":
                    case "--no-setup":
                        noSetup = true;
                        break;

                    case "-t":
                    case "--target":
                        i++;

                        if (i >= args.Length)
                        {
                            Console.WriteLine("Expected target time after -t or --target", "Argument error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        currentArg = args[i];
                        
                        if (!DateTime.TryParse(currentArg, out DateTime parsedTarget))
                        {
                            MessageBox.Show("Could not parse target date", "Argument error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        currentTarget = parsedTarget;

                        break;

                    case "--duration":
                    case "-d":
                        i++;

                        if (i >= args.Length)
                        {
                            MessageBox.Show("Expected duration after -d or --duration", "Argument error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        currentArg = args[i];

                        if (!TimeSpan.TryParse(currentArg, out TimeSpan duration))
                        {
                            MessageBox.Show("Could not parse duration", "Argument error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        currentTarget = DateTime.Now + duration;
                        break;

                    case "-2":
                        currentTarget = ref secondaryTarget;
                        break;

                    case "-2t":
                        currentTarget = ref secondaryTarget;
                        goto case "-t";

                    case "-2d":
                        currentTarget = ref secondaryTarget;
                        goto case "-d";

                    case "--help":
                    case "-h":
                    case "-?":
                        MessageBox.Show(@"Command line arguments:
    --target|-t <target>: Set the target time
    --duration|-d <duration>: Set the target time as a duration from now
    -2: Subsequent arguments will apply to secondary timer
    --no-setup|-n: Starts the timer in idle mode without showing setup
    --help|-h|-?: Show this dialog", "Command line argument help", MessageBoxButtons.OK);
                        return;
                    default:
                        startingFileName = currentArg;
                        break;
                }
            }

            //If there is an argument, treat it as a file path and open it
            if (startingFileName != null)
            {
                setupForm.LoadFile(startingFileName);
                setupForm.StartWithCurrentSettings();
            }
            else if (primaryTarget != null)
            {
                Globals.StartTimer(
                    target: currentTarget ?? DateTime.Now,
                    stopAtZero: false,
                    colorScheme: Globals.ColorSchemes[0],
                    freeMode: currentTarget == null,
                    startedFromDuration: false,
                    closingForm: null,
                    secondaryTarget: secondaryTarget
                );
            }
            else if (noSetup)
            {
                Globals.StartTimer(
                    target: DateTime.Now,
                    stopAtZero: false,
                    colorScheme: Globals.ColorSchemes[0],
                    freeMode: true,
                    startedFromDuration: false,
                    closingForm: null
                );
            }
            else
            {
                //Show window and run message pump
                setupForm.Show();
            }

            Application.Run();


            //Dispose taskbar icon before shutdown
            TaskbarHelper.DisposeIcon();
        }
    }
}
