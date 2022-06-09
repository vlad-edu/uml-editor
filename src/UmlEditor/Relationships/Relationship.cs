using System;
using System.Collections.Generic;
using UmlEditor.EventArguments;
using UmlEditor.Geometry;
using UmlEditor.Hitboxes;
using UmlEditor.Nodes;
using UmlEditor.Nodes.Interfaces;
using UmlEditor.NodeStructure;
using UmlEditor.Rendering;
using UmlEditor.Rendering.ElementStyles;

namespace UmlEditor.Relationships
{
    public class Relationship : BasicNode
    {
        public RelationshipSegment Origin { get; set; }
        public RelationshipSegment Target { get; set; }
        public string Name { get; set; }

        public new List<IHitbox> TriggerAreas { get; set; }
        public EventHandler<ResizeEventArgs> OnResize { get; set; }
        public EventHandler OnFocused { get; set; }
        public EventHandler OnUnfocused { get; set; }
        public BasicContainerNode OptionsPrefab { get; set; }
        public BasicContainerNode OptionsMenu { get; set; }
        public bool isFocused { get; set; }

        private ClassDiagramNode OriginNode;
        private ClassDiagramNode TargetNode;

        //Sketchy but needed
        public Relationship(ClassDiagramNode origin, ClassDiagramNode target) : base(new BasicNodeStructure(Vector.Zero, 0,0), RectangleRenderElementStyle.Default)
        {
            OriginNode = origin;
            TargetNode = target;
            List<Vector> vectors = GetLineVectors();
            Origin = new RelationshipSegment(vectors[0], vectors[3], vectors[2]);
            Target = new RelationshipSegment(vectors[1], vectors[4], vectors[2], true);
            OriginNode.OnPositionChanged += OnPositionChanged;
            TargetNode.OnPositionChanged += OnPositionChanged;
            OnOptionsHide += HideOptions;
            OnOptionsShow += ShowOptions;
            GeneratePrefab();
            StealHitboxes();
        }
        public override void Render(Renderer renderer)
        {
            Target.Render(renderer);
            Origin.Render(renderer);
            OptionsMenu?.Render(renderer);
        }

        private void StealHitboxes()
        {
            TriggerAreas = new List<IHitbox>();
            TriggerAreas.AddRange(Origin.TriggerAreas);
            TriggerAreas.AddRange(Target.TriggerAreas);
        }
       
        private void OnPositionChanged(object sender, PositionEventArgs e)
        {
            List<Vector> vectors = GetLineVectors();
            Origin.Position = vectors[0];
            Origin.Joint = vectors[3];
            Origin.Midpoint = vectors[2];

            Target.Position = vectors[1];
            Target.Joint = vectors[4];      
            Target.Midpoint = vectors[2];
            StealHitboxes();
        }

        private List<Vector> GetLineVectors()
        {
            List<Vector> Values = new();
            Line CntrLine = new(TargetNode.GetCenter(), OriginNode.GetCenter());

            if (Šafránková.Intersect(CntrLine, OriginNode.GetRightSide()))
                Values.Add(OriginNode.GetRightAnchor());
            else if (Šafránková.Intersect(CntrLine, OriginNode.GetTopSide()))
                Values.Add(OriginNode.GetTopAnchor());
            else if (Šafránková.Intersect(CntrLine, OriginNode.GetLeftSide()))
                Values.Add(OriginNode.GetLeftAnchor());
            else
                Values.Add(OriginNode.GetBotAnchor());

            if (Šafránková.Intersect(CntrLine, TargetNode.GetRightSide()))
                Values.Add(TargetNode.GetRightAnchor());
            else if (Šafránková.Intersect(CntrLine, TargetNode.GetTopSide()))
                Values.Add(TargetNode.GetTopAnchor());
            else if (Šafránková.Intersect(CntrLine, TargetNode.GetLeftSide()))
                Values.Add(TargetNode.GetLeftAnchor());
            else
                Values.Add(TargetNode.GetBotAnchor());

            Values.Add((Values[0] + Values[1]) / 2);
            if (Values[0] == OriginNode.GetLeftAnchor() || Values[0] == OriginNode.GetRightAnchor())
                Values.Add(new Vector(Values[2].X, Values[0].Y));
            else
                Values.Add(new Vector(Values[0].X, Values[2].Y));

            if (Values[1] == TargetNode.GetLeftAnchor() || Values[1] == TargetNode.GetRightAnchor())
                Values.Add(new Vector(Values[2].X, Values[1].Y));
            else
                Values.Add(new Vector(Values[1].X, Values[2].Y));

            return Values;
        }

        public List<INode> GetChildren()
        {
            List<INode> Children = new();
            if (OptionsMenu != null)
                Children.Add(OptionsMenu);
            return Children;
        }

        public IFocusableNode FocusedNode { get; set; }

        public void GeneratePrefab()
        {
            float total_Width = Renderer.GetTextWidth(16);
            OptionsPrefab = new BasicContainerNode(new BasicNodeStructure(Vector.Zero, total_Width, Renderer.SingleTextHeight * 1), RectangleRenderElementStyle.Default);
            OptionsPrefab.AddNode(new ButtonNode(new ButtonStructure(Vector.Zero, "Proof of Concept", total_Width, Renderer.SingleTextHeight, () =>
                {
                    OnOptionsHide?.Invoke(this, EventArgs.Empty);
                }),
                RectangleRenderElementStyle.Default,
                TextRenderElementStyle.Default));
        }

        public void ShowOptions(object sender, EventArgs e)
        {
            if (OptionsMenu == null)
            {
                OptionsMenu = OptionsPrefab;
                TriggerAreas.Add(OptionsMenu.TriggerAreas[0]);
                OnFocused?.Invoke(this, new NodeEventArgs(OptionsMenu));
                FocusedNode?.OnUnfocused?.Invoke(this, new NodeEventArgs(FocusedNode));
                TriggerAreas.Add(OptionsMenu.TriggerAreas[0]);
            }
            else
                OnOptionsHide?.Invoke(this, e);
        }
        public void HideOptions(object sender, EventArgs e)
        {
            TriggerAreas.Remove(OptionsMenu.TriggerAreas[0]);
            OptionsMenu = null;
        }

        public EventHandler OnOptionsShow { get; set; }
        public EventHandler OnOptionsHide { get; set; }

        public EventHandler OnMouseClick { get; set; }
        public Vector Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public EventHandler OnChange { get; set; }
        public EventHandler<NodeEventArgs> OnRemoval { get; set; }
    }
}
