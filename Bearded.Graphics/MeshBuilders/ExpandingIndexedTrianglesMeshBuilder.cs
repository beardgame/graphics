using System;
using Bearded.Graphics.Rendering;
using Bearded.Graphics.Vertices;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.MeshBuilders
{
    public sealed class ExpandingIndexedTrianglesMeshBuilder<TVertex> : IIndexedTrianglesMeshBuilder<TVertex, ushort>, IDisposable
        where TVertex : struct, IVertexData
    {
        private readonly Batcher<Streams> batcher;
        private Streams currentStreams;

        private readonly struct Streams : IDisposable
        {
            public BufferStream<TVertex> Vertices { get; }
            public BufferStream<ushort> Indices { get; }

            public static Streams Create()
            {
                return new Streams(
                    new BufferStream<TVertex>(new Buffer<TVertex>()),
                    new BufferStream<ushort>(new Buffer<ushort>())
                );
            }

            private Streams(BufferStream<TVertex> vertices, BufferStream<ushort> indices)
            {
                Vertices = vertices;
                Indices = indices;
            }

            public void Dispose()
            {
                Vertices.Buffer.Dispose();
                Indices.Buffer.Dispose();
            }
        }

        public ExpandingIndexedTrianglesMeshBuilder()
        {
            batcher = new Batcher<Streams>(Streams.Create);
            currentStreams = batcher.AllocateBatch().Data;
        }

        public void Add(
            int vertexCount, int indexCount,
            out Span<TVertex> vertices, out Span<ushort> indices, out ushort indexOffset)
        {
            // TODO(#25): due to this potentially allocating new buffers, Add has to be called on the GL thread
            // a possible solution would be to wrap BufferStream into a LazyBufferStream that only creates buffers
            // on the first upload
            ensureCapacity(vertexCount);

            indexOffset = (ushort) currentStreams.Vertices.Count;
            vertices = currentStreams.Vertices.AddRange(vertexCount);
            indices = currentStreams.Indices.AddRange(indexCount);
        }

        private void ensureCapacity(int vertexCount)
        {
            const int maxVertexCount = ushort.MaxValue;

            if (currentStreams.Vertices.Count + vertexCount <= maxVertexCount)
                return;

            currentStreams = batcher.AllocateBatch().Data;
        }

        public void Clear()
        {
            foreach (var batch in batcher.ActiveBatches)
            {
                batch.Data.Vertices.Clear();
                batch.Data.Indices.Clear();
            }
            batcher.FreeAll();
            currentStreams = batcher.AllocateBatch().Data;
        }

        public IBatchedRenderable ToRenderable()
        {
            return Renderable.ForBatchedVerticesAndIndices(batcher, streams => (streams.Vertices, streams.Indices), PrimitiveType.Triangles);
        }

        public void Dispose()
        {
            batcher.Dispose();
        }
    }
}
