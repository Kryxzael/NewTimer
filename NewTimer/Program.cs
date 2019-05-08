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

            //If there is an argument, treat is as a file path and open it
            if (args.Length > 0)
            {
                setupForm.LoadFile(args[0]);
            }

            //Show window and run message pump
            setupForm.Show();
            Application.Run();

            //Dispose taskbar icon before shutdown
            TaskbarHelper.DisposeIcon();
        }
    }
}
