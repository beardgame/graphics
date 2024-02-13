using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Vertices;

public readonly struct VertexAttributeTemplate(
    string name,
    int size,
    int bytes,
    VertexAttribPointerType type,
    VertexAttributeFormat format)
{
    public int Bytes => bytes;
    public VertexAttribute ToAttribute(int offset, int stride) => new(name, size, type, stride, offset, format);
}
