using System.Drawing;
using System.Drawing.Drawing2D;
using UmlEditor.Geometry;

namespace UmlEditor.Rendering.RenderingElements
{
    public class LineRenderElement : IRenderElement
    {
        public Vector StartPoint { get; set; }
        public Vector EndPoint { get; set; }
        private Pen DrawPen { get; set; }
        private int width;
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                DrawPen.Width = width; 
            }
        }
        private Color color;
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                DrawPen.Color = color;
            }
        }
        private DashStyle dashStyle;
        public DashStyle DashStyle
        {
            get
            {
                return dashStyle;
            }
            set
            {
                dashStyle = value;
                DrawPen.DashStyle = dashStyle;
            }
        }
        private LineCap startCap;
        public LineCap StartCap
        {
            get
            {
                return startCap;
            }
            set
            {
                startCap = value;
                DrawPen.StartCap = startCap;
            }
        }
        private LineCap endCap;
        public LineCap EndCap
        {
            get
            {
                return endCap;
            }
            set
            {
                endCap = value;
                DrawPen.EndCap = endCap;
            }
        }

        public LineRenderElement(Vector from, Vector to, int width, Color color)
        {
            DrawPen = new Pen(color, width);
            StartPoint = from;
            EndPoint = to;
            Width = width;
            EndCap = LineCap.NoAnchor;
            StartCap = LineCap.NoAnchor;
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawLine(StartPoint, EndPoint, DrawPen);
        }
    }
}
