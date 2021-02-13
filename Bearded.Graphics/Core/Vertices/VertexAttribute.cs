using Bearded.Graphics.Shading;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Vertices
{
    public sealed class VertexAttribute
    {
        private readonly string name;
        private readonly int size;
        private readonly VertexAttribPointerType type;
        private readonly bool normalize;
        private readonly int stride;
        private readonly int offset;

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

        public void SetAttribute(ShaderProgram program)
        {
            var index = program.GetAttributeLocation(name);
            if (index == StatusCode.NotFound)
                return;
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, size, type, normalize, stride, offset);
        }

        public override string ToString() =>
            $"{{name: {name}, size: {size}, type: {type}, normalize: {normalize}, stride: {stride}, offset: {offset}}}";
    }
}
