using Bearded.Graphics.Vertices;

namespace Bearded.Graphics.Rendering;

public static class BufferExtensions
{
    public static IVertexBuffer AsVertexBuffer<TVertex>(this Buffer<TVertex> buffer)
        where TVertex : struct, IVertexData
    {
        return VertexBuffer.From(buffer);
    }

    public static IVertexBuffer AsVertexBuffer<TVertex>(this BufferStream<TVertex> buffer)
        where TVertex : struct, IVertexData
    {
        return VertexBuffer.From(buffer);
    }

    public static IIndexBuffer AsIndexBuffer(this Buffer<byte> buffer) => IndexBuffer.From(buffer);
    public static IIndexBuffer AsIndexBuffer(this Buffer<ushort> buffer) => IndexBuffer.From(buffer);
    public static IIndexBuffer AsIndexBuffer(this Buffer<uint> buffer) => IndexBuffer.From(buffer);
    public static IIndexBuffer AsIndexBuffer(this BufferStream<byte> buffer) => IndexBuffer.From(buffer);
    public static IIndexBuffer AsIndexBuffer(this BufferStream<ushort> buffer) => IndexBuffer.From(buffer);
    public static IIndexBuffer AsIndexBuffer(this BufferStream<uint> buffer) => IndexBuffer.From(buffer);
}
