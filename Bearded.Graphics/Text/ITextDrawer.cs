using Vector3 = OpenTK.Mathematics.Vector3;

namespace Bearded.Graphics.Text
{
    public interface ITextDrawer<in TVertexParameters>
    {
        void DrawLine(Vector3 xyz, string text, float fontHeight, float alignHorizontal, float alignVertical,
            Vector3 unitRightDP, Vector3 unitDownDP, TVertexParameters parameters);

        Vector3 StringWidth(string text, float fontHeight, Vector3 unitRightDP);
        Vector3 StringHeight(float fontHeight, Vector3 unitDownDP);
        (Vector3 Width, Vector3 Height) StringSize(string text, float fontHeight, Vector3 unitRightDP, Vector3 unitDownDP);
    }
}
