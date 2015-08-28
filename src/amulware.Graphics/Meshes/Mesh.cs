using System;

namespace amulware.Graphics.Meshes
{
    sealed partial class Mesh
    {
        private readonly MeshVertex[] vertices;
        private readonly IndexTriangle[] triangles;

        private Mesh(MeshVertex[] vertices, IndexTriangle[] triangles)
        {
            this.vertices = vertices;
            this.triangles = triangles;
        }

        public IndexedSurface<MeshVertex> ToIndexedSurface(
            Func<MeshVertex, MeshVertex> transform = null
            )
        {
            var surface = new IndexedSurface<MeshVertex>();

            this.writeVertices(surface, transform);
            this.writeIndices(surface);

            return surface;
        }
        public VertexSurface<MeshVertex> ToPointCloudSurface(
            Func<MeshVertex, MeshVertex> transform = null
            )
        {
            var surface = new VertexSurface<MeshVertex>();

            this.writeVertices(surface, transform);

            return surface;
        }

        private void writeIndices(IndexedSurface<MeshVertex> surface)
        {
            int iOffset;
            var indexArray = surface
                .WriteIndicesDirectly(this.vertices.Length * 3, out iOffset);

            if (iOffset != 0)
            {
                // test not strictly speaking necessary, but enforces usage
                throw new Exception("Expected index offset to be zero.");
            }

            for (int i = 0; i < this.triangles.Length; i++)
            {
                var triangle = this.triangles[i];

                indexArray[iOffset] = (ushort)triangle.Index0;
                indexArray[iOffset + 1] = (ushort)triangle.Index1;
                indexArray[iOffset + 2] = (ushort)triangle.Index2;

                iOffset += 3;
            }
        }

        private void writeVertices(
            VertexSurface<MeshVertex> surface,
            Func<MeshVertex, MeshVertex> transform = null
            )
        {
            ushort vOffset;
            var vertexArray = surface
                .WriteVerticesDirectly(this.vertices.Length, out vOffset);

            if (vOffset != 0)
            {
                throw new Exception("Expected vertex offset to be zero.");
            }

            if (transform == null)
            {
                this.vertices.CopyTo(vertexArray, 0);
            }
            else
            {
                for (int i = 0; i < this.vertices.Length; i++)
                {
                    vertexArray[i] = transform(this.vertices[i]);
                }
            }
        }
    }
}