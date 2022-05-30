using UmlEditor.Geometry;

namespace UmlEditor.ProjectStructure
{
    public abstract class BasicStructure
    {
        protected BasicStructure(Vector position, string name)
        {
            Position = position;
            Name = name;
        }

        public Vector Position { get; set; }
        public string Name { get; set; }
    }
}
