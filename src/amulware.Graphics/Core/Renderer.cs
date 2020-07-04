namespace amulware.Graphics
{
    public class Renderer
    {
        private readonly IRenderable renderable;
        private readonly ShaderProgram shaderProgram;
        private readonly VertexArray vertexArray;

        public static Renderer From(IRenderable renderable, ShaderProgram shaderProgram)
        {
            return new Renderer(renderable, shaderProgram);
        }

        private Renderer(IRenderable renderable, ShaderProgram shaderProgram)
        {
            this.renderable = renderable;
            this.shaderProgram = shaderProgram;
            vertexArray = VertexArray.ForRenderable(renderable, shaderProgram);
        }

        public void Render()
        {
            // TODO: all this code could be in the vertex array, or a separate RenderState class
            // - but should it be?
            // TODO: figure out where shader program replacing mutability comes in
            using (shaderProgram.Use())
            using (vertexArray.Bind())
            {
                renderable.Render();
            }
        }
    }
}
