using System;
using System.Windows.Forms;

namespace UmlEditor.Nodes.Interfaces
{
    public interface IKeyboardFocusableNode : IFocusableNode
    {
        EventHandler<KeyPressEventArgs> OnKeyPress { get; set; }
    }
}
