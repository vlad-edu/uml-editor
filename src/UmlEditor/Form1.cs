using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace UmlEditor
{
    public partial class Form1 : Form
    {
        private readonly EditorController _editorController;

        public Form1()
        {
            InitializeComponent();

            Plane.Image = new Bitmap(Plane.Width, Plane.Height);
            ControlerTab.Image = new Bitmap(ControlerTab.Width, ControlerTab.Height);
            _editorController = new EditorController(ControlerTab, Plane);
            KeyPress += _editorController.OnKeyPress;

            KeyDown += FormKeyDown;
        }

        private Form1(string config)
        {
            InitializeComponent();

            Plane.Image = new Bitmap(Plane.Width, Plane.Height);
            ControlerTab.Image = new Bitmap(ControlerTab.Width, ControlerTab.Height);
            _editorController = new EditorController(ControlerTab, Plane, config);
            KeyPress += _editorController.OnKeyPress;

            KeyDown += FormKeyDown;
        }

        private void FormKeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Control) return;

            switch (e.KeyCode)
            {
                case Keys.S:
                    e.SuppressKeyPress = true;
                    ConfigSave(sender);
                    break;
                case Keys.O:
                    e.SuppressKeyPress = true;
                    ConfigLoad(sender);
                    break;
                case Keys.N:
                    e.SuppressKeyPress = true;
                    var newForm = new Form1();
                    newForm.Show();
                    Close();
                    break;
            }
        }

        private void ConfigSave(object sender)
        {
            using var dialog = new SaveFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dialog.SupportMultiDottedExtensions = false;
            dialog.Filter = @"Configuration Files (*.config)|*.config";
            if (dialog.ShowDialog((Form)sender) != DialogResult.OK) return;

            using var writer = new StreamWriter(dialog.OpenFile());
            writer.AutoFlush = true;
            writer.Write(_editorController.Serialize());
        }

        private void ConfigLoad(object sender)
        {
            using var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dialog.SupportMultiDottedExtensions = false;
            dialog.Filter = @"Configuration Files (*.config)|*.config";
            dialog.CheckFileExists = true;
            if (dialog.ShowDialog((Form)sender) != DialogResult.OK) return;
            using var reader = new StreamReader(dialog.OpenFile());
            var config = reader.ReadToEnd();

            var newForm = new Form1(config);
            newForm.Show();
            Close();
        }
    }
}
