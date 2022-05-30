using UmlEditor.Enums;
using UmlEditor.Geometry;

namespace UmlEditor.ProjectStructure
{
    public class MethodStructure : BasicCodeStructure
    {
        public string Arguments { get; set; }
        public MethodStructure(Vector position, string name, string type, string arguments, AccessModifiers accessModifier, Modifiers modifier) : base(position, name, type, accessModifier, modifier)
        {
            Arguments = arguments;
        }
    }
}
