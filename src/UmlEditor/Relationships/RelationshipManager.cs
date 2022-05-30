using System.Collections.Generic;
using UmlEditor.Nodes;
using UmlEditor.Rendering;

namespace UmlEditor.Relationships
{
    public class RelationshipManager
    {
        public List<Relationship> Relationships = new();
        public bool IsCreating = false;

        private ClassDiagramNode node = null;
        public ClassDiagramNode SelectedNode
        {
            get => node;
            set
            {
                if (node == null)
                    node = value;
                else if(node != value)
                {
                    CreateRelationship(node, value);
                    IsCreating = false;
                    node = null;
                }
            }
        }
        public void CreateRelationship(ClassDiagramNode from, ClassDiagramNode to)
        {
            Relationships.Add(new Relationship(from, to));
        }
        public void Render(Renderer renderer)
        {
            Relationships.ForEach(x => x.Render(renderer));
        }


    }
}
