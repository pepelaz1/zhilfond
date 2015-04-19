using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Zhilfond.Keys
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
            LoginForm frm = new LoginForm();
            if ( frm.ShowDialog() == DialogResult.OK)
                Application.Run(new MainForm(frm));
        }
    }
}
