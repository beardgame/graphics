using OpenTK.Mathematics;

namespace Bearded.Graphics.Shapes
{
    public interface IShapeDrawer3<in TVertexParameters>
    {
        void DrawTetrahedron(Vector3 center, float scale, TVertexParameters parameters);
        void DrawOctahedron(Vector3 center, float scale, TVertexParameters parameters);
        void DrawCuboid(float x, float y, float z, float w, float h, float d, TVertexParameters parameters);
        void DrawDodecahedron(Vector3 center, float scale, TVertexParameters parameters);
        void DrawIcosahedron(Vector3 center, float scale, TVertexParameters parameters);

        void DrawCone(
            Vector3 baseCenter, Vector3 baseToApex, float baseRadius, TVertexParameters parameters, int edges = 32);
    }
}
