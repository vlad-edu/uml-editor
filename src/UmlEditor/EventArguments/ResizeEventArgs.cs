using System;

namespace UmlEditor.EventArguments
{
    public class ResizeEventArgs : EventArgs
    {
        public float Width { get; }
        public float Height { get; }
        public ResizeEventArgs(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
}
