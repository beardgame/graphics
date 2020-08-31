using amulware.Graphics.Shading;

namespace amulware.Graphics.Rendering
{
    public interface IRenderer
    {
        public void SetShaderProgram(ShaderProgram program);

        public void Render();
    }
}
