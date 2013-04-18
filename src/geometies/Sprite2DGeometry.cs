using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace AWGraphics
{
    public class Sprite2DGeometry : Geometry<UVColorVertexData>
    {
        public Color Color = Color.White;
        public float LineWidth = 1;

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

        public Sprite2DGeometry(QuadSurface<UVColorVertexData> surface)
            : base(surface)
        {
        }

        #region DrawSprite

        public void DrawSprite(Vector2 position)
        {
            this.DrawSprite(new Vector3(position.X, position.Y, 0));
        }

        public void DrawSprite(Vector2 position, float z)
        {
            this.DrawSprite(new Vector3(position.X, position.Y, z));
        }

        public void DrawSprite(Vector3 position)
        {
            float top = position.Y - this.expandY;
            float bottom = position.Y + this.expandY;
            float left = position.X - this.expandX;
            float right = position.X + this.expandX;
            this.Surface.AddVertices(new UVColorVertexData[] {
                new UVColorVertexData(left, top, position.Z, this.UV.TopLeft, this.Color),
                new UVColorVertexData(right, top, position.Z, this.UV.TopRight, this.Color),
                new UVColorVertexData(right, bottom, position.Z, this.UV.BottomRight, this.Color),
                new UVColorVertexData(left, bottom, position.Z, this.UV.BottomLeft, this.Color)
                });
        }

        #endregion

        #region DrawRectangle

        public void DrawRectangle(Vector3 position)
        {
            this.DrawRectangle(position.X, position.Y, position.Z, this.size.X, this.size.Y);
        }

        public void DrawRectangle(Vector3 position, Vector2 size)
        {
            this.DrawRectangle(position.X, position.Y, position.Z, size.X, size.Y);
        }

        public void DrawRectangle(Vector2 position)
        {
            this.DrawRectangle(position.X, position.Y, 0, this.size.X, this.size.Y);
        }

        public void DrawRectangle(Vector2 position, Vector2 size)
        {
            this.DrawRectangle(position.X, position.Y, 0, size.X, size.Y);
        }

        public void DrawRectangle(Vector2 position, float z, Vector2 size)
        {
            this.DrawRectangle(position.X, position.Y, z, size.X, size.Y);
        }

        public void DrawRectangle(float x, float y)
        {
            this.DrawRectangle(x, y, 0, this.size.X, this.size.Y);
        }

        public void DrawRectangle(float x, float y, float w, float h)
        {
            this.DrawRectangle(x, y, 0, w, h);
        }

        public void DrawRectangle(float x, float y, float z, float w, float h)
        {
            this.Surface.AddVertices(new UVColorVertexData[] {
                new UVColorVertexData(x, y, z, this.UV.TopLeft, this.Color),
                new UVColorVertexData(x + w, y, z, this.UV.TopRight, this.Color),
                new UVColorVertexData(x + w, y + h, z, this.UV.BottomRight, this.Color),
                new UVColorVertexData(x, y + h, z, this.UV.BottomLeft, this.Color)
                });
        }

        #endregion

        #region DrawLine

        public void DrawLine(Vector2 xy1, Vector2 xy2)
        {
            this.DrawLine(xy1.X, xy1.Y, 0, xy2.X, xy2.Y, 0);
        }

        public void DrawLine(Vector3 xyz1, Vector3 xyz2)
        {
            this.DrawLine(xyz1.X, xyz1.Y, xyz1.Z, xyz2.X, xyz2.Y, xyz2.Z);
        }

        public void DrawLine(float x1, float y1, float x2, float y2)
        {
            this.DrawLine(x1, y1, 0, x2, y2, 0);
        }

        public void DrawLine(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float vx = x2 - x1;
            float vy = y1 - y2; // switch order for correct normal direction
            float ilxy = this.LineWidth / (float)Math.Sqrt(vx * vx + vy * vy);
            float nx = vy * ilxy;
            float ny = vx * ilxy;

            this.Surface.AddVertices(new UVColorVertexData[] {
                new UVColorVertexData(x1 + nx, y1 + ny, z1, this.UV.BottomLeft, this.Color),
                new UVColorVertexData(x1 - nx, y1 - ny, z1, this.UV.TopLeft, this.Color),
                new UVColorVertexData(x2 - nx, y2 - ny, z2, this.UV.TopRight, this.Color),
                new UVColorVertexData(x2 + nx, y2 + ny, z2, this.UV.BottomRight, this.Color)
                });
        }

        #endregion
    }
}
