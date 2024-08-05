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

    public static IIndexBuffer AsIndexBuffer<TIndex>(this Buffer<TIndex> buffer)
        where TIndex : struct
    {
        return IndexBuffer.From(buffer);
    }

    public static IIndexBuffer AsIndexBuffer<TIndex>(this BufferStream<TIndex> buffer)
        where TIndex : struct
    {
        return IndexBuffer.From(buffer);
    }
}
