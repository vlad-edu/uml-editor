using System;

namespace UmlEditor.EventArguments
{
    public class TextEventArgs : EventArgs
    {
        public string Text { get; }

        public TextEventArgs(string text)
        {
            Text = text;
        }
    }
}
