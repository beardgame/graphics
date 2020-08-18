using amulware.Graphics.Rendering;

namespace amulware.Graphics.ShaderManagement
{
    public interface IRendererShader
    {
        void UseOnRenderer(Renderer renderer);
        void RemoveFromRenderer(Renderer renderer);
    }
}
