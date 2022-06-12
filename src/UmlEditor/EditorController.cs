using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using UmlEditor.Geometry;
using UmlEditor.Hitboxes;
using UmlEditor.Nodes;
using UmlEditor.Nodes.Interfaces;
using UmlEditor.ProjectStructure;
using UmlEditor.Rendering;

namespace UmlEditor
{
    public class EditorController
    {
        public List<Editor> Editors = new();
        public Editor CurrentEditor;
        private Renderer Renderer;
        private List<ButtonNode> Buttons = new();
        private PictureBox Plane;
        private int MaxProject = 5;

        // TODO: Make this work again
        public EditorController(PictureBox tab, PictureBox renderPlane)
        {
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

        public EditorController(PictureBox tab, PictureBox renderPlane, string configData)
        {
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
            Deserialize(configData);

            //Renderer.Clear();
            //CurrentEditor.Render();
            //Renderer.Render();
        }

        private void Tab_MouseClick(object sender, MouseEventArgs e)
        {
            ButtonNode node = Buttons.FirstOrDefault(x => CheckIfClicked(e.Location, x));
            if (node != null)
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
            if (CurrentEditor != Editors[i])
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
            if (Editors.Count < MaxProject)
            {
                int newEditor = Editors.Count;
                Editor newEdit = new(Plane, "New Project" + (Editors.Count + 1).ToString());
                newEdit.isFocused = false;
                Editors.Add(newEdit);
            }
        }

        public void OnKeyPress(object sender, KeyPressEventArgs e) => CurrentEditor.OnKeyPress(sender, e);

        public void OnResize(object sender, EventArgs e) => CurrentEditor.OnFormResize(sender, e);

        public string Serialize()
        {
            var data = new SerializeData();
            data.Diagrams.AddRange(CurrentEditor.Children.Cast<ClassDiagramNode>().Select(c => c.CodeStructure));
            data.Relations.AddRange(
                CurrentEditor.RelationshipManager.Relationships
                    .Select(r => new SerializeData.RelationData
                    {
                        Source = r.OriginNode.CodeStructure.Id,
                        Target = r.TargetNode.CodeStructure.Id,
                    }));
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data)));
        }

        private void Deserialize(string value)
        {
            var data = JsonSerializer.Deserialize<SerializeData>(Encoding.UTF8.GetString(Convert.FromBase64String(value)));
            CreateNewProject();
            ChangeEditor(Editors.Count - 1);

            foreach (var item in data!.Diagrams)
            {
                CurrentEditor.AddDiagram(item);
            }

            foreach (var relation in data.Relations)
            {
                var source = CurrentEditor.Children
                    .Cast<ClassDiagramNode>()
                    .FirstOrDefault(c => c.CodeStructure.Id == relation.Source);

                var target = CurrentEditor.Children
                    .Cast<ClassDiagramNode>()
                    .FirstOrDefault(c => c.CodeStructure.Id == relation.Source);

                CurrentEditor.RelationshipManager.CreateRelationship(source, target);
            }
        }

        private class SerializeData
        {
            public List<ClassStructure> Diagrams { get; set; } = new();
            public List<RelationData> Relations { get; set; } = new();

            public class RelationData
            {
                public Guid Source { get; set; }
                public Guid Target { get; set; }
            }
        }
    }
}
