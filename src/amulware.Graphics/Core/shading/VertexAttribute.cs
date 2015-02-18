using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a vertex attributes, defining the layout if <see cref="IVertexData"/> implementations.
    /// </summary>
    sealed public class VertexAttribute
    {
        readonly string name;
        readonly int size;
        readonly VertexAttribPointerType type;
        readonly bool normalize;
        readonly int stride;
        readonly int offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="size">The size.</param>
        /// <param name="type">The type.</param>
        /// <param name="stride">The stride.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="normalize">Whether to normalise the attribute's value when passing it to the shader.</param>
        public VertexAttribute(string name, int size, VertexAttribPointerType type,
            int stride, int offset, bool normalize = false)
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
        public void setAttribute(ShaderProgram program)
        {
            int index = program.GetAttributeLocation(this.name);
            if (index == -1)
                return;
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, this.size, this.type,
                this.normalize, this.stride, this.offset);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "{name:" + this.name
                + ", size:" + this.size
                + ", type:" + this.type
                + ", normalize:" + this.normalize
                + ", stride:" + this.stride
                + ", offset:" + this.offset
                + "}";
        }
    }
}
