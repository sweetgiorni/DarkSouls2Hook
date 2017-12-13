using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace DShookcsharp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainWindow mainWin = new mainWindow();
            try
            {
                Application.Run(mainWin);
            }
            catch
            {
                MessageBox.Show("An unknown error has occurred. XD");
            }
            
           
        }
    }
}
