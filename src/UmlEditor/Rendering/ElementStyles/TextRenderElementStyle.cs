using System.Drawing;

namespace UmlEditor.Rendering.ElementStyles
{
    public class TextRenderElementStyle
    {
        public int FontSize { get; set; }
        public FontStyle FontStyle { get; set; }
        public string FontName { get; set; }
        public Color Color { get; set; }

        public TextRenderElementStyle(Font font, Color color)
        {
            FontSize = (int)font.Size;
            FontStyle = font.Style;
            FontName = font.Name;
            Color = color;
        }

        public TextRenderElementStyle(int fontSize, FontStyle fontStyle, string fontName, Color color)
        {
            FontSize = fontSize;
            FontStyle = fontStyle;
            FontName = fontName;
            Color = color;
        }

        public static TextRenderElementStyle Default = new(12, FontStyle.Regular, "Consolas", Color.Black);
    }
}
