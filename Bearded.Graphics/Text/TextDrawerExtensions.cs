using OpenTK.Mathematics;

namespace Bearded.Graphics.Text
{
    public static class TextDrawerExtensions
    {
        public static TextDrawerWithDefaults<TVertexParameters> WithDefaults<TVertexParameters>(
            this ITextDrawer<TVertexParameters> textDrawer, float fontHeight, float alignHorizontal, float alignVertical,
            Vector3 unitRightDp, Vector3 unitDownDp, TVertexParameters parameters)
        {
            return new TextDrawerWithDefaults<TVertexParameters>(
                textDrawer, fontHeight, alignHorizontal, alignVertical, unitRightDp, unitDownDp, parameters);
        }
    }
}
