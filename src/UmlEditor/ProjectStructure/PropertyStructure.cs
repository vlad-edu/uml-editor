using UmlEditor.Enums;
using UmlEditor.Geometry;

namespace UmlEditor.ProjectStructure
{
    public class PropertyStructure : BasicCodeStructure
    {
        public PropertyStructure(Vector position, string name, string type, AccessModifiers accessModifier, Modifiers modifier) : base(position, name, type, accessModifier, modifier)
        {
        }
    }
}
