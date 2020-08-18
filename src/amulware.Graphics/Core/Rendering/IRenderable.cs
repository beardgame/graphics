using amulware.Graphics.Shading;

namespace amulware.Graphics.Rendering
{
    public interface IRenderable
    {
        void ConfigureBoundVertexArray(ShaderProgram program);
        void Render();
    }
}
