using amulware.Graphics.Shading;

namespace amulware.Graphics.Rendering
{
    public interface IRenderable
    {
        DrawCall MakeDrawCallFor(ShaderProgram program);
    }
}
