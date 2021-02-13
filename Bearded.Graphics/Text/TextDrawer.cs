using System.Runtime.CompilerServices;
using Bearded.Graphics.MeshBuilders;
using Bearded.Graphics.Vertices;
using OpenTK.Mathematics;

namespace Bearded.Graphics.Text
{
    public sealed class TextDrawer<TVertex, TVertexParameters> : ITextDrawer<TVertexParameters>
        where TVertex : struct, IVertexData
    {
        public delegate TVertex CreateTextVertex(Vector3 xyz, Vector2 uv, TVertexParameters parameters);

        private readonly Font font;
        private readonly IIndexedTrianglesMeshBuilder<TVertex, ushort> meshBuilder;
        private readonly CreateTextVertex createTextVertex;

        public TextDrawer(Font font, IIndexedTrianglesMeshBuilder<TVertex, ushort> meshBuilder, CreateTextVertex createTextVertex)
        {
            this.font = font;
            this.meshBuilder = meshBuilder;
            this.createTextVertex = createTextVertex;
        }

        // TODO: this does a lot of linear algebra, should we have a version that draws axis aligned for most use cases?
        public void DrawLine(Vector3 xyz, string text, float fontHeight, float alignHorizontal, float alignVertical,
            Vector3 unitRightDp, Vector3 unitDownDp, TVertexParameters parameters)
        {
            var fontHeightAdjustedUnitX = unitRightDp * fontHeight;
            var fontHeightAdjustedUnitY = unitDownDp * fontHeight;

            var alignOffset = new Vector2(
                alignHorizontal == 0 ? 0 : alignHorizontal * -font.StringWidth(text),
                -alignVertical
            );

            var currentTopLeft = xyz + transform(alignOffset, fontHeightAdjustedUnitX, fontHeightAdjustedUnitY);

            meshBuilder.Add(text.Length * 4, text.Length * 6, out var vertices, out var indices, out var indexOffset);

            var vI = 0;
            var iI = 0;

            foreach (var character in text)
            {
                var charInfo = font.GetCharacterInfoFor(character);

                var characterTopLeft = currentTopLeft + transform(charInfo.Offset, fontHeightAdjustedUnitX, fontHeightAdjustedUnitY);
                var stepRight = charInfo.Size.X * fontHeightAdjustedUnitX;
                var stepDown = charInfo.Size.Y * fontHeightAdjustedUnitY;

                var uv0 = charInfo.TopLeftUV;
                var uv1 = charInfo.BottomRightUV;

                vertices[vI] = createTextVertex(characterTopLeft, uv0, parameters);
                vertices[vI + 1] = createTextVertex(characterTopLeft + stepRight, new Vector2(uv1.X, uv0.Y), parameters);
                vertices[vI + 2] = createTextVertex(characterTopLeft + stepDown, new Vector2(uv0.X, uv1.Y), parameters);
                vertices[vI + 3] = createTextVertex(characterTopLeft + stepRight + stepDown, uv1, parameters);

                indices[iI] = (ushort)(indexOffset + vI);
                indices[iI + 1] = (ushort)(indexOffset + vI + 1);
                indices[iI + 2] = (ushort)(indexOffset + vI + 2);
                indices[iI + 3] = (ushort)(indexOffset + vI + 1);
                indices[iI + 4] = (ushort)(indexOffset + vI + 3);
                indices[iI + 5] = (ushort)(indexOffset + vI + 2);

                currentTopLeft += charInfo.SpacingWidth * fontHeightAdjustedUnitX;
                vI += 4;
                iI += 6;
            }
        }

        public (Vector3 Width, Vector3 Height) StringSize(
            string text, float fontHeight, Vector3 unitRightDP, Vector3 unitDownDP)
        {
            return (StringWidth(text, fontHeight, unitRightDP), StringHeight(fontHeight, unitDownDP));
        }

        public Vector3 StringWidth(string text, float fontHeight, Vector3 unitRightDP)
        {
            return font.StringWidth(text) * fontHeight * unitRightDP;
        }

        public Vector3 StringHeight(float fontHeight, Vector3 unitDownDP)
        {
            return fontHeight * unitDownDP;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector3 transform(Vector2 v, Vector3 unitX, Vector3 unitY)
        {
            return v.X * unitX + v.Y * unitY;
        }
    }
}
