using System;
using UmlEditor.EventArguments;

namespace UmlEditor.Nodes.Interfaces
{
    public interface ITextNode : INode
    {
        string Text { get; set; }
        EventHandler<TextEventArgs> OnTextChange { get; set; }
    }
}
