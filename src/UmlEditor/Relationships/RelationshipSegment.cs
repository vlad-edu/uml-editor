using System;
using System.Collections.Generic;
using System.Drawing;
using UmlEditor.EventArguments;
using UmlEditor.Geometry;
using UmlEditor.Hitboxes;
using UmlEditor.Rendering;
using UmlEditor.Rendering.RenderingElements;

namespace UmlEditor.Relationships
{
    public class RelationshipSegment //: IRenderableNode
    {
        public string Name { get; set; }
        public Vector Position
        {
            get => SegmentStart.StartPoint;
            set
            {
                SegmentStart.StartPoint = value;
                CreateHitboxes();
            }
        }
        public float Width { get; set; }
        public float Height { get; set; }
        public List<IHitbox> TriggerAreas { get; set; } = new List<IHitbox>();
        public EventHandler<ResizeEventArgs> OnResize { get; set; }
        private List<RectangleRenderElement> DebugRectangles = new();

        private LineRenderElement SegmentStart;
        private LineRenderElement SegmentEnd;
        public Vector Joint
        {
            get => SegmentStart.EndPoint;
            set
            {
                SegmentStart.EndPoint = value;
                SegmentEnd.StartPoint = value;
                CreateHitboxes();
            }
        }
        public Vector Midpoint
        {
            get => SegmentEnd.EndPoint;
            set
            {
                SegmentEnd.EndPoint = value;
                CreateHitboxes();
            }
        }

        public RelationshipSegment(Vector position, Vector joint, Vector anchor)
        {
            SegmentStart = new LineRenderElement(position, joint, 1, Color.White);
            SegmentEnd = new LineRenderElement(joint, anchor, 1, Color.White);
            CreateHitboxes();
        }
        public void Render(Renderer renderer)
        {
            SegmentStart.Render(renderer);
            SegmentEnd.Render(renderer);
            //DebugRectangles.ForEach(x => x.BorderOnly(renderer));
        }

        private void CreateHitboxes()
        {
            Vector position = SegmentStart.StartPoint;
            Vector joint = SegmentStart.EndPoint;
            Vector anchor = SegmentEnd.EndPoint;
            TriggerAreas = new List<IHitbox>();
            DebugRectangles = new List<RectangleRenderElement>();
            TriggerAreas.Add(RectangleHitbox.CreateFromLine(position, joint, 10));
            TriggerAreas.Add(RectangleHitbox.CreateFromLine(joint, anchor, 10));
            DebugRectangles.Add(RectangleRenderElement.CreateFromLine(joint, anchor, 10));
            DebugRectangles.Add(RectangleRenderElement.CreateFromLine(position, joint, 10));
        }
    }
}
