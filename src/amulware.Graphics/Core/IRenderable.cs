namespace amulware.Graphics
{
    public interface IRenderable
    {
        void ConfigureBoundVertexArray(ShaderProgram program);
        void Render();
    }
}
