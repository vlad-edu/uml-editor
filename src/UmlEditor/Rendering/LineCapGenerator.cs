using System.Drawing.Drawing2D;
using UmlEditor.Geometry;

namespace UmlEditor.Rendering
{
    public static class LineCapGenerator
    {
        public static CustomLineCap DiamondCap(bool fill)
        {
            GraphicsPath path = new();
            CustomLineCap Diamond;
            path.AddLine(new Vector(0, 0), new Vector(5, 5));
            path.AddLine(new Vector(5, 5), new Vector(10, 0));
            path.AddLine(new Vector(10, 0), new Vector(5, -5));
            path.AddLine(new Vector(5, -5), new Vector(0, 0));
            if (fill)
                Diamond = new CustomLineCap(null, path);
            else
                Diamond = new CustomLineCap(path, path);
            return Diamond;
        }
    }
}
