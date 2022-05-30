using UmlEditor.Geometry;

namespace UmlEditor.NodeStructure
{
    public class BasicNodeStructure
    {
        public Vector Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public BasicNodeStructure(Vector pos, float width, float height)
        {
            Position = pos;
            Width = width;
            Height = height;
        }
    }
}
