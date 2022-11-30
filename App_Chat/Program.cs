using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_App_Chat
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static MainForm mainForm;
        [STAThread]
        static MainForm StartForm()
        {
            mainForm = new MainForm();

            return mainForm;
        }
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(StartForm());
        }
    }
}
