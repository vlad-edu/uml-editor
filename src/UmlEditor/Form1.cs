using System.Drawing;
using System.Windows.Forms;

namespace UmlEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Plane.Image = new Bitmap(Plane.Width, Plane.Height);
            ControlerTab.Image = new Bitmap(ControlerTab.Width, ControlerTab.Height);
            EditorControler app = new(ControlerTab, Plane);
            KeyPress += app.OnKeyPress;
        }
    }
}
