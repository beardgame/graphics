using System;
using Bearded.Graphics.Vertices;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.MeshBuilders
{
    public interface IIndexedTrianglesMeshBuilder<TVertex, TIndex> : IIndexedMeshBuilder<TVertex, TIndex>
    {
    }

    public sealed class IndexedTrianglesMeshBuilder<TVertex>
        : IndexedMeshBuilder<TVertex>, IIndexedTrianglesMeshBuilder<TVertex, ushort>, IDisposable
        where TVertex : struct, IVertexData
    {
        public IndexedTrianglesMeshBuilder() : base(PrimitiveType.Triangles)
        {
        }
    }
}
