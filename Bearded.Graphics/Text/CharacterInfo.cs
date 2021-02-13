using OpenTK.Mathematics;

namespace Bearded.Graphics.Text
{
    public readonly struct CharacterInfo
    {
        public Vector2 Size { get; }
        public Vector2 Offset { get; }

        public float SpacingWidth { get; }

        public Vector2 TopLeftUV { get; }
        public Vector2 BottomRightUV { get; }

        public CharacterInfo(Vector2 size, Vector2 offset, float spacingWidth, Vector2 topLeftUV, Vector2 bottomRightUV)
        {
            Size = size;
            Offset = offset;
            SpacingWidth = spacingWidth;
            TopLeftUV = topLeftUV;
            BottomRightUV = bottomRightUV;
        }
    }
}
