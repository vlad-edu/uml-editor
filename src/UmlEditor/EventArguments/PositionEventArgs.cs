using System;
using UmlEditor.Geometry;

namespace UmlEditor.EventArguments
{
    public class PositionEventArgs : EventArgs
    {
        public Vector Position;
        public PositionEventArgs(Vector position)
        {
            Position = position;
        }
    }
}
