using System;
using System.Drawing;
using UmlEditor.EventArguments;
using UmlEditor.Geometry;
using UmlEditor.Nodes.Interfaces;
using UmlEditor.NodeStructure;
using UmlEditor.Rendering;
using UmlEditor.Rendering.ElementStyles;
using UmlEditor.Rendering.RenderingElements;

namespace UmlEditor.Nodes
{
    public class LabelNode : BasicNode, ITextNode
    {
        public new BasicTextNodeStructure Structure { get; set; }
        public TextRenderElementStyle TextStyle { get; }

        private TextRenderElement TextElement;

        public LabelNode(BasicTextNodeStructure structure, TextRenderElementStyle text_style,
            RectangleRenderElementStyle border_style) : base(structure, border_style)
        {
            Structure = structure;
            TextStyle = text_style;
            TextElement = new TextRenderElement(Position, Text, text_style.Color, text_style.FontSize, text_style.FontStyle);
        }
        public string Text
        {
            get => Structure.Text;
            set
            {
                Structure.Text = value;
                TextElement.Text = value;
                Width = Renderer.GetTextWidth(value.Length);
                OnTextChange?.Invoke(this, new TextEventArgs(Text));
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }


        public override Vector Position
        {
            get => base.Position;
            set
            {
                TextElement.Position = value;
                base.Position = value;
            }
        }

        public Color TextColor
        {
            get => TextStyle.Color;
            set
            {
                TextStyle.Color = value;
                TextElement.Color = value;
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public FontStyle Style
        {
            get => TextStyle.FontStyle;
            set
            {
                TextStyle.FontStyle = value;
                TextElement.FontStyle = value;
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public override void Render(Renderer renderer)
        {
            base.Render(renderer);
            TextElement.Render(renderer);
        }
        public EventHandler<TextEventArgs> OnTextChange { get; set; }
    }
}
