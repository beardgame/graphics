using Bearded.Graphics.Shading;
using Bearded.Graphics.Vertices;

namespace Bearded.Graphics.Rendering;

public interface IVertexBuffer
{
    int Count { get; }
    void ConfigureBoundVertexArray(ShaderProgram program);
}

public static class VertexBuffer
{
    public static IVertexBuffer From<TVertex>(Buffer<TVertex> buffer)
        where TVertex : struct, IVertexData
    {
        return new Static<TVertex>(buffer);
    }

    public static IVertexBuffer From<TVertex>(BufferStream<TVertex> stream)
        where TVertex : struct, IVertexData
    {
        return new Streaming<TVertex>(stream);
    }

    private sealed class Static<TVertex>(Buffer<TVertex> buffer) : IVertexBuffer
        where TVertex : struct, IVertexData
    {
        public int Count => buffer.Count;

        public void ConfigureBoundVertexArray(ShaderProgram program)
        {
            using var _ = buffer.Bind();
            VertexData.SetAttributes<TVertex>(program);
        }
    }

    private sealed class Streaming<TVertex>(BufferStream<TVertex> stream) : IVertexBuffer, IFlushableBuffer
        where TVertex : struct, IVertexData
    {
        public int Count => stream.Count;

        public void ConfigureBoundVertexArray(ShaderProgram program)
        {
            using var _ = stream.Buffer.Bind();
            VertexData.SetAttributes<TVertex>(program);
        }

        public void FlushIfNeeded() => stream.FlushIfDirty();
    }
}
