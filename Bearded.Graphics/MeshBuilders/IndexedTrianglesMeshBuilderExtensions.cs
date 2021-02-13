namespace Bearded.Graphics.MeshBuilders
{
    public static class IndexedTrianglesMeshBuilderExtensions
    {
        public static void AddTriangle<TVertex>(
            this IIndexedTrianglesMeshBuilder<TVertex, ushort> meshBuilder, in TVertex v0, in TVertex v1, in TVertex v2)
        {
            meshBuilder.Add(3, 3, out var vertices, out var indices, out var indexOffset);

            vertices[0] = v0;
            vertices[1] = v1;
            vertices[2] = v2;

            indices[0] = indexOffset;
            indices[1] = (ushort) (indexOffset + 1);
            indices[2] = (ushort) (indexOffset + 2);
        }

        public static void AddQuad<TVertex>(
            this IIndexedTrianglesMeshBuilder<TVertex, ushort> meshBuilder,
            in TVertex v0, in TVertex v1, in TVertex v2, in TVertex v3)
        {
            meshBuilder.Add(4, 6, out var vertices, out var indices, out var indexOffset);

            vertices[0] = v0;
            vertices[1] = v1;
            vertices[2] = v2;
            vertices[3] = v3;

            indices[0] = indexOffset;
            indices[1] = (ushort) (indexOffset + 1);
            indices[2] = (ushort) (indexOffset + 2);

            indices[3] = indexOffset;
            indices[4] = (ushort) (indexOffset + 2);
            indices[5] = (ushort) (indexOffset + 3);
        }
    }
}
