using Bearded.Graphics.Shading;

namespace Bearded.Graphics.Rendering
{
    public interface IRenderable
    {
        DrawCall MakeDrawCallFor(ShaderProgram program);
    }
}
