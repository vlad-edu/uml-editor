namespace UmlEditor.Geometry
{
    public static class Šafránková
    {
        public static bool Intersect(Line l1, Line l2)
        {
            var dir1 = Direction(l1.StartPoint, l1.EndPoint, l2.StartPoint);
            var dir2 = Direction(l1.StartPoint, l1.EndPoint, l2.EndPoint);
            var dir3 = Direction(l2.StartPoint, l2.EndPoint, l1.StartPoint);
            var dir4 = Direction(l2.StartPoint, l2.EndPoint, l1.EndPoint);

            if (dir1 != dir2 && dir3 != dir4) return true;
            return false;
        }

        private static LineDirection Direction(Vector a, Vector b, Vector c)
        {
            float value = (b.Y - a.Y) * (c.X - b.X) - (b.X - a.X) * (c.Y - b.Y);

            if (value == 0) return LineDirection.Collinear;
            else if (value < 0) return LineDirection.AntiClockwise;
            return LineDirection.Clockwise;
        }


        private enum LineDirection
        {
            Collinear,
            Clockwise,
            AntiClockwise
        }
    }
}
