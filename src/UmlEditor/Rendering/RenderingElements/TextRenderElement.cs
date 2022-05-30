using System.Drawing;
using UmlEditor.Geometry;

namespace UmlEditor.Rendering.RenderingElements
{
    public class TextRenderElement : IRenderElement
    {
        public Vector Position { get; set; } 
        public string Text { get; set; }
        public Font Font { get; private set; }
        private SolidBrush Brush { get; set; }
        private int fontSize;
        public int FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                fontSize = value;
                Font = new Font(Font.FontFamily, fontSize, FontStyle);
            }
        }
        private FontStyle fontStyle;
        public FontStyle FontStyle
        {
            get
            {
                return fontStyle;
            }
            set
            {
                fontStyle = value;
                Font = new Font(Font, fontStyle);
            }
        }
        public string FontName { get; private set; }
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
                Brush.Color = color;
            }
        }

        public TextRenderElement(Vector position, string text, Color text_color, int size = 14, FontStyle style = FontStyle.Regular)
        {
            FontName = "Consolas";
            Font = new Font(FontName ,size, style);
            Brush = new SolidBrush(text_color);
            Position = position;
            Text = text;
            Color = text_color;
            FontSize = size;
            FontStyle = style;
        }
        public void Render(Renderer renderer)
        {
            renderer.DrawText(Position, Text, Font, Brush);
        }

    }
}
