using System.Collections.Generic;

namespace amulware.Graphics.Meshes
{
    sealed class Mesh
    {
        private readonly MeshVertex[] vertices;
        private readonly IndexTriangle[] triangles;

        private Mesh(MeshVertex[] vertices, IndexTriangle[] triangles)
        {
            this.vertices = vertices;
            this.triangles = triangles;
        }

        public class Builder
        {
            private readonly List<MeshVertex> vertices;
            private readonly List<IndexTriangle> triangles;

            public Builder(int vertexCapacity = 0, int triangleCapacity = 0)
            {
                this.vertices = new List<MeshVertex>(vertexCapacity);
                this.triangles = new List<IndexTriangle>(triangleCapacity);
            }

            public Mesh Build()
            {
                return new Mesh(
                    this.vertices.ToArray(),
                    this.triangles.ToArray()
                    );
            }

            public int AddVertex(MeshVertex v)
            {
                var i = this.vertices.Count;
                this.vertices.Add(v);
                return i;
            }

            #region AddVertices()
            public int AddVertices(MeshVertex v0, MeshVertex v1)
            {
                var i = this.vertices.Count;
                this.vertices.Add(v0);
                this.vertices.Add(v1);
                return i;
            }
            public int AddVertices(MeshVertex v0, MeshVertex v1, MeshVertex v2)
            {
                var i = this.vertices.Count;
                this.vertices.Add(v0);
                this.vertices.Add(v1);
                this.vertices.Add(v2);
                return i;
            }
            public int AddVertices(MeshVertex v0, MeshVertex v1, MeshVertex v2, MeshVertex v3)
            {
                var i = this.vertices.Count;
                this.vertices.Add(v0);
                this.vertices.Add(v1);
                this.vertices.Add(v2);
                this.vertices.Add(v3);
                return i;
            }
            public int AddVertices(params MeshVertex[] vs)
            {
                var i = this.vertices.Count;
                this.vertices.AddRange(vs);
                return i;
            }
            #endregion

            public void AddTriangle(IndexTriangle t)
            {
                this.triangles.Add(t);
            }

            public void AddTriangles(IndexTriangle t0, IndexTriangle t1)
            {
                this.triangles.Add(t0);
                this.triangles.Add(t1);
            }
            public void AddTriangles(params IndexTriangle[] ts)
            {
                this.triangles.AddRange(ts);
            }
        }
    }
}