using System;

namespace UmlEditor.Nodes.Interfaces
{
    public interface IOptionsNode : IMouseFocusableNode
    {
        BasicContainerNode OptionsPrefab { get; set; }
        BasicContainerNode OptionsMenu { get; set; }
        EventHandler OnOptionsShow { get; set; }
        EventHandler OnOptionsHide { get; set; }
    }
}
