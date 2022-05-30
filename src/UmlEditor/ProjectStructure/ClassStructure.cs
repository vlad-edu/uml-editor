using System.Collections.Generic;
using UmlEditor.Enums;
using UmlEditor.Geometry;

namespace UmlEditor.ProjectStructure
{
    public class ClassStructure : BasicCodeStructure
    {
        public List<MethodStructure> Methods { get; set; } = new List<MethodStructure>();
        public List<PropertyStructure> Properties { get; set; } = new List<PropertyStructure>();

        public void AddProperty(PropertyStructure prop) => Properties.Add(prop);
        public void AddMethod(MethodStructure method) => Methods.Add(method);

        public ClassStructure(Vector position, string name, string type, AccessModifiers accessModifier, Modifiers modifier) : base(position, name, type, accessModifier, modifier)
        {
        }
    }
}
