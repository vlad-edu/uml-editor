using UmlEditor.Rendering;

namespace UmlEditor.Nodes.Interfaces
{
    public interface IRenderableNode : INode
    {
        void Render(Renderer renderer);
    }
}
