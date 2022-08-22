using System;

namespace Bearded.Graphics.MeshBuilders
{
    public interface IIndexedTrianglesMeshBuilder<TVertex, TIndex>
    {
        void Add(
            int vertexCount, int indexCount,
            out Span<TVertex> vertices, out Span<TIndex> indices, out TIndex indexOffset);

        void Clear();
    }
}
