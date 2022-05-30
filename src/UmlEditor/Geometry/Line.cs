namespace UmlEditor.Geometry
{
    public struct Line
    {
        public Vector StartPoint { get; set; }
        public Vector EndPoint { get; set; }
        public Line(Vector start, Vector end)
        {
            StartPoint = start;
            EndPoint = end;
        }
    }
}
