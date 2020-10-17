using OpenToolkit.Mathematics;

namespace amulware.Graphics.Shapes
{
    public interface IShapeDrawer3<in TVertexParameters>
    {
        void DrawTetrahedron(Vector3 center, float scale, TVertexParameters parameters);
        void DrawOctahedron(Vector3 center, float scale, TVertexParameters parameters);
        void DrawCuboid(float x, float y, float z, float w, float h, float d, TVertexParameters parameters);
        void DrawDodecahedron(Vector3 center, float scale, TVertexParameters parameters);
        void DrawIcosahedron(Vector3 center, float scale, TVertexParameters parameters);
    }
}
