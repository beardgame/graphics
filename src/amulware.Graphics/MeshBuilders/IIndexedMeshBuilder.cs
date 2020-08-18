using System;

namespace amulware.Graphics.MeshBuilders
{
    public interface IIndexedMeshBuilder<TVertex, TIndex>
    {
        void Add(
            int vertexCount, int indexCount,
            out Span<TVertex> vertices, out Span<TIndex> indices, out TIndex indexOffset);

        void Clear();
    }
}
