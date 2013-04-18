using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class VertexAttribute
    {
        readonly string name;
        readonly int size;
        readonly VertexAttribPointerType type;
        readonly bool normalize;
        readonly int stride;
        readonly int offset;

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

        public void setAttribute(ShaderProgram program)
        {
            int index = program.GetAttributeLocation(this.name);
            if (index == -1)
                return;
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, this.size, this.type,
                this.normalize, this.stride, this.offset);
        }

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
