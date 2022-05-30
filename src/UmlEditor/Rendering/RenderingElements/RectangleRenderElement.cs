using System.Drawing;
using UmlEditor.Geometry;

namespace UmlEditor.Rendering.RenderingElements
{
    public class RectangleRenderElement : IRenderElement
    {
        public Vector Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        private Pen DrawPen { get; set; }
        private SolidBrush FillBrush { get; set; }
        private Color borderColor;
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
                DrawPen.Color = borderColor;
            }
        }
        private Color fillColor;
        public Color FillColor
        {
            get
            {
                return fillColor;
            }
            set
            {
                fillColor = value;
                FillBrush.Color = fillColor;
            }
        }
        private float borderWidth;
        public float BorderWidth
        {
            get
            {
                return borderWidth;
            }
            set
            {
                borderWidth = value;
                DrawPen.Width = borderWidth;
            }
        }
        public RectangleRenderElement(Vector position, float width, float height, Color fill_color, Color border_color, int border_width = 1)
        {
            DrawPen = new Pen(border_color, border_width);
            FillBrush = new SolidBrush(fill_color);
            Position = position;
            Width = width;
            Height = height;
            FillColor = fill_color;
            BorderColor = border_color;
            BorderWidth = border_width;
        }
        public void Render(Renderer renderer)
        {
            renderer.DrawRectangle(Position, Width, Height, DrawPen, FillBrush);
        }
        public void BorderOnly(Renderer renderer)
        {
            renderer.DrawRectangle(Position, Width, Height, DrawPen);
        }

        public static RectangleRenderElement CreateFromLine(Vector start, Vector end, int width)
        {
            Vector pos = Vector.Zero;
            int height = 0;
            if (start.Y < end.Y)
            {
                pos.Y = start.Y;
                pos.X = start.X - (width / 2);
                height = Vector.GetDistance(start - end);
                return new RectangleRenderElement(pos, width, height, Color.White, Color.Violet);
            }
            else if (start.Y > end.Y)
            {
                pos.Y = end.Y;
                pos.X = end.X - (width / 2);
                height = Vector.GetDistance(start - end);
                return new RectangleRenderElement(pos, width, height, Color.White, Color.Violet);
            }
            else if (start.X < end.X)
            {
                pos.X = start.X;
                pos.Y = start.Y - (width / 2);
                height = Vector.GetDistance(start - end);
                return new RectangleRenderElement(pos, height, width, Color.White, Color.Violet);
            }
            else
            {
                pos.X = end.X;
                pos.Y = end.Y - (width / 2);
                height = Vector.GetDistance(start - end);
                return new RectangleRenderElement(pos, height, width, Color.White, Color.Violet);
            }
        }
    }
}
