using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class Sprite3DGeometry : Geometry<SimpleSpriteVertexData>
    {
        public Color Color = Color.White;

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

        public UVRectangle UV = UVRectangle.Default;

        public Sprite3DGeometry(QuadSurface<SimpleSpriteVertexData> surface)
            : base(surface)
        {
        }

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
