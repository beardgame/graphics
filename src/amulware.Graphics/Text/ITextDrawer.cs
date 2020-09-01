using OpenToolkit.Mathematics;

namespace amulware.Graphics.Text
{
    public interface ITextDrawer
    {
        void DrawLine(Vector3 xyz, string text, float fontHeight, float alignHorizontal, float alignVertical, Vector3 unitRightDP, Vector3 unitDownDP);
    }
}
