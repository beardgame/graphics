using System;
using System.Runtime.CompilerServices;
using amulware.Graphics.MeshBuilders;
using amulware.Graphics.Rendering;
using amulware.Graphics.Vertices;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Text
{
    public sealed class TextDrawer<TVertex> : ITextDrawer, IDisposable
        where TVertex : struct, IVertexData
    {
        public delegate TVertex CreateTextVertex(Vector3 xyz, Vector2 uv);

        private readonly Font font;
        private readonly CreateTextVertex createTextVertex;
        private readonly ExpandingIndexedTrianglesMeshBuilder<TVertex> meshBuilder;

        public TextDrawer(Font font, CreateTextVertex createTextVertex)
        {
            this.font = font;
            this.createTextVertex = createTextVertex;
            meshBuilder = new ExpandingIndexedTrianglesMeshBuilder<TVertex>();
        }

        public void DrawLine(Vector3 xyz, string text, float fontHeight, float alignHorizontal, float alignVertical,
            Vector3 unitRightDp, Vector3 unitDownDp)
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

                vertices[vI] = createTextVertex(characterTopLeft, uv0);
                vertices[vI + 1] = createTextVertex(characterTopLeft + stepRight, new Vector2(uv1.X, uv0.Y));
                vertices[vI + 2] = createTextVertex(characterTopLeft + stepDown, new Vector2(uv0.X, uv1.Y));
                vertices[vI + 3] = createTextVertex(characterTopLeft + stepRight + stepDown, uv1);

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector3 transform(Vector2 v, Vector3 unitX, Vector3 unitY)
        {
            return v.X * unitX + v.Y * unitY;
        }

        public void Clear()
        {
            meshBuilder.Clear();
        }

        public IBatchedRenderable ToRenderable()
        {
            return meshBuilder.ToRenderable();
        }

        public void Dispose()
        {
            meshBuilder.Dispose();
        }
    }
}
