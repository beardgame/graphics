
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

    }
}