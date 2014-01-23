using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// Geometry for drawing camera space aligned sprites in three dimensional space.
    /// </summary>
    public class Sprite3DGeometry : UVQuadGeometry<SimpleSpriteVertexData>
    {
        /// <summary>
        /// The color used for drawing
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite3DGeometry"/> class.
        /// </summary>
        /// <param name="surface">The surface two draw with.</param>
        public Sprite3DGeometry(IndexedSurface<SimpleSpriteVertexData> surface)
            : base(surface)
        {
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        public void DrawSprite(Vector3 position)
        {
            this.Surface.AddQuad(
                new SimpleSpriteVertexData(position, this.UV.TopLeft, this.Color, -this.expandX, this.expandY),
                new SimpleSpriteVertexData(position, this.UV.TopRight, this.Color, this.expandX, this.expandY),
                new SimpleSpriteVertexData(position, this.UV.BottomRight, this.Color, this.expandX, -this.expandY),
                new SimpleSpriteVertexData(position, this.UV.BottomLeft, this.Color, -this.expandX, -this.expandY)
                );
        }

        public override void DrawSprite(Vector3 position, float angle, float scale)
        {
            Vector2 expand1 = new Vector2(expandX, expandY) * scale;
            Vector2 expand2 = new Vector2(-expand1.X, expand1.Y);

            if (angle != 0)
            {
                Matrix2 rotation = Matrix2.CreateRotation(angle);
                expand1 = rotation * expand1;
                expand2 = rotation * expand2;
            }

            this.Surface.AddQuad(
                new SimpleSpriteVertexData(position, this.UV.TopLeft, this.Color, expand2),
                new SimpleSpriteVertexData(position, this.UV.TopRight, this.Color, expand1),
                new SimpleSpriteVertexData(position, this.UV.BottomRight, this.Color, -expand2),
                new SimpleSpriteVertexData(position, this.UV.BottomLeft, this.Color, -expand1)
                );
        }

        public override void DrawRectangle(float x, float y, float z, float w, float h)
        {
            throw new System.NotImplementedException();
        }

        public override void DrawLine(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            throw new System.NotImplementedException();
        }
    }
}
