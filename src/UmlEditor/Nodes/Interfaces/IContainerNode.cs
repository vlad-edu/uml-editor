using System;
using System.Collections.Generic;
using UmlEditor.EventArguments;

namespace UmlEditor.Nodes.Interfaces
{
    public interface IContainerNode : INode
    {
        List<INode> Children { get; set; }
        List<IFocusableNode> GetFocusableNodes();
        IFocusableNode FocusedNode { get; set; }
        EventHandler<NodeEventArgs> OnNodeAdd { get; set; }
        EventHandler<NodeEventArgs> OnNodeRemoval { get; set; }
    }
}
