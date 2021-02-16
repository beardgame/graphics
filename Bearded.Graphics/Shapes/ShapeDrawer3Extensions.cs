using OpenTK.Mathematics;

namespace Bearded.Graphics.Shapes
{
    public static class ShapeDrawer3Extensions
    {
        public static void DrawCube<TVertexParameters>(
            this IShapeDrawer3<TVertexParameters> drawer, Vector3 center, float scale, TVertexParameters parameters)
        {
            const float u = 0.5f;

            var (x, y, z) = center;
            drawer.DrawCuboid(x - u * scale, y - u * scale, z - u * scale, scale, scale, scale, parameters);
        }

        public static void DrawCuboid<TVertexParameters>(
            this IShapeDrawer3<TVertexParameters> drawer, Vector3 xyz, Vector3 whd, TVertexParameters parameters)
        {
            var (x, y, z) = xyz;
            var (w, h, d) = whd;
            drawer.DrawCuboid(x, y, z, w, h, d, parameters);
        }
    }
}
