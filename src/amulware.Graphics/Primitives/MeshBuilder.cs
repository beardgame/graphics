using System;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public sealed class MeshBuilder<TVertex> : IIndexedMeshBuilder<TVertex, ushort>, IDisposable
        where TVertex : struct, IVertexData
    {
        private readonly BufferStream<TVertex> vertices;
        private readonly BufferStream<ushort> indices;

        public MeshBuilder()
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

        public IRenderable ToClearingRenderable()
        {
            return new ClearingRenderable(ToRenderable(), this);
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

        private sealed class ClearingRenderable : IRenderable
        {
            private readonly IRenderable baseRenderer;
            private readonly MeshBuilder<TVertex> builder;

            public ClearingRenderable(IRenderable baseRenderer, MeshBuilder<TVertex> builder)
            {
                this.baseRenderer = baseRenderer;
                this.builder = builder;
            }

            public void ConfigureBoundVertexArray(ShaderProgram program)
            {
                baseRenderer.ConfigureBoundVertexArray(program);
            }

            public void Render()
            {
                baseRenderer.Render();
                builder.Clear();
            }
        }

    }
}