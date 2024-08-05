using System;
using Bearded.Graphics.Shading;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Vertices;

public readonly struct VertexAttribute(
    string name,
    int size,
    VertexAttribPointerType type,
    int stride,
    int offset,
    VertexAttributeFormat format,
    int divisor)
{
    public void SetAttribute(ShaderProgram program)
    {
        var index = program.GetAttributeLocation(name);
        if (index == StatusCode.NotFound)
            return;

        GL.EnableVertexAttribArray(index);

        switch (format)
        {
            case VertexAttributeFormat.Float:
                GL.VertexAttribPointer(index, size, type, false, stride, offset);
                break;
            case VertexAttributeFormat.FloatNormalized:
                GL.VertexAttribPointer(index, size, type, true, stride, offset);
                break;
            case VertexAttributeFormat.Double:
                GL.VertexAttribLPointer(index, size, VertexAttribDoubleType.Double, stride, new IntPtr(offset));
                break;
            case VertexAttributeFormat.Integer:
                GL.VertexAttribIPointer(index, size, (VertexAttribIntegerType)type, stride, new IntPtr(offset));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        GL.VertexAttribDivisor(index, divisor);
    }

    public override string ToString() =>
        $"{{name: {name}, size: {size}, type: {type}, format: {format}, stride: {stride}, offset: {offset}}}";
}
