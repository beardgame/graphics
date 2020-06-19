using OpenToolkit.Mathematics;

namespace amulware.Graphics.Charts
{
    public sealed class Chart2DSpriteContainer
    {
        private readonly Sprite2DGeometry line;
        private readonly Sprite2DGeometry point;
        private readonly FontGeometry font;
        private readonly IndexedSurface<UVColorVertexData> surface;
        private readonly UVRectangle quadUV;

        private readonly VertexSurface<FastChart2DBarVertex> fastBarSurface;

        public float ThinLineWidth { get; set; }
        public float ThickLineWidth { get; set; }

        public float SmallPointSize { get; set; }
        public float LargePointSize { get; set; }

        public Color Color { get; set; }

        public Chart2DSpriteContainer(SpriteSet<UVColorVertexData> sprites, FontGeometry fontGeo,
            VertexSurface<FastChart2DBarVertex> fastBarSurface = null)
        {
            this.line = (Sprite2DGeometry)sprites["line"].Geometry;
            this.point = (Sprite2DGeometry)sprites["point"].Geometry;

            this.surface = sprites.Surface;

            this.quadUV = sprites["quad"].Geometry.UV;

            this.font = fontGeo;

            this.ThinLineWidth = 0.015f;
            this.ThickLineWidth = 0.03f;
            this.SmallPointSize = 0.03f;
            this.LargePointSize = 0.1f;
            this.Color = Color.DeepPink;

            if (fastBarSurface != null)
            {
                this.fastBarSurface = fastBarSurface;
                this.fastBarSurface.AddSettings(sprites.Surface.Settings);
                this.fastBarSurface.AddSettings(
                    new Vector2Uniform("uv01", this.quadUV.TopLeft),
                    new Vector2Uniform("uv11", this.quadUV.TopRight),
                    new Vector2Uniform("uv00", this.quadUV.BottomLeft),
                    new Vector2Uniform("uv10", this.quadUV.BottomRight)
                    );
            }
        }

        public bool CanDrawBarsFast { get { return this.fastBarSurface != null; } }

        public FastChart2DBarVertex[] DrawFastBars(int count, out ushort offset)
        {
            return this.fastBarSurface.WriteVerticesDirectly(count, out offset);
        }

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

        public void DrawQuad(Vector2 v0, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            this.surface.AddQuad(
                new UVColorVertexData(v0.X, v0.Y, 0, this.quadUV.BottomLeft, this.Color),
                new UVColorVertexData(v1.X, v1.Y, 0, this.quadUV.TopLeft, this.Color),
                new UVColorVertexData(v2.X, v2.Y, 0, this.quadUV.TopRight, this.Color),
                new UVColorVertexData(v3.X, v3.Y, 0, this.quadUV.BottomRight, this.Color)
                );
        }
    }
}
