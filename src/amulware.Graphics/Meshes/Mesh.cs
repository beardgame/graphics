using System;

namespace amulware.Graphics.Meshes
{
    /// <summary>
    /// Represents an immutable mesh of vertices and triangles.
    /// </summary>
    public sealed partial class Mesh
    {
        private readonly MeshVertex[] vertices;
        private readonly IndexTriangle[] triangles;

        private Mesh(MeshVertex[] vertices, IndexTriangle[] triangles)
        {
            this.vertices = vertices;
            this.triangles = triangles;
        }

        /// <summary>
        /// Converts the mesh into a renderable indexed surface.
        /// </summary>
        /// <param name="transform">A function to apply to all the vertices.</param>
        public IndexedSurface<MeshVertex> ToIndexedSurface(
            Func<MeshVertex, MeshVertex> transform = null
            )
        {
            var surface = new IndexedSurface<MeshVertex>();

            this.writeVertices(surface, transform);
            this.writeIndices(surface);

            return surface;
        }
        /// <summary>
        /// Converts the mesh into a renderable surface as a point cloud. Only vertices and no triangles are included.
        /// </summary>
        /// <param name="transform">A function to apply to all the vertices.</param>
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
                .WriteIndicesDirectly(this.triangles.Length * 3, out iOffset);

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