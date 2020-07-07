namespace amulware.Graphics
{
    public interface IRenderSetting
    {
        IProgramRenderSetting ForProgram(ShaderProgram program);
    }
}
