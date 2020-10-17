using OpenToolkit.Mathematics;

namespace amulware.Graphics.Text
{
    public interface ITextDrawer<TVertexParameters>
    {
        void DrawLine(Vector3 xyz, string text, float fontHeight, float alignHorizontal, float alignVertical,
            Vector3 unitRightDP, Vector3 unitDownDP, TVertexParameters parameters);

        TextDrawerWithDefaults<TVertexParameters> WithDefaults(float fontHeight, float alignHorizontal, float alignVertical,
            Vector3 unitRightDp, Vector3 unitDownDp, TVertexParameters parameters)
        {
            return new TextDrawerWithDefaults<TVertexParameters>(
                this, fontHeight, alignHorizontal, alignVertical, unitRightDp, unitDownDp, parameters);
        }
    }
}
