using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer
{
    static class Program
    {

        //public static void Main(string[] args)
        //{
        //    ColorFactory.Initialize();
        //    Color[] clr = ColorFactory.GenerateMany(7);
        //    Console.WriteLine(clr[0].ToArgb().ToString("x"));
        //}


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Forms.Setup setupForm = new Forms.Setup();

            setupForm.Show();
            if (args.Length > 0)
            {
                setupForm.LoadFile(args[0]);
            }

            Application.Run();

            TaskbarHelper.DisposeIcon();
        }
    }
}
