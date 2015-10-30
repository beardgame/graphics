using System.Collections.Generic;
using OpenTK;

namespace amulware.Graphics.Meshes.ObjFile
{
    partial class ObjFileMesh
    {
        private class Builder
        {
            private List<Vector4> vertices = new List<Vector4>();
            private List<Vector3> uvCoordinates = new List<Vector3>();
            private List<Vector3> normals = new List<Vector3>();
            private List<Face> faces = new List<Face>();

            public void AddVertex(Vector4 vertex)
            {
                this.vertices.Add(vertex);
            }
            public void AddUV(Vector3 uv)
            {
                this.uvCoordinates.Add(uv);
            }
            public void AddNormal(Vector3 normal)
            {
                this.normals.Add(normal);
            }
            public void AddFace(Face face)
            {
                this.faces.Add(face);
            }

            public ObjFileMesh Build()
            {
                var obj = new ObjFileMesh(
                    this.vertices, this.uvCoordinates, this.normals, this.faces);

                // forget about lists, so they cannot be modified any longer
                // somewhat dirty implementation of builder pattern,
                // but prevents having to copy all data to mesh
                this.vertices = null;
                this.normals = null;
                this.uvCoordinates = null;
                this.faces = null;

                return obj;
            }
        }
    }
}