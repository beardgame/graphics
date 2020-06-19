using System.Collections.Generic;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Meshes.ObjFile
{
    /// <summary>
    /// A class representing a mesh loaded from a wavefront *.obj file.
    /// </summary>
    public sealed partial class ObjFileMesh
    {
        private readonly List<Vector4> positions;
        private readonly List<Vector3> uvCoordinates;
        private readonly List<Vector3> normals;
        private readonly List<Face> faces;

        private ObjFileMesh(
            List<Vector4> vertices, List<Vector3> uvCoordinates,
            List<Vector3> normals, List<Face> faces
            )
        {
            this.positions = vertices;
            this.uvCoordinates = uvCoordinates;
            this.normals = normals;
            this.faces = faces;
        }

    }
}
