using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UmlEditor.Enums;
using UmlEditor.EventArguments;
using UmlEditor.Geometry;
using UmlEditor.Hitboxes;
using UmlEditor.Nodes.Interfaces;
using UmlEditor.NodeStructure;
using UmlEditor.ProjectStructure;
using UmlEditor.Rendering;
using UmlEditor.Rendering.ElementStyles;
using UmlEditor.Rendering.RenderingElements;

namespace UmlEditor.Nodes
{
    public class ClassDiagramNode : BasicContainerNode, IOptionsNode, IFocusableNode
    {
        public ClassStructure CodeStructure { get; set; }
        public List<PropertyNode> Properties = new();
        public List<MethodNode> Methods = new();
        public BasicContainerNode OptionsPrefab { get; set; }
        public BasicContainerNode OptionsMenu { get; set; }
        private TextBoxNode NameTextBox;
        private LineRenderElement NameLine;
        private LineRenderElement SeparatorLine;

        public ClassDiagramNode(ClassStructure codestructure, BasicNodeStructure structure, RectangleRenderElementStyle border_style) : base(structure, border_style)
        {
            CodeStructure = codestructure;
            NameTextBox = new TextBoxNode(new BasicTextNodeStructure(Position, Renderer.GetTextWidth(Name.Length), Renderer.SingleTextHeight, Name), TextRenderElementStyle.Default, RectangleRenderElementStyle.Textbox);
            NameLine = new LineRenderElement(new Vector(Position.X, Position.Y + Renderer.SingleTextHeight), new Vector(Position.X + Width, Position.Y + Renderer.SingleTextHeight), 1, Color.Black);
            SeparatorLine = new LineRenderElement(new Vector(Position.X, Position.Y + Renderer.SingleTextHeight), new Vector(Position.X + Width, Position.Y + Renderer.SingleTextHeight), 1, Color.Black);
            Children.Add(NameTextBox);
            GeneratePrefab();
            RepositionChildren();
            SetEvents();
        }
        public void UpdateStructure(object sender, EventArgs e)
        {
            CodeStructure.Name = NameTextBox.Text;
        }
        public void SetEvents()
        {
            OnOptionsShow += ShowOptions;
            OnOptionsHide += HideOptions;
            OnUnfocused += OnUnFocus;
            OnChange += HideOptions;
            OnChange += UpdateStructure;
            //OnChange += (sender, args) => OnUnFocus(this, new NodeEventArgs(this)); 
            Children.ForEach(x => x.OnResize += OnChildResize);
            Children.OfType<IFocusableNode>().ToList().ForEach(x =>
            {
                x.OnFocused += OnNodeFocus;
                x.OnFocused += HideOptions;
                x.OnUnfocused += OnNodeUnfocus;
                x.OnRemoval += (sender, args) => RemoveNode(args.Node);
            });
        }
        public virtual string Name
        {
            get => CodeStructure.Name;
            set
            {
                CodeStructure.Name = value;
                NameTextBox.Text = value;
                OnCodeStructureChange?.Invoke(this, new CodeStructureEventArgs(CodeStructure));
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public override void AddNode(INode node)
        {
            if (node is MethodNode mn)
            {
                Methods.Add(mn);
                mn.OnHitboxCreation += AddHitbox;
                mn.OnHitboxDeletion += RemoveHitbox;
                mn.OnUnfocused += OnNodeUnfocus;
                mn.OnFocused += OnNodeFocus;
                mn.OnRemoval += (sender, args) => RemoveNode(args.Node);
                CodeStructure.Methods.Add(mn.CodeStructure);
                Height += Renderer.SingleTextHeight;
                mn.RepositionChildren();
                mn.SetEvents();
                base.AddNode(node);
            }
            else if (node is PropertyNode pn)
            {
                Properties.Add(pn);
                pn.OnHitboxCreation += AddHitbox;
                pn.OnHitboxDeletion += RemoveHitbox;
                pn.OnUnfocused += OnNodeUnfocus;
                pn.OnFocused += OnNodeFocus;
                pn.OnRemoval += (sender, args) => RemoveNode(args.Node);
                CodeStructure.Properties.Add(pn.CodeStructure);
                Height += Renderer.SingleTextHeight;
                pn.RepositionChildren();
                pn.SetEvents();
                base.AddNode(node);
            }
        }

        public override void RemoveNode(INode node)
        {
            if (node is MethodNode mn)
            {
                Methods.Remove(mn);
                CodeStructure.Methods.Remove(mn.CodeStructure);
            }
            else if (node is PropertyNode pn)
            {
                Properties.Remove(pn);
                CodeStructure.Properties.Remove(pn.CodeStructure);
            }
            Height -= Renderer.SingleTextHeight;
            base.RemoveNode(node);
        }

        public virtual AccessModifiers AccessModifier
        {
            get => CodeStructure.AccessModifier;
            set
            {
                CodeStructure.AccessModifier = value;
                OnCodeStructureChange?.Invoke(this, new CodeStructureEventArgs(CodeStructure));
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public virtual Modifiers Modifier
        {
            get => CodeStructure.Modifier;
            set
            {
                CodeStructure.Modifier = value;
                OnCodeStructureChange?.Invoke(this, new CodeStructureEventArgs(CodeStructure));
                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public float GetWidest()
        {
            INode temp_node = Children.OrderByDescending(x => x.Width).FirstOrDefault();
            return temp_node.Width;
        }
        public override void RepositionChildren()
        {
            Width = GetWidest();
            NameTextBox.Position = new Vector((Position.X + Width / 2) - (NameTextBox.Width / 2), Position.Y);
            for (int i = 0; i < Properties.Count; i++)
            {
                Vector newpos = Position + new Vector(0, (i + 1) * Renderer.SingleTextHeight);
                if(Properties[i].Position != newpos)
                    Properties[i].Position = newpos;
            }
            for (int i = 0; i < Methods.Count; i++)
            {
                Vector newpos = Position + new Vector(0, (i + Properties.Count + 1) * Renderer.SingleTextHeight);
                if (Methods[i].Position != newpos)
                    Methods[i].Position = newpos;
            }
            NameLine.StartPoint = new Vector(Position.X, Position.Y + Renderer.SingleTextHeight);
            NameLine.EndPoint = new Vector(Position.X + Width, Position.Y + Renderer.SingleTextHeight);
            if (Properties.Count > 0)
            {
                PropertyNode prop = Properties.Last();
                SeparatorLine.StartPoint = prop.Position + new Vector(0, Renderer.SingleTextHeight);
                SeparatorLine.EndPoint = prop.Position + new Vector(Width, Renderer.SingleTextHeight);
            }
            else
            {
                SeparatorLine = new LineRenderElement(new Vector(Position.X, Position.Y + Renderer.SingleTextHeight), new Vector(Position.X + Width, Position.Y + Renderer.SingleTextHeight), 1, Color.Black);
            }
        }
        public override void OnNodeFocus(object sender, NodeEventArgs e)
        {
            if (FocusedNode != e.Node)
            {
                OnFocused?.Invoke(this, new NodeEventArgs(this));
                FocusedNode?.OnUnfocused?.Invoke(this, new NodeEventArgs(FocusedNode));
                FocusedNode = (IFocusableNode)e.Node;
            }
        }
        public void OnUnFocus(object sender, NodeEventArgs e)
        {
            FocusedNode?.OnUnfocused?.Invoke(this, new NodeEventArgs(FocusedNode));
            OnOptionsHide?.Invoke(this, EventArgs.Empty);
        }
        public override void OnNodeUnfocus(object sender, NodeEventArgs e)
        {
            if (FocusedNode == e.Node)
                FocusedNode = null;
        }
        public void GeneratePrefab()
        {
            float total_Width = Renderer.GetTextWidth(13);
            OptionsPrefab = new BasicContainerNode(new BasicNodeStructure(Vector.Zero, total_Width, Renderer.SingleTextHeight * 3), RectangleRenderElementStyle.Default);
            OptionsPrefab.AddNode(new ButtonNode(new ButtonStructure(Vector.Zero, "Add Property", total_Width, Renderer.SingleTextHeight, () =>
                {
                    AddNode(new PropertyNode(new PropertyStructure(Vector.Zero, "Property", "String", AccessModifiers.Public, Modifiers.None), new BasicNodeStructure(Vector.Zero, 0, Renderer.SingleTextHeight),
                        RectangleRenderElementStyle.Textbox));
                    OnOptionsHide?.Invoke(this, EventArgs.Empty);
                }),
                RectangleRenderElementStyle.Default,
                TextRenderElementStyle.Default));
            OptionsPrefab.AddNode(new ButtonNode(new ButtonStructure(Vector.Zero, "AddMethod", total_Width, Renderer.SingleTextHeight, () =>
                {
                    AddNode(new MethodNode(new MethodStructure(Vector.Zero, "Method", "void", "", AccessModifiers.Public, Modifiers.None), new BasicNodeStructure(Vector.Zero, 0, Renderer.SingleTextHeight),
                        RectangleRenderElementStyle.Textbox));
                    OnOptionsHide?.Invoke(this, EventArgs.Empty);
                }),
                RectangleRenderElementStyle.Default,
                TextRenderElementStyle.Default));
            OptionsPrefab.AddNode(new ButtonNode(new ButtonStructure(Vector.Zero, "Remove", total_Width, Renderer.SingleTextHeight, () =>
            {
                OnRemoval?.Invoke(this, new NodeEventArgs(this));
                OnOptionsHide?.Invoke(this, EventArgs.Empty);
                }),
                RectangleRenderElementStyle.Default,
                TextRenderElementStyle.Default));
        }
        public EventHandler OnOptionsShow { get; set; }
        public EventHandler OnOptionsHide { get; set; }
        public EventHandler<NodeEventArgs> OnFocused { get; set; }
        public EventHandler<NodeEventArgs> OnUnfocused { get; set; }
        public EventHandler OnMouseClick { get; set; }
        public EventHandler<CodeStructureEventArgs> OnCodeStructureChange { get; set; }
        public EventHandler<HitboxEventArgs> OnHitboxCreation { get; set; }
        public EventHandler<HitboxEventArgs> OnHitboxDeletion { get; set; }
        public void AddHitbox(object sender, HitboxEventArgs e)
        {
            TriggerAreas.Add(e.Hitbox);
        }

        public void RemoveHitbox(object sender, HitboxEventArgs e)
        {
            TriggerAreas.Remove(e.Hitbox);
        }
        public void AddHitbox(IHitbox hitbox)
        {
            TriggerAreas.Add(hitbox);
            OnHitboxCreation?.Invoke(this, new HitboxEventArgs(hitbox));
        }

        public void RemoveHitbox(IHitbox hitbox)
        {
            TriggerAreas.Remove(hitbox);
            OnHitboxDeletion?.Invoke(this, new HitboxEventArgs(hitbox));
        }
        public void ShowOptions(object sender, EventArgs e)
        {
            if (OptionsMenu == null)
            {
                OptionsMenu = OptionsPrefab;
                PrependNode(OptionsMenu);
                OnFocused?.Invoke(this, new NodeEventArgs(this));
                FocusedNode?.OnUnfocused?.Invoke(this, new NodeEventArgs(FocusedNode));
                AddHitbox(OptionsMenu.TriggerAreas[0]);
            }
            else
                OnOptionsHide?.Invoke(this, e);
        }
        public void HideOptions(object sender, EventArgs e)
        {
            if (OptionsMenu != null)
            {
                RemoveHitbox(OptionsMenu.TriggerAreas[0]);
                Children.Remove(OptionsMenu);
            }
            OptionsMenu = null;
        }

        public override void Render(Renderer renderer)
        {
            base.Render(renderer);
            FocusedNode?.Render(renderer);
            SeparatorLine.Render(renderer);
            NameLine.Render(renderer);
            if (FocusedNode != null && FocusedNode is PropertyNode pn)
            {
                pn.OptionsMenu?.Render(renderer);
                pn.AccessModifierMenu?.Render(renderer);
            }
            BorderElement.BorderOnly(renderer);
            OptionsMenu?.Render(renderer);
        }

        public bool IsOnEdge(Vector v)
        {
            float left = Position.X;
            float right = Position.X + Width;
            float top = Position.Y;
            float bot = Position.Y + Height;
            return v.X == left || v.X == right || v.Y == top || v.Y == bot;
        }

        public List<Vector> GetSideCenters()
        {
            float left = Position.X;
            float right = Position.X + Width;
            float top = Position.Y;
            float bot = Position.Y + Height;
            List<Vector> centers = new();
            centers.Add(new Vector((left + right) / 2, top));
            centers.Add(new Vector(right, (top + bot) / 2));
            centers.Add(new Vector((left + right) / 2, bot));
            centers.Add(new Vector(left, (top + bot) / 2));
            return centers;
        }

        public Vector GetTopAnchor() => new((Position.X + (Width / 2)), Position.Y);
        public Vector GetBotAnchor() => new((Position.X + (Width / 2)), Position.Y + Height);
        public Vector GetLeftAnchor() => new(Position.X, Position.Y + (Height / 2));
        public Vector GetRightAnchor() => new(Position.X + Width, Position.Y + (Height / 2));
        public Vector GetCenter() => new(Position.X + (Width / 2), Position.Y + (Height / 2));
        public Vector GetTopLeftCorner() => new(Position.X, Position.Y);
        public Vector GetTopRightCorner() => new(Position.X + Width, Position.Y);
        public Vector GetBotLeftCorner() => new(Position.X, Position.Y + Height);
        public Vector GetBotRightCorner() => new(Position.X + Width, Position.Y + Height);
        public Line GetTopSide() => new(GetTopLeftCorner(), GetTopRightCorner());
        public Line GetBotSide() => new(GetBotLeftCorner(), GetBotRightCorner());
        public Line GetLeftSide() => new(GetTopLeftCorner(), GetBotLeftCorner());
        public Line GetRightSide() => new(GetTopRightCorner(), GetBotRightCorner());

    }
}
