using amulware.Graphics.Rendering;

namespace amulware.Graphics.ShaderManagement
{
    public interface IRendererShader
    {
        void UseOnRenderer(IRenderer renderer);
        void RemoveFromRenderer(IRenderer renderer);
    }
}
