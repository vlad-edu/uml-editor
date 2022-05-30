using System;
using System.Collections.Generic;
using UmlEditor.EventArguments;
using UmlEditor.Geometry;
using UmlEditor.Hitboxes;

namespace UmlEditor.Nodes.Interfaces
{
    public interface INode
    {
        Vector Position { get; set; }
        float Width { get; set; }
        float Height { get; set; }
        List<IHitbox> TriggerAreas { get; set; }
        EventHandler<PositionEventArgs> OnPositionChanged { get; set; }
        EventHandler<ResizeEventArgs> OnResize { get; set; }
        EventHandler OnChange { get; set; }
        EventHandler<NodeEventArgs> OnRemoval { get; set; }
    }
}
