namespace Bearded.Graphics.Shapes
{
    public interface IShapeDrawer2<in TVertexParameters>
    {
        void FillRectangle(float x, float y, float z, float w, float h, TVertexParameters parameters);

        void DrawRectangle(float x, float y, float z, float w, float h, float lineWidth, TVertexParameters parameters);

        void FillOval(float centerX, float centerY, float centerZ, float radiusX, float radiusY,
            TVertexParameters parameters, int edges);

        void DrawOval(float centerX, float centerY, float centerZ, float radiusX, float radiusY,
            float lineWidth, TVertexParameters parameters, int edges);

        void DrawLine(float x1, float y1, float z1, float x2, float y2, float z2, float lineWidth,
            TVertexParameters parameters);
    }
}
