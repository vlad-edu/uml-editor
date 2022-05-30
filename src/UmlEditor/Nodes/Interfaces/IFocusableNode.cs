using System;
using UmlEditor.EventArguments;

namespace UmlEditor.Nodes.Interfaces
{
    public interface IFocusableNode : IRenderableNode
    {
        EventHandler<NodeEventArgs> OnFocused { get; set; }
        EventHandler<NodeEventArgs> OnUnfocused { get; set; }
    }
}
