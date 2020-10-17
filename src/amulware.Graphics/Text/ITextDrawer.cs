using OpenToolkit.Mathematics;

namespace amulware.Graphics.Text
{
    public interface ITextDrawer<in TVertexParameters>
    {
        void DrawLine(Vector3 xyz, string text, float fontHeight, float alignHorizontal, float alignVertical,
            Vector3 unitRightDP, Vector3 unitDownDP, TVertexParameters parameters);
    }
}
