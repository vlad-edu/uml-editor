using System;
using System.Drawing;

namespace UmlEditor.Geometry
{
    public struct Vector
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector Zero = new(0, 0);

        public static Vector operator +(Vector left, Vector right)
        {
            return new Vector(left.X + right.X, left.Y + right.Y);
        }
        public static Vector operator -(Vector left, Vector right)
        {
            return new Vector(left.X - right.X, left.Y - right.Y);
        }
        public static Vector operator *(Vector left, Vector right)
        {
            return new Vector(left.X * right.X, left.Y * right.Y);
        }
        public static Vector operator /(Vector left, Vector right)
        {
            return new Vector(left.X / right.X, left.Y / right.Y);
        }
        public static bool operator ==(Vector left, Vector right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        public static bool operator !=(Vector left, Vector right)
        {
            return left.X != right.X || left.Y != right.Y;
        }

        public static implicit operator Point(Vector v)
        {
            return new Point((int)v.X, (int)v.Y);
        }
        public static implicit operator Vector(Point p)
        {
            return new Vector(p.X, p.Y);
        }

        public static implicit operator PointF(Vector v)
        {
            return new PointF(v.X, v.Y);
        }
        public static implicit operator Vector(PointF p)
        {
            return new Vector(p.X, p.Y);
        }
        public static Vector operator +(Vector left, int right)
        {
            return new Vector(left.X + right, left.Y + right);
        }
        public static Vector operator -(Vector left, int right)
        {
            return new Vector(left.X - right, left.Y - right);
        }
        public static Vector operator *(Vector left, int right)
        {
            return new Vector(left.X * right, left.Y * right);
        }
        public static Vector operator /(Vector left, int right)
        {
            return new Vector(left.X / right, left.Y / right);
        }
        public static Vector operator +(Vector left, float right)
        {
            return new Vector(left.X + right, left.Y + right);
        }
        public static Vector operator -(Vector left, float right)
        {
            return new Vector(left.X - right, left.Y - right);
        }
        public static Vector operator *(Vector left, float right)
        {
            return new Vector(left.X * right, left.Y * right);
        }
        public static Vector operator /(Vector left, float right)
        {
            return new Vector(left.X / right, left.Y / right);
        }
        public static int GetDistance(Vector v)
        {
            return (int)Math.Sqrt((int)Math.Pow((double)v.X, 2) + (int)Math.Pow((double)v.Y, 2));
        }
    }
}
