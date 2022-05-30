using System;
using UmlEditor.ProjectStructure;

namespace UmlEditor.EventArguments
{
    public class CodeStructureEventArgs : EventArgs
    {
        public BasicCodeStructure CodeStructure { get; }

        public CodeStructureEventArgs(BasicCodeStructure structure)
        {
            CodeStructure = structure;
        }
    }
}
