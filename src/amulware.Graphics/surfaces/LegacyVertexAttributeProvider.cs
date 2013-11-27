namespace amulware.Graphics
{
    internal class LegacyVertexAttributeProvider<TVertexData>
        : IVertexAttributeProvider<TVertexData>
        where TVertexData : struct, IVertexData
    {
        private ShaderProgram program;

        public void SetVertexData()
        {
            if (this.program != null)
                this.program.SetVertexAttributes(new TVertexData().VertexAttributes());
        }

        public void UnSetVertexData()
        {

        }

        public void SetShaderProgram(ShaderProgram program)
        {
            this.program = program;
        }
    }
}
