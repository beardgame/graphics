using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a vertex attributes, defining the layout if <see cref="IVertexData"/> implementations.
    /// </summary>
    public sealed class VertexAttribute
    {
        private readonly string name;
        private readonly int size;
        private readonly VertexAttribPointerType type;
        private readonly bool normalize;
        private readonly int stride;
        private readonly int offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="size">The size.</param>
        /// <param name="type">The type.</param>
        /// <param name="stride">The stride.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="normalize">Whether to normalise the attribute's value when passing it to the shader.</param>
        public VertexAttribute(
            string name, int size, VertexAttribPointerType type, int stride, int offset, bool normalize = false)
        {
            this.name = name;
            this.size = size;
            this.type = type;
            this.stride = stride;
            this.offset = offset;
            this.normalize = normalize;
        }

        /// <summary>
        /// Sets the attribute for a specific program.
        /// </summary>
        /// <param name="program">The program.</param>
        public void SetAttribute(ShaderProgram program)
        {
            var index = program.GetAttributeLocation(name);
            if (index == StatusCode.NotFound)
                return;
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, size, type, normalize, stride, offset);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() =>
            $"{{name: {name}, size: {size}, type: {type}, normalize: {normalize}, stride: {stride}, offset: {offset}}}";
    }
}
