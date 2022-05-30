using System;
using UmlEditor.Nodes.Interfaces;

namespace UmlEditor.EventArguments
{
    public class OptionsMenuEventArgs : EventArgs
    {
        public IOptionsNode Node { get; private set; }

        public OptionsMenuEventArgs(IOptionsNode optionsNode)
        {
            Node = optionsNode;
        }
    }
}
