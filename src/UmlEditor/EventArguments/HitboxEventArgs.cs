using System;
using UmlEditor.Hitboxes;

namespace UmlEditor.EventArguments
{
    public class HitboxEventArgs : EventArgs
    {
        public IHitbox Hitbox;

        public HitboxEventArgs(IHitbox hitbox)
        {
            Hitbox = hitbox;
        }
    }
}
