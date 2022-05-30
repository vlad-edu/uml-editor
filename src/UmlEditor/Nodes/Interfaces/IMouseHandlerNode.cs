using System;

namespace UmlEditor.Nodes.Interfaces
{
    public interface IMouseFocusableNode : IFocusableNode
    {
        EventHandler OnMouseClick { get; set; }
    }
}
