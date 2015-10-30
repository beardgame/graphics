using System.Collections.Generic;

namespace amulware.Graphics.Meshes
{
    partial class Mesh
    {
        /// <summary>
        /// A mutable builder class to help construct immutable meshes.
        /// </summary>
        public class Builder
        {
            private readonly List<MeshVertex> vertices;
            private readonly List<IndexTriangle> triangles;

            /// <summary>
            /// Creates a new mesh builder.
            /// If known, specifying the vertex and/or triangle capacity
            /// will prevent additional memory allocations for better performance.
            /// </summary>
            public Builder(int vertexCapacity = 0, int triangleCapacity = 0)
            {
                this.vertices = new List<MeshVertex>(vertexCapacity);
                this.triangles = new List<IndexTriangle>(triangleCapacity);
            }

            /// <summary>
            /// Builds and return a mesh with the builder's data.
            /// </summary>
            public Mesh Build()
            {
                return new Mesh(
                    this.vertices.ToArray(),
                    this.triangles.ToArray()
                    );
            }

            /// <summary>
            /// Adds a vertex to the builder.
            /// </summary>
            /// <returns>The index of the vertex for triangle creation.</returns>
            public int AddVertex(MeshVertex v)
            {
                var i = this.vertices.Count;
                this.vertices.Add(v);
                return i;
            }

            #region AddVertices()
            /// <summary>
            /// Adds a vertex to the builder.
            /// </summary>
            /// <returns>The index of the first vertex for triangle creation.
            /// Indices for following vertices incrent by 1 each.</returns>
            public int AddVertices(MeshVertex v0, MeshVertex v1)
            {
                var i = this.vertices.Count;
                this.vertices.Add(v0);
                this.vertices.Add(v1);
                return i;
            }
            /// <summary>
            /// Adds a vertex to the builder.
            /// </summary>
            /// <returns>The index of the first vertex for triangle creation.
            /// Indices for following vertices incrent by 1 each.</returns>
            public int AddVertices(MeshVertex v0, MeshVertex v1, MeshVertex v2)
            {
                var i = this.vertices.Count;
                this.vertices.Add(v0);
                this.vertices.Add(v1);
                this.vertices.Add(v2);
                return i;
            }
            /// <summary>
            /// Adds a vertex to the builder.
            /// </summary>
            /// <returns>The index of the first vertex for triangle creation.
            /// Indices for following vertices incrent by 1 each.</returns>
            public int AddVertices(MeshVertex v0, MeshVertex v1, MeshVertex v2, MeshVertex v3)
            {
                var i = this.vertices.Count;
                this.vertices.Add(v0);
                this.vertices.Add(v1);
                this.vertices.Add(v2);
                this.vertices.Add(v3);
                return i;
            }
            /// <summary>
            /// Adds a vertex to the builder.
            /// </summary>
            /// <returns>The index of the first vertex for triangle creation.
            /// Indices for following vertices incrent by 1 each.</returns>
            public int AddVertices(params MeshVertex[] vs)
            {
                var i = this.vertices.Count;
                this.vertices.AddRange(vs);
                return i;
            }
            /// <summary>
            /// Adds a vertex to the builder.
            /// </summary>
            /// <returns>The index of the first vertex for triangle creation.
            /// Indices for following vertices incrent by 1 each.</returns>
            public int AddVertices(IEnumerable<MeshVertex> vs)
            {
                var i = this.vertices.Count;
                this.vertices.AddRange(vs);
                return i;
            }
            #endregion

            /// <summary>
            /// Adds a triangle to the builder.
            /// </summary>
            public void AddTriangle(IndexTriangle t)
            {
                this.triangles.Add(t);
            }

            /// <summary>
            /// Adds triangles to the builder.
            /// </summary>
            public void AddTriangles(IndexTriangle t0, IndexTriangle t1)
            {
                this.triangles.Add(t0);
                this.triangles.Add(t1);
            }
            /// <summary>
            /// Adds triangles to the builder.
            /// </summary>
            public void AddTriangles(params IndexTriangle[] ts)
            {
                this.triangles.AddRange(ts);
            }
            /// <summary>
            /// Adds triangles to the builder.
            /// </summary>
            public void AddTriangles(IEnumerable<IndexTriangle> ts)
            {
                this.triangles.AddRange(ts);
            }
        }

    }

}