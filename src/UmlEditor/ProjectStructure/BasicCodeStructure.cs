using UmlEditor.Enums;
using UmlEditor.Geometry;

namespace UmlEditor.ProjectStructure
{
    public class BasicCodeStructure : BasicStructure
    {
        public AccessModifiers AccessModifier { get; set; }
        public Modifiers Modifier { get; set; }
        public string Type { get; set; }
        public BasicCodeStructure(Vector position, string name, string type, AccessModifiers accessModifier, Modifiers modifier) : base(position, name)
        {
            AccessModifier = accessModifier;
            Modifier = modifier;
            Type = type;
        }
    }
}
