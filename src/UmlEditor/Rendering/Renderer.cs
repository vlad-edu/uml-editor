using System.Drawing;
using System.Windows.Forms;
using UmlEditor.Geometry;

namespace UmlEditor.Rendering
{
    public class Renderer
    {
        private PictureBox plane;
        public Vector Origin { get; set; }
        private float width;
        private float height;
        public Color ClearColor = Color.SlateGray;
        public float Scale { get; set; } = 1f;
        public Renderer(PictureBox pic)
        {
            plane = pic;
            width = plane.Width;
            height = plane.Height;
            Origin = new Vector(width / 2, height / 2);
        }
        public void DrawLine(Vector from, Vector to, Pen draw_pen)
        {
            using (Graphics g = GetGraphics())
            {
                g.ScaleTransform(Scale, Scale);
                g.DrawLine(draw_pen, from + Origin, to + Origin);
            }
        }
        public void DrawRectangle(Vector position, float width, float height, Pen draw_pen, Brush fill_brush = null)
        {
            Rectangle rect = new((int)(position.X + Origin.X), (int)(position.Y + Origin.Y), (int)width, (int)height);
            using (Graphics g = GetGraphics())
            {
                g.ScaleTransform(Scale, Scale);
                if (fill_brush != null)
                {
                    g.FillRectangle(fill_brush, rect);
                }
                g.DrawRectangle(draw_pen, rect);
            }
        }

        public void DrawText(PointF position, string text, Font font, Brush brush)
        {
            using (Graphics g = GetGraphics())
            {
                g.ScaleTransform(Scale, Scale);
                g.DrawString(text, font, brush, position + Origin);
            }
        }
        public void Render()
        {
            plane.Refresh();
        }
        public void Clear()
        {
            GetGraphics().Clear(ClearColor);
        }

        private Graphics GetGraphics()
        {
            return Graphics.FromImage(plane.Image);
        }

        public static readonly int TextWidthGap = 9;
        public static readonly int TextHeightGap = 15;
        public static readonly int SingleTextHeight = 18;
        public static readonly int SingleTextWidth = 13;
        public static int GetTextWidth(int text_length)
        {
            return 13 + (text_length - 1) * 9;
        }
        public void Resize()
        {
            plane.Image = new Bitmap(plane.Width, plane.Height);
        }
    }
}
