using System;
using UmlEditor.Nodes.Interfaces;

namespace UmlEditor.EventArguments
{
    public class NodeEventArgs : EventArgs
    {
        public INode Node { get; }

        public NodeEventArgs(INode node)
        {
            Node = node;
        }
    }
}
