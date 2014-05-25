using OpenTK;

namespace amulware.Graphics.Charts
{
    public sealed class Chart2DSpriteContainer
    {
        private readonly Sprite2DGeometry line;
        private readonly Sprite2DGeometry point;
        private readonly FontGeometry font;

        public Chart2DSpriteContainer(SpriteSet<UVColorVertexData> sprites, FontGeometry fontGeo)
        {
            this.line = (Sprite2DGeometry)sprites["line"].Geometry;
            this.point = (Sprite2DGeometry)sprites["point"].Geometry;

            this.font = fontGeo;
            this.Color = Color.DeepPink;
        }

        public Color Color { get; set; }

        public void DrawLine(Vector2 from, Vector2 to, float thickness = 1)
        {
            this.line.Color = this.Color;
            this.line.LineWidth = thickness;
            this.line.DrawLine(from, to);
        }

        public void DrawPoint(Vector2 position, float size = 1)
        {
            this.point.Color = this.Color;
            this.point.DrawSprite(position, 0, size);
        }

        public void DrawText(Vector2 position, string text,
            float fontHeight = 1, float xAlign = 0, float yAlign = 0)
        {
            this.font.Height = fontHeight;
            this.font.Color = this.Color;
            this.font.DrawString(position, text, xAlign, yAlign);
        }
    }
}
