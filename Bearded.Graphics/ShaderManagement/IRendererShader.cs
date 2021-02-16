using Bearded.Graphics.Rendering;

namespace Bearded.Graphics.ShaderManagement
{
    public interface IRendererShader
    {
        void UseOnRenderer(IRenderer renderer);
        void RemoveFromRenderer(IRenderer renderer);
    }
}
