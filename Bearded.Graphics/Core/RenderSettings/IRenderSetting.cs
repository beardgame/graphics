using amulware.Graphics.Shading;

namespace amulware.Graphics.RenderSettings
{
    public interface IRenderSetting
    {
        void SetForProgram(ShaderProgram program);
        IProgramRenderSetting ForProgram(ShaderProgram program);
    }
}
