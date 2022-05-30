using System.Drawing;

namespace UmlEditor.Rendering.ElementStyles
{
    public class RectangleRenderElementStyle
    {
        public RectangleRenderElementStyle(Color borderColor, Color fillColor, int borderWidth)
        {
            BorderColor = borderColor;
            FillColor = fillColor;
            BorderWidth = borderWidth;
        }

        public Color BorderColor { get; set; }
        public Color FillColor { get; set; }
        public int BorderWidth { get; set; }

        public static RectangleRenderElementStyle Default = new(Color.Black, Color.Lavender, 1);
        public static RectangleRenderElementStyle Classes = new(Color.Black, Color.White, 1);
        public static RectangleRenderElementStyle Textbox = new(Color.White, Color.White, 1);
    }
}
