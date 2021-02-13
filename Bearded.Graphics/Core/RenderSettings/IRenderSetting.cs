using Bearded.Graphics.Shading;

namespace Bearded.Graphics.RenderSettings
{
    public interface IRenderSetting
    {
        void SetForProgram(ShaderProgram program);
        IProgramRenderSetting ForProgram(ShaderProgram program);
    }
}
