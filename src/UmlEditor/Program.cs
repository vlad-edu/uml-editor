using System;
using System.Windows.Forms;

namespace UmlEditor
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            var main = new Form1();
            main.FormClosed += FormClosed;
            main.Show();
            Application.Run();
        }

        private static void FormClosed(object sender, FormClosedEventArgs e)
        {
            ((Form) sender).FormClosed -= FormClosed;
            if (Application.OpenForms.Count == 0) Application.ExitThread();
            else Application.OpenForms[0].FormClosed += FormClosed;
        }
    }
}