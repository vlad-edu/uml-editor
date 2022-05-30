using System;
using UmlEditor.Geometry;

namespace UmlEditor.NodeStructure
{
    public class ButtonStructure : BasicTextNodeStructure
    {
        public Action ButtonAction { get; set; }
        public string ButtonText { get; set; }
        public ButtonStructure(Vector pos, string text, float width, float height, Action action) : base(pos, width, height, text)
        {
            ButtonAction = action;
            ButtonText = text;
        }
    }
}
