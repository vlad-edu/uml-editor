using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UmlEditor.Geometry;
using UmlEditor.Hitboxes;
using UmlEditor.Nodes;
using UmlEditor.Nodes.Interfaces;
using UmlEditor.Rendering;

namespace UmlEditor
{
    public class EditorControler
    {
        public List<Editor> Editors = new();
        public Editor CurrentEditor;
        private Renderer Renderer;
        private List<ButtonNode> Buttons = new();
        private PictureBox Plane;
        private int MaxProject = 5;
        // TODO: Make this work again
        public EditorControler(PictureBox tab, PictureBox renderPlane)
        {
            //Buttons.Add(new ButtonNode("btn", "+", Vector.Zero, Renderer.SingleTextWidth, Renderer.SingleTextHeight, () =>
            //{
            //    CreateNewProject();
            //}, RectangleRenderElementStyle.Default));
            Plane = renderPlane;
            tab.Height = Renderer.SingleTextHeight;
            tab.MouseClick += Tab_MouseClick;
            Renderer = new Renderer(tab);
            Renderer.Origin = Vector.Zero;
            Renderer.ClearColor = Color.White;
            CreateNewProject();
            CurrentEditor = Editors[0];
            CurrentEditor.isFocused = true;
            Plane.MouseClick += CurrentEditor.OnMouseClick;
            Plane.MouseMove += CurrentEditor.OnMouseMove;
            Plane.MouseDown += CurrentEditor.OnMouseDown;
            Plane.MouseUp += CurrentEditor.OnMouseUp;
            Plane.MouseWheel += CurrentEditor.OnMouseWheel;
            CurrentEditor.Render();
            Render();
        }

        private void Tab_MouseClick(object sender, MouseEventArgs e)
        {
            ButtonNode node = Buttons.FirstOrDefault(x => CheckIfClicked(e.Location, x));
            if(node != null)
            {
                node.OnMouseClick(this, e);
            }
            Render();
        }

        private void Render()
        {
            Renderer.Clear();
            CurrentEditor.Render();
            Buttons.ForEach(x => x.Render(Renderer));
            Renderer.Render();
        }
        private void ChangeEditor(int i)
        {
            if(CurrentEditor != Editors[i])
            {
                CurrentEditor.isFocused = false;
                Plane.MouseClick -= CurrentEditor.OnMouseClick;
                Plane.MouseMove -= CurrentEditor.OnMouseMove;
                Plane.MouseDown -= CurrentEditor.OnMouseDown;
                Plane.MouseUp -= CurrentEditor.OnMouseUp;
                Plane.MouseWheel -= CurrentEditor.OnMouseWheel;
                CurrentEditor = Editors[i];
                Plane.MouseClick += CurrentEditor.OnMouseClick;
                Plane.MouseMove += CurrentEditor.OnMouseMove;
                Plane.MouseDown += CurrentEditor.OnMouseDown;
                Plane.MouseUp += CurrentEditor.OnMouseUp;
                Plane.MouseWheel += CurrentEditor.OnMouseWheel;
                CurrentEditor.isFocused = true;
                CurrentEditor.Render();
            }
        }

        private bool CheckIfClicked(Vector position, INode node)
        {
            foreach (IHitbox hitbox in node.TriggerAreas)
            {
                if (hitbox.HasTriggered(position))
                    return true;
            }
            return false;
        }

        private void CreateNewProject()
        {
            if(Editors.Count < MaxProject)
            {
                int newEditor = Editors.Count;
                Editor newEdit = new(Plane, "New Project" + (Editors.Count + 1).ToString());
                newEdit.isFocused = false;
                Editors.Add(newEdit);
                //float final_pos = Buttons.Last().Position.X + Buttons.Last().Width;
                //Buttons.Add(new ButtonNode("btn", "Project" + Editors.Count, new Vector(final_pos, 0), Renderer.GetTextWidth(8), Renderer.SingleTextHeight, () =>
                //{
                //   ChangeEditor(newEditor);
                //}, RectangleRenderElementStyle.Default));
            }
        }

        public void OnKeyPress(object sender, KeyPressEventArgs e) => CurrentEditor.OnKeyPress(sender, e);
        public void OnResize(object sender, EventArgs e) => CurrentEditor.OnFormResize(sender, e);
    }
}
