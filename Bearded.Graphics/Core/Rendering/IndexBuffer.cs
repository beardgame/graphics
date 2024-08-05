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
    public static IIndexBuffer From(Buffer<byte> buffer) => new Static<byte>(buffer);
    public static IIndexBuffer From(Buffer<ushort> buffer) => new Static<ushort>(buffer);
    public static IIndexBuffer From(Buffer<uint> buffer) => new Static<uint>(buffer);

    public static IIndexBuffer From(BufferStream<byte> stream) => new Streaming<byte>(stream);
    public static IIndexBuffer From(BufferStream<ushort> stream) => new Streaming<ushort>(stream);
    public static IIndexBuffer From(BufferStream<uint> stream) => new Streaming<uint>(stream);

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
