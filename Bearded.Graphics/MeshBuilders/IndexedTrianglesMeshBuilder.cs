using System;
using Bearded.Graphics.Rendering;
using Bearded.Graphics.Vertices;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.MeshBuilders
{
    public sealed class IndexedTrianglesMeshBuilder<TVertex> : IIndexedTrianglesMeshBuilder<TVertex, ushort>, IDisposable
        where TVertex : struct, IVertexData
    {
        private readonly BufferStream<TVertex> vertices;
        private readonly BufferStream<ushort> indices;

        public IndexedTrianglesMeshBuilder()
        {
            vertices = new BufferStream<TVertex>(new Buffer<TVertex>());
            indices = new BufferStream<ushort>(new Buffer<ushort>());
        }

        public void Add(
            int vertexCount, int indexCount,
            out Span<TVertex> vertices, out Span<ushort> indices, out ushort indexOffset)
        {
            indexOffset = (ushort) this.vertices.Count;
            vertices = this.vertices.AddRange(vertexCount);
            indices = this.indices.AddRange(indexCount);
        }

        public IRenderable ToRenderable()
        {
            return Renderable.ForVerticesAndIndices(vertices, indices, PrimitiveType.Triangles);
        }

        public void Clear()
        {
            vertices.Clear();
            indices.Clear();
        }

        public void Dispose()
        {
            vertices.Buffer.Dispose();
            indices.Buffer.Dispose();
        }
    }
}
