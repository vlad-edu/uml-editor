using UmlEditor.Geometry;

namespace UmlEditor.Hitboxes
{
    public interface IHitbox
    {
        bool HasTriggered(Vector position);
        bool IsTriggerable { get; set; }
    }
}
