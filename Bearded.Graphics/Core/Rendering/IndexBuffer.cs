using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Rendering;

public interface IIndexBuffer
{
    int Count { get; }
    DrawElementsType ElementType { get; }
    void ConfigureBoundVertexArray();
}

public static class IndexBuffer
{
    public static IIndexBuffer From<TIndex>(Buffer<TIndex> buffer)
        where TIndex : struct
    {
        return new Static<TIndex>(buffer);
    }

    public static IIndexBuffer From<TIndex>(BufferStream<TIndex> stream)
        where TIndex : struct
    {
        return new Streaming<TIndex>(stream);
    }

    private sealed class Static<TIndex>(Buffer<TIndex> buffer) : IIndexBuffer
        where TIndex : struct
    {
        public int Count => buffer.Count;

        public DrawElementsType ElementType { get; } = elementType<TIndex>();

        public void ConfigureBoundVertexArray() => buffer.Bind(BufferTarget.ElementArrayBuffer);
    }

    private sealed class Streaming<TIndex>(BufferStream<TIndex> stream) : IIndexBuffer, IFlushableBuffer
        where TIndex : struct
    {
        public int Count => stream.Count;

        public DrawElementsType ElementType { get; } = elementType<TIndex>();

        public void ConfigureBoundVertexArray() => stream.Buffer.Bind(BufferTarget.ElementArrayBuffer);

        public void FlushIfNeeded() => stream.FlushIfDirty();
    }

    private static DrawElementsType elementType<TIndex>()
    {
        return default(TIndex) switch
        {
            byte => DrawElementsType.UnsignedByte,
            ushort => DrawElementsType.UnsignedShort,
            uint => DrawElementsType.UnsignedInt,
            _ => throw new NotSupportedException("Index type must be one of [byte, ushort, uint].")
        };
    }
}
