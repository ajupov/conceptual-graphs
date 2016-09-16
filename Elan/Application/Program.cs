using System;
using Elan.Forms;

namespace Elan.Application
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.DoEvents();
            System.Windows.Forms.Application.Run(new MainForm());
        }
    }
}