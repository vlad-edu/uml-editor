using UmlEditor.Geometry;

namespace UmlEditor.Hitboxes
{
    public class RectangleHitbox : IHitbox
    {
        public RectangleHitbox(Vector position, float width, float height)
        {
            Position = new Vector(position.X, position.Y);
            Width = width;
            Height = height;
        }

        public float Width { get; set; }
        public float Height { get; set; }
        public Vector Position { get; set; }
        public bool IsTriggerable { get; set; } = true;

        public bool HasTriggered(Vector position)
        {
            float left = Position.X;
            float right = Position.X + Width;
            float top = Position.Y;
            float bot = Position.Y + Height;
            return position.X <= right && position.X >= left && position.Y <= bot && position.Y >= top;
        }

        public static RectangleHitbox CreateFromLine(Vector start, Vector end, int width)
        {
            Vector pos = Vector.Zero;
            int height = 0;
            if(start.Y < end.Y)
            {
                pos.Y = start.Y;
                pos.X = start.X - (width / 2);
                height = Vector.GetDistance(start - end);
                return new RectangleHitbox(pos, width, height);
            }
            else if(start.Y > end.Y)
            {
                pos.Y = end.Y;
                pos.X = end.X - (width / 2);
                height = Vector.GetDistance(start - end);
                return new RectangleHitbox(pos, width, height);
            }
            else if(start.X < end.X)
            {
                pos.X = start.X;
                pos.Y = start.Y - (width / 2);
                height = Vector.GetDistance(start - end);
                return new RectangleHitbox(pos, height, width);
            }
            else
            {
                pos.X = end.X;
                pos.Y = end.Y - (width / 2);
                height = Vector.GetDistance(start - end);
                return new RectangleHitbox(pos, height, width);
            }
        }
    }
}
