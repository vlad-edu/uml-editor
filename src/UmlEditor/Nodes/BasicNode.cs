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
    public class BasicNode : IRenderableNode
    {
        public virtual BasicNodeStructure Structure { get; }
        public RectangleRenderElementStyle BorderStyle { get; }
        public List<IHitbox> TriggerAreas { get; set; } = new List<IHitbox>();

        public RectangleRenderElement BorderElement { get; set; }
        public EventHandler<ResizeEventArgs> OnResize { get; set; }
        public EventHandler OnChange { get; set; }
        public EventHandler<NodeEventArgs> OnRemoval { get; set; }
        public EventHandler<PositionEventArgs> OnPositionChanged { get; set; }
        public BasicNode(BasicNodeStructure structure, RectangleRenderElementStyle border_style)
        {
            Structure = structure;
            BorderStyle = border_style;
            BorderElement = new RectangleRenderElement(Position, Width, Height, border_style.FillColor, border_style.BorderColor, border_style.BorderWidth);
            TriggerAreas.Add(new RectangleHitbox(Position, Width, Height));
        }
        public virtual void Render(Renderer renderer)
        {
            BorderElement.Render(renderer);
        }
        public virtual Vector Position
        {
            get => Structure.Position;
            set
            {
                Structure.Position = value;
                BorderElement.Position = value;
                ((RectangleHitbox)TriggerAreas[0]).Position = value;
                OnPositionChanged?.Invoke(this, new PositionEventArgs(value));
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public virtual float Width
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
        public virtual float Height
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
        public virtual Color BorderColor
        {
            get => BorderStyle.BorderColor;
            set
            {
                BorderStyle.BorderColor = value;
                BorderElement.BorderColor = value;
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public virtual Color FillColor
        {
            get => BorderStyle.FillColor;
            set
            {
                BorderStyle.FillColor = value;
                BorderElement.FillColor = value;
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
