using System;
using Bearded.Graphics.Rendering;
using Bearded.Graphics.Vertices;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.MeshBuilders
{
    public class IndexedMeshBuilder<TVertex>
        where TVertex : struct, IVertexData
    {
        private readonly PrimitiveType primitiveType;
        protected BufferStream<TVertex> Vertices { get; }
        protected BufferStream<ushort> Indices { get; }

        public IndexedMeshBuilder(PrimitiveType primitiveType)
        {
            this.primitiveType = primitiveType;
            Vertices = new BufferStream<TVertex>(new Buffer<TVertex>());
            Indices = new BufferStream<ushort>(new Buffer<ushort>());
        }

        public IRenderable ToRenderable()
        {
            return Renderable.Build(primitiveType, b => b
                .With(Vertices.AsVertexBuffer())
                .With(Indices.AsIndexBuffer())
            );
        }

        public void Add(
            int vertexCount, int indexCount,
            out Span<TVertex> vertices, out Span<ushort> indices, out ushort indexOffset)
        {
            indexOffset = (ushort) Vertices.Count;
            vertices = Vertices.AddRange(vertexCount);
            indices = Indices.AddRange(indexCount);
        }

        public void Clear()
        {
            Vertices.Clear();
            Indices.Clear();
        }

        public void Dispose()
        {
            Vertices.Buffer.Dispose();
            Indices.Buffer.Dispose();
        }
    }
}
