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
            vertexArray = VertexArray.For(renderable, shaderProgram);
        }

        public void Render()
        {
            // TODO: figure out where shader program replacing mutability comes in
            using (shaderProgram.Use())
            {
                vertexArray.Render();
            }
        }
    }
}
