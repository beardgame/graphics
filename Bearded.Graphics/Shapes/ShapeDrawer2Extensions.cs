using OpenTK.Mathematics;

namespace Bearded.Graphics.Shapes
{
    public static class ShapeDrawer2Extensions
    {
        public static void FillRectangle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector2 xy, Vector2 wh, TVertexParameters parameters)
        {
            drawer.FillRectangle(xy.X, xy.Y, 0, wh.X, wh.Y, parameters);
        }

        public static void FillRectangle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector3 xyz, Vector2 wh, TVertexParameters parameters)
        {
            drawer.FillRectangle(xyz.X, xyz.Y, xyz.Z, wh.X, wh.Y, parameters);
        }

        public static void FillRectangle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            float x, float y, float w, float h, TVertexParameters parameters)
        {
            drawer.FillRectangle(x, y, 0, w, h, parameters);
        }

        public static void DrawRectangle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector2 xy, Vector2 wh, float lineWidth, TVertexParameters parameters)
        {
            drawer.DrawRectangle(xy.X, xy.Y, 0, wh.X, wh.Y, lineWidth, parameters);
        }

        public static void DrawRectangle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector3 xyz, Vector2 wh, float lineWidth, TVertexParameters parameters)
        {
            drawer.DrawRectangle(xyz.X, xyz.Y, xyz.Z, wh.X, wh.Y, lineWidth, parameters);
        }

        public static void DrawRectangle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            float x, float y, float w, float h, float lineWidth, TVertexParameters parameters)
        {
            drawer.DrawRectangle(x, y, 0, w, h, lineWidth, parameters);
        }

        public static void FillCircle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            float x, float y, float r, TVertexParameters parameters, int edges = 32)
        {
            drawer.FillOval(x, y, 0, r, r, parameters, edges);
        }

        public static void FillCircle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector2 xy, float r, TVertexParameters parameters, int edges = 32)
        {
            drawer.FillOval(xy.X, xy.Y, 0, r, r, parameters, edges);
        }

        public static void FillCircle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            float x, float y, float z, float r, TVertexParameters parameters, int edges = 32)
        {
            drawer.FillOval(x, y, z, r, r, parameters, edges);
        }

        public static void FillCircle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector3 xyz, float r, TVertexParameters parameters, int edges = 32)
        {
            drawer.FillOval(xyz.X, xyz.Y, xyz.Z, r, r, parameters, edges);
        }

        public static void DrawCircle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            float x, float y, float r, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            drawer.DrawOval(x, y, 0, r, r, lineWidth, parameters, edges);
        }

        public static void DrawCircle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector2 xy, float r, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            drawer.DrawOval(xy.X, xy.Y, 0, r, r, lineWidth, parameters, edges);
        }

        public static void DrawCircle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            float x, float y, float z, float r, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            drawer.DrawOval(x, y, z, r, r, lineWidth, parameters, edges);
        }

        public static void DrawCircle<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector3 xyz, float r, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            drawer.DrawOval(xyz.X, xyz.Y, xyz.Z, r, r, lineWidth, parameters, edges);
        }

        public static void FillOval<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector2 xy, Vector2 wh, TVertexParameters parameters, int edges = 32)
        {
            wh *= 0.5f;
            drawer.FillOval(xy.X + wh.X, xy.Y + wh.Y, 0, wh.X, wh.Y, parameters, edges);
        }

        public static void FillOval<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector3 xyz, Vector2 wh, TVertexParameters parameters, int edges = 32)
        {
            wh *= 0.5f;
            drawer.FillOval(xyz.X + wh.X, xyz.Y + wh.Y, xyz.Z, wh.X, wh.Y, parameters, edges);
        }

        public static void FillOval<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            float x, float y, float w, float h, TVertexParameters parameters, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            drawer.FillOval(x + w, y + h, 0, w, h, parameters, edges);
        }

        public static void FillOval<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            float x, float y, float z, float w, float h, TVertexParameters parameters, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            drawer.FillOval(x + w, y + h, z, w, h, parameters, edges);
        }

        public static void DrawOval<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector2 xy, Vector2 wh, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            wh *= 0.5f;
            drawer.DrawOval(xy.X + wh.X, xy.Y + wh.Y, 0, wh.X, wh.Y, lineWidth, parameters, edges);
        }

        public static void DrawOval<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector3 xyz, Vector2 wh, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            wh *= 0.5f;
            drawer.DrawOval(xyz.X + wh.X, xyz.Y + wh.Y, xyz.Z, wh.X, wh.Y, lineWidth, parameters, edges);
        }

        public static void DrawOval<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            float x, float y, float w, float h, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            drawer.DrawOval(x + w, y + h, 0, w, h, lineWidth, parameters, edges);
        }

        public static void DrawOval<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            float x, float y, float z, float w, float h, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            drawer.DrawOval(x + w, y + h, z, w, h, lineWidth, parameters, edges);
        }

        public static void DrawLine<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector2 xy1, Vector2 xy2, float lineWidth, TVertexParameters parameters)
        {
            drawer.DrawLine(xy1.X, xy1.Y, 0, xy2.X, xy2.Y, 0, lineWidth, parameters);
        }

        public static void DrawLine<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            Vector3 xyz1, Vector3 xyz2, float lineWidth, TVertexParameters parameters)
        {
            drawer.DrawLine(xyz1.X, xyz1.Y, xyz1.Z, xyz2.X, xyz2.Y, xyz2.Z, lineWidth, parameters);
        }

        public static void DrawLine<TVertexParameters>(this IShapeDrawer2<TVertexParameters> drawer,
            float x1, float y1, float x2, float y2, float lineWidth, TVertexParameters parameters)
        {
            drawer.DrawLine(x1, y1, 0, x2, y2, 0, lineWidth, parameters);
        }
    }
}
