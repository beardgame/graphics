using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    /// <summary>
    /// Geometry for drawing camera space aligned sprites in three dimensional space.
    /// </summary>
    public class Sprite3DGeometry : Geometry<SimpleSpriteVertexData>
    {
        /// <summary>
        /// The color used for drawing
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// The size to draw the sprite with
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.expandX = value.X * 0.5f;
                this.expandY = value.Y * 0.5f;
                this.size = value;
            }
        }

        private Vector2 size = Vector2.One;
        private float expandX = 0.5f;
        private float expandY = 0.5f;

        /// <summary>
        /// The <see cref="UVRectangle"/> used to map the sprite with. Obtain from <see cref="Texture.GrabUV"/>
        /// </summary>
        public UVRectangle UV = UVRectangle.Default;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite3DGeometry"/> class.
        /// </summary>
        /// <param name="surface">The surface two draw with.</param>
        public Sprite3DGeometry(QuadSurface<SimpleSpriteVertexData> surface)
            : base(surface)
        {
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        public void DrawSprite(Vector3 position)
        {
            this.Surface.AddVertices(new SimpleSpriteVertexData[] {
                new SimpleSpriteVertexData(position, this.UV.TopLeft, this.Color, -this.expandX, this.expandY),
                new SimpleSpriteVertexData(position, this.UV.TopRight, this.Color, this.expandX, this.expandY),
                new SimpleSpriteVertexData(position, this.UV.BottomRight, this.Color, this.expandX, -this.expandY),
                new SimpleSpriteVertexData(position, this.UV.BottomLeft, this.Color, -this.expandX, -this.expandY)
                });
        }
    }
}
