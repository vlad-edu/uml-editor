using System;
using System.Collections.Generic;
using System.Drawing;
using UmlEditor.EventArguments;
using UmlEditor.Geometry;
using UmlEditor.Hitboxes;
using UmlEditor.Nodes.Interfaces;
using UmlEditor.NodeStructure;
using UmlEditor.Rendering;
using UmlEditor.Rendering.ElementStyles;
using UmlEditor.Rendering.RenderingElements;

namespace UmlEditor.Nodes
{
    public class ButtonNode : IRenderableNode, IMouseFocusableNode, ITextNode
    {
        public ButtonStructure Structure { get; }
        public RectangleRenderElementStyle BorderStyle { get; }
        public TextRenderElementStyle TextStyle { get; }

        public ButtonNode(ButtonStructure structure, RectangleRenderElementStyle style, TextRenderElementStyle text_style)
        {
            Structure = structure;
            BorderStyle = style;
            TextStyle = text_style;
            BorderElement = new RectangleRenderElement(Position, Width, Height, style.FillColor, style.BorderColor, style.BorderWidth);
            TextElement = new TextRenderElement(Position, Text, TextColor, TextStyle.FontSize, TextStyle.FontStyle);
            TriggerAreas.Add(new RectangleHitbox(Position, Width, Height));
            ButtonAction = Structure.ButtonAction;
            OnMouseClick += (object sender, EventArgs e) =>
            {
                ButtonAction.Invoke();
                OnUnfocused?.Invoke(this, new NodeEventArgs(this));
            };
        }

        public List<IHitbox> TriggerAreas { get; set; } = new List<IHitbox>();
        public EventHandler<PositionEventArgs> OnPositionChanged { get; set; }

        public Vector Position
        {
            get => Structure.Position;
            set
            {
                Structure.Position = value;
                BorderElement.Position = value;
                TextElement.Position = value;
                ((RectangleHitbox)TriggerAreas[0]).Position = value;
                OnPositionChanged?.Invoke(this, new PositionEventArgs(value));
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public float Width
        {
            get => Structure.Width;
            set
            {
                Structure.Width = value;
                BorderElement.Width = value;
                ((RectangleHitbox)TriggerAreas[0]).Width = value;
                OnResize?.Invoke(this, new ResizeEventArgs(Width, Height));
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public float Height
        {
            get => Structure.Height;
            set
            {
                Structure.Height = value;
                BorderElement.Height = value;
                ((RectangleHitbox)TriggerAreas[0]).Height = value;
                OnResize?.Invoke(this, new ResizeEventArgs(Width, Height));
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public string Text
        {
            get => Structure.Text;
            set
            {
                Structure.Text = value;
                TextElement.Text = value; 
                OnTextChange?.Invoke(this, new TextEventArgs(value));
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public Color BorderColor
        {
            get => BorderStyle.BorderColor;
            set
            {
                BorderStyle.BorderColor = value;
                BorderElement.BorderColor = value;
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public Color FillColor
        {
            get => BorderStyle.FillColor;
            set
            {
                BorderStyle.FillColor = value;
                BorderElement.FillColor = value;
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public int BorderWidth
        {
            get => BorderStyle.BorderWidth;
            set
            {
                BorderStyle.BorderWidth = value;
                BorderElement.BorderWidth = value;
                OnChange?.Invoke(this, EventArgs.Empty);
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

        public EventHandler<TextEventArgs> OnTextChange { get; set; }

        public Action ButtonAction { get; private set; }

        public EventHandler<ResizeEventArgs> OnResize { get; set; }
        public EventHandler OnChange { get; set; }
        public EventHandler<NodeEventArgs> OnRemoval { get; set; }
        public EventHandler<NodeEventArgs> OnFocused { get; set; }
        public EventHandler<NodeEventArgs> OnUnfocused { get; set; }
        public EventHandler OnMouseClick { get; set; }

        private RectangleRenderElement BorderElement;
        private TextRenderElement TextElement;

        public void Render(Renderer renderer)
        {
            BorderElement.Render(renderer);
            TextElement.Render(renderer);
        }
    }
}
