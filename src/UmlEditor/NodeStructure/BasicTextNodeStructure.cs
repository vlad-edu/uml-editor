using UmlEditor.Geometry;

namespace UmlEditor.NodeStructure
{
    public class BasicTextNodeStructure : BasicNodeStructure
    {
        public BasicTextNodeStructure(Vector pos, float width, float height, string text) : base(pos, width, height)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
