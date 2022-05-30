using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UmlEditor.CodeGeneration;
using UmlEditor.Enums;
using UmlEditor.EventArguments;
using UmlEditor.Geometry;
using UmlEditor.Hitboxes;
using UmlEditor.Nodes;
using UmlEditor.Nodes.Interfaces;
using UmlEditor.NodeStructure;
using UmlEditor.ProjectStructure;
using UmlEditor.Relationships;
using UmlEditor.Rendering;
using UmlEditor.Rendering.ElementStyles;

namespace UmlEditor
{
    public class Editor
    {
        public Project Project;
        private Renderer Renderer;
        private List<INode> Children = new();
        private IFocusableNode FocusedNode;
        private BasicContainerNode OptionsPrefab;
        private BasicContainerNode OptionsMenu;
        public bool isFocused = false;
        private Relationship focusedRelationship;
        private RelationshipManager RelationshipManager = new();
        private ClassDiagramNode Dragged;
        private Relationship CurrentRelationship;
        private Vector DraggingVector;
        private Vector LastMousePos;
        public string Directory = "C:\\Testing";
        public Editor(PictureBox renderTarget, string ProjectName)
        {
            Project = new Project(ProjectName);
            Renderer = new Renderer(renderTarget);
            GeneratePrefab();
            OnOptionsHide += HideOptions;
            OnOptionsShow += ShowOptions;
        }
        public void Render()
        {
            if(isFocused)
            {
                Clear();
                Children.OfType<IRenderableNode>().ToList().ForEach(x => x.Render(Renderer));
                OptionsMenu?.Render(Renderer);
                RelationshipManager.Render(Renderer);
                Renderer.Render();
            }
        }

        public void Clear()
        {
            Renderer.Clear();
        }
        public void AddDiagram(ClassStructure structure)
        {
            ClassDiagramNode node = new(structure, new BasicNodeStructure(structure.Position, 0, Renderer.SingleTextHeight), RectangleRenderElementStyle.Classes);
            node.AddNode(new PropertyNode(new PropertyStructure(Vector.Zero, "Prop", "Type", AccessModifiers.Public, Modifiers.None), new BasicNodeStructure(Vector.Zero, 0, Renderer.SingleTextHeight), RectangleRenderElementStyle.Textbox));
            node.AddNode(new MethodNode(new MethodStructure(Vector.Zero, "Method", "Type", "Name : Type", AccessModifiers.Public, Modifiers.None), new BasicNodeStructure(Vector.Zero, 0, Renderer.SingleTextHeight), RectangleRenderElementStyle.Textbox));
            if (!Project.Classes.Contains(structure))
                Project.AddClass(structure);
            Children.Add(node);
            node.OnRemoval += (sender, args) => RemoveDiagram((ClassDiagramNode) args.Node);
            node.OnFocused += OnNodeFocus;
            node.OnUnfocused += OnNodeUnfocus;
            node.Position = new Vector(node.Position.X - (node.Width / 2), node.Position.Y - (node.Height / 2));
        }

        public void RemoveDiagram(ClassDiagramNode diagram)
        {
            Children.Remove(diagram);
        }

        public void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if(FocusedNode != null)
            {
                if (e.KeyChar != (char) 13)
                {
                    IFocusableNode node = GetLastFocusInHierarchy((ClassDiagramNode) FocusedNode);
                    if (node is IKeyboardFocusableNode kn)
                        kn.OnKeyPress(this, e);

                }
                else
                    FocusedNode?.OnUnfocused?.Invoke(this, new NodeEventArgs(FocusedNode));
            }
            Render();
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            Vector mouse_position = (Vector)e.Location / Renderer.Scale - Renderer.Origin;
            LastMousePos = mouse_position;
            if (e.Button == MouseButtons.Left)
            {
                if (Dragged != null)
                    Dragged.Position = mouse_position + DraggingVector;
                else
                {
                    mouse_position = e.Location - Vector.Zero;
                    Renderer.Origin = mouse_position + DraggingVector;
                }
            }
            Render();
        }
        // TODO: Drag only if nothing is clicked on
        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            Vector mouse_position = (Vector)e.Location / Renderer.Scale - Renderer.Origin;
            LastMousePos = mouse_position;
            if (e.Button == MouseButtons.Left)
            {
                Dragged = ((ClassDiagramNode)Children.FirstOrDefault(x => CheckIfClicked(mouse_position, x)));
                if (Dragged != null)
                    DraggingVector = Dragged.Position - mouse_position;
                else
                {
                    mouse_position = e.Location - Vector.Zero;
                    DraggingVector = Renderer.Origin - mouse_position;
                }
            }
        }
        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            if(Dragged != null)
            {
                Dragged = null;
                DraggingVector = Vector.Zero;
            }
        }
        public void OnMouseClick(object sender, MouseEventArgs e)
        {
            Vector mouse_position = (Vector)e.Location / Renderer.Scale - Renderer.Origin;
            LastMousePos = mouse_position;
            if (e.Button == MouseButtons.Left)
            {
                HandleLeftClick(mouse_position, e);
            }
            else if(e.Button == MouseButtons.Right)
            {
                HandleRightClick(mouse_position, e);
            }

            Render();
        }

        private void HandleLeftClick(Vector mouse_position, MouseEventArgs e)
        {
            INode temp = null;
            if (OptionsMenu != null && CheckIfClicked(mouse_position, OptionsMenu))
                temp = OptionsMenu;
            else if (RelationshipManager.Relationships.Count > 0 && RelationshipManager.Relationships.FirstOrDefault(x => CheckIfClicked(mouse_position, x)) != null)
                temp = RelationshipManager.Relationships.FirstOrDefault(x => CheckIfClicked(mouse_position, x));
            else
                temp = Children.FirstOrDefault(x => CheckIfClicked(mouse_position, x));
            if (RelationshipManager.IsCreating && temp != null && temp is ClassDiagramNode cn)
            {
                RelationshipManager.SelectedNode = cn;
            }
            else
            {
                IFocusableNode node = SearchForClicked(temp, mouse_position);
                if (node != null)
                {
                    if (node is Relationship r)
                    {

                    }
                    else
                    {
                        node.OnFocused?.Invoke(this, new NodeEventArgs(node));
                        if (node is IMouseFocusableNode mn)
                            mn.OnMouseClick?.Invoke(this, e);
                    }
                }
            }
        }
        private void HandleRightClick(Vector mouse_position, MouseEventArgs e)
        {
            INode temp = Children.FirstOrDefault(x => CheckIfClicked(mouse_position, x));
            if(temp == null)
            {
                temp = RelationshipManager.Relationships.FirstOrDefault(x => CheckIfClicked(mouse_position, x));
            }
            if(temp != null)
            {
                if(temp is ClassDiagramNode cn)
                {
                    IOptionsNode op = SearchForOptionsNode(temp, mouse_position);
                    op.OptionsPrefab.Position = mouse_position;
                    op.OnOptionsShow?.Invoke(this, EventArgs.Empty);
                }
                else if (temp is Relationship rs)
                {
                    OnOptionsHide?.Invoke(this, EventArgs.Empty);
                    rs.OptionsPrefab.Position = mouse_position;
                    rs.OnOptionsShow?.Invoke(this, EventArgs.Empty);
                    focusedRelationship = rs;
                }
            }
            else
            {
                OptionsPrefab.Position = mouse_position;
                OnOptionsShow?.Invoke(this, EventArgs.Empty);
            }
        }

        private IFocusableNode GetLastFocusInHierarchy(INode node)
        {
            if (node is ClassDiagramNode cn)
            {
                if (cn.FocusedNode != null && cn.FocusedNode is TextBoxNode tb)
                    return tb;
                if (cn.FocusedNode != null && cn.FocusedNode is PropertyNode pn)
                {
                    if (pn.FocusedNode != null)
                        return pn.FocusedNode;
                    return pn;
                }
            }
            return null;
        }

        private IFocusableNode SearchForClicked(INode parent_node, Vector mouse_position)
        {
            bool found = false;
            while (!found)
            {
                if (parent_node is IContainerNode cn && parent_node is IMouseFocusableNode mn)
                {
                    INode n = cn.Children.FirstOrDefault(x => CheckIfClicked(mouse_position, x));
                    if (n == null)
                        return mn;
                    else
                        parent_node = n;
                }
                else if (parent_node is IContainerNode c)
                {
                    INode n = c.Children.FirstOrDefault(x => CheckIfClicked(mouse_position, x));
                    if (n == null)
                        return null;
                    parent_node = n;
                }
                else if (parent_node is IFocusableNode m)
                    return m;
                else if(parent_node is Relationship r && r.OptionsMenu != null)
                    parent_node = r.OptionsMenu;
                else
                    return null;
            }
            return null;
        }
        private IOptionsNode SearchForOptionsNode(INode parent_node, Vector mouse_position)
        {
            while(true)
            {
                if (parent_node is IContainerNode cn && parent_node is IOptionsNode op)
                {
                    INode n = cn.Children.FirstOrDefault(x => CheckIfClicked(mouse_position, x));
                    if (n == null || !(n is IOptionsNode))
                        return op;
                    else
                        parent_node = n;
                }
                else if (parent_node is IOptionsNode o)
                    return o;
                else
                    return null;
            }
        }

        private bool CheckIfClicked(Vector position, INode node)
        {
            if (node is Relationship r)
            {
                foreach (IHitbox hitbox in r.TriggerAreas)
                {
                    if (hitbox.HasTriggered(position))
                        return true;
                }
            }
            else
            {
                foreach (IHitbox hitbox in node.TriggerAreas)
                {
                    if (hitbox.HasTriggered(position))
                        return true;
                }
            }
            return false;
        }
        public EventHandler OnOptionsShow { get; set; }
        public EventHandler OnOptionsHide { get; set; }
        public void ShowOptions(object sender, EventArgs e)
        {
            if (OptionsMenu == null)
            {
                OptionsMenu = OptionsPrefab;
                FocusedNode?.OnUnfocused?.Invoke(this, new NodeEventArgs(FocusedNode));
                focusedRelationship?.OnOptionsHide?.Invoke(this, EventArgs.Empty);
            }
            else
                OnOptionsHide?.Invoke(this, e);
        }
        public void HideOptions(object sender, EventArgs e)
        {
            OptionsMenu = null;
        }
        private void GeneratePrefab()
        {
            float total_Width = Renderer.GetTextWidth(21);
            OptionsPrefab = new BasicContainerNode(new BasicNodeStructure(Vector.Zero, total_Width, Renderer.SingleTextHeight * 3), RectangleRenderElementStyle.Default);
            OptionsPrefab.AddNode(new ButtonNode(new ButtonStructure(Vector.Zero, "Add a Diagram", total_Width, Renderer.SingleTextHeight, () =>
                {
                    AddDiagram(new ClassStructure(LastMousePos, "NewClass", "class", AccessModifiers.Public, Modifiers.None));
                    OptionsMenu = null;
                }),
                RectangleRenderElementStyle.Default,
                TextRenderElementStyle.Default));
            OptionsPrefab.AddNode(new ButtonNode(new ButtonStructure(Vector.Zero, "Relationship", total_Width, Renderer.SingleTextHeight, () =>
                {
                    RelationshipManager.IsCreating = true;
                    OptionsMenu = null;
                }),
                RectangleRenderElementStyle.Default,
                TextRenderElementStyle.Default));
            OptionsPrefab.AddNode(new ButtonNode(new ButtonStructure(Vector.Zero, "Generate Code", total_Width, Renderer.SingleTextHeight, () =>
                {
                    CodeGenerator generator = new(Project, Directory);
                    generator.Generate();
                    OptionsMenu = null;
                }),
                RectangleRenderElementStyle.Default,
                TextRenderElementStyle.Default));
        }

        public void OnMouseWheel(object sender, MouseEventArgs e)
        {
            float PreviousScale = Renderer.Scale;
            if(e.Delta > 0)
                Renderer.Scale *= 1.1f;
            else
                Renderer.Scale *= 0.9f;

            Vector PreScale = (Vector)e.Location / PreviousScale - Renderer.Origin;
            Vector AfterScale = (Vector)e.Location / Renderer.Scale - Renderer.Origin;

            Vector Subtracted = PreScale - AfterScale;

            Renderer.Origin = Renderer.Origin - Subtracted;

            Render();
        }
        public void OnNodeFocus(object sender, NodeEventArgs e)
        {
            if (FocusedNode != e.Node)
            {
                //OnFocused?.Invoke(this, new NodeEventArgs(this));
                //OnOptionsHide?.Invoke(this, EventArgs.Empty);
                FocusedNode?.OnUnfocused?.Invoke(this, new NodeEventArgs(FocusedNode));
                focusedRelationship?.OnOptionsHide?.Invoke(this, EventArgs.Empty);
                OnOptionsHide?.Invoke(this, EventArgs.Empty);
                FocusedNode = (IFocusableNode)e.Node;
            }
        }
        public void OnNodeUnfocus(object sender, NodeEventArgs e)
        {
            if (FocusedNode == e.Node)
                FocusedNode = null;
        }
        public void OnFormResize(object sender, EventArgs e)
        {
            Renderer.Resize();
            Render();
        }
    }
}
