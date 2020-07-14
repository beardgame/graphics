namespace amulware.Graphics
{
    public interface IRenderSetting
    {
        void SetForProgram(ShaderProgram program);
        IProgramRenderSetting ForProgram(ShaderProgram program);
    }
}
