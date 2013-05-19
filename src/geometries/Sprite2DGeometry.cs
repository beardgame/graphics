using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace amulware.Graphics
{
    /// <summary>
    /// Geometry that draws textured quads or sprites in two dimensional space
    /// </summary>
    public class Sprite2DGeometry : Geometry<UVColorVertexData>
    {
        /// <summary>
        /// The color to draw with
        /// </summary>
        public Color Color = Color.White;
        /// <summary>
        /// The line width used when drawing lines
        /// </summary>
        public float LineWidth = 1;

        /// <summary>
        /// The size of the sprite to draw
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
        /// Initializes a new instance of the <see cref="Sprite2DGeometry"/> class.
        /// </summary>
        /// <param name="surface">The surface used for drawing</param>
        public Sprite2DGeometry(QuadSurface<UVColorVertexData> surface)
            : base(surface)
        {
        }

        #region DrawSprite /// @name DrawSprite

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        public void DrawSprite(Vector2 position)
        {
            this.DrawSprite(new Vector3(position.X, position.Y, 0), 0, 1);
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        /// <param name="angle">The angle to rotate the sprite by, around the z axis, in radians.</param>
        public void DrawSprite(Vector2 position, float angle)
        {
            this.DrawSprite(new Vector3(position.X, position.Y, 0), angle, 1);
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        /// <param name="angle">The angle to rotate the sprite by, around the z axis, in radians.</param>
        /// <param name="scale">An additional scalar to scale the sprite by, relative to <see cref="Size"/>.</param>
        public void DrawSprite(Vector2 position, float angle, float scale)
        {
            this.DrawSprite(new Vector3(position.X, position.Y, 0), angle, scale);
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        public void DrawSprite(Vector3 position)
        {
            this.DrawSprite(position, 0, 1);
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        /// <param name="angle">The angle to rotate the sprite by, around the z axis, in radians.</param>
        public void DrawSprite(Vector3 position, float angle)
        {
            this.DrawSprite(position, angle, 1);
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        /// <param name="angle">The angle to rotate the sprite by, around the z axis, in radians.</param>
        /// <param name="scale">An additional scalar to scale the sprite by, relative to <see cref="Size"/>.</param>
        public void DrawSprite(Vector3 position, float angle, float scale)
        {
            float x = this.expandX * scale;
            float y = this.expandY * scale;
            Vector2 topLeft = new Vector2(-x, -y);
            Vector2 topRight = new Vector2(x, -y);
            Vector2 bottomLeft = new Vector2(-x, y);
            Vector2 bottomRight = new Vector2(x, y);
            if (angle != 0)
            {
                Matrix2 rotation = Matrix2.CreateRotation(angle);
                topLeft = rotation * topLeft;
                topRight = rotation * topRight;
                bottomLeft = rotation * bottomLeft;
                bottomRight = rotation * bottomRight;
            }
            this.Surface.AddVertices(new UVColorVertexData[] {
                new UVColorVertexData(position.X + topLeft.X, position.Y + topLeft.Y, position.Z, this.UV.TopLeft, this.Color),
                new UVColorVertexData(position.X + topRight.X, position.Y + topRight.Y, position.Z, this.UV.TopRight, this.Color),
                new UVColorVertexData(position.X + bottomRight.X, position.Y + bottomRight.Y, position.Z, this.UV.BottomRight, this.Color),
                new UVColorVertexData(position.X + bottomLeft.X, position.Y + bottomLeft.Y, position.Z, this.UV.BottomLeft, this.Color),
                });
        }

        #endregion

        #region DrawRectangle /// @name DrawRectangle

        /// <summary>
        /// Draws a uv mapped(textured) rectangle.
        /// </summary>
        /// <param name="position">The coordinates of the rectangle's corner.</param>
        public void DrawRectangle(Vector3 position)
        {
            this.DrawRectangle(position.X, position.Y, position.Z, this.size.X, this.size.Y);
        }

        /// <summary>
        /// Draws a uv mapped(textured) rectangle.
        /// </summary>
        /// <param name="position">The coordinates of the rectangle's corner.</param>
        /// <param name="size">The size of the rectangle.</param>
        public void DrawRectangle(Vector3 position, Vector2 size)
        {
            this.DrawRectangle(position.X, position.Y, position.Z, size.X, size.Y);
        }

        /// <summary>
        /// Draws a uv mapped(textured) rectangle.
        /// </summary>
        /// <param name="position">The coordinates of the rectangle's corner.</param>
        public void DrawRectangle(Vector2 position)
        {
            this.DrawRectangle(position.X, position.Y, 0, this.size.X, this.size.Y);
        }

        /// <summary>
        /// Draws a uv mapped(textured) rectangle.
        /// </summary>
        /// <param name="position">The coordinates of the rectangle's corner.</param>
        /// <param name="size">The size of the rectangle.</param>
        public void DrawRectangle(Vector2 position, Vector2 size)
        {
            this.DrawRectangle(position.X, position.Y, 0, size.X, size.Y);
        }

        /// <summary>
        /// Draws a uv mapped(textured) rectangle.
        /// </summary>
        /// <param name="x">The x coordinate of the rectangle's corner.</param>
        /// <param name="y">The y coordinate of the rectangle's corner.</param>
        public void DrawRectangle(float x, float y)
        {
            this.DrawRectangle(x, y, 0, this.size.X, this.size.Y);
        }

        /// <summary>
        /// Draws a uv mapped(textured) rectangle.
        /// </summary>
        /// <param name="x">The x coordinate of the rectangle's corner.</param>
        /// <param name="y">The y coordinate of the rectangle's corner.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        public void DrawRectangle(float x, float y, float w, float h)
        {
            this.DrawRectangle(x, y, 0, w, h);
        }

        /// <summary>
        /// Draws a uv mapped(textured) rectangle.
        /// </summary>
        /// <param name="x">The x coordinate of the rectangle's corner.</param>
        /// <param name="y">The y coordinate of the rectangle's corner.</param>
        /// <param name="z">The z coordinate of the rectangle's corner..</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
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

        #region DrawLine /// @name DrawLine

        /// <summary>
        /// Draws a uv mapped(textured) line between two points.
        /// </summary>
        /// <param name="xy1">The coordinates of the first point.</param>
        /// <param name="xy2">The coordinates of the second point.</param>
        public void DrawLine(Vector2 xy1, Vector2 xy2)
        {
            this.DrawLine(xy1.X, xy1.Y, 0, xy2.X, xy2.Y, 0);
        }

        /// <summary>
        /// Draws a uv mapped(textured) line between two points.
        /// </summary>
        /// <param name="xyz1">The coordinates of the first point.</param>
        /// <param name="xyz2">The coordinates of the second point.</param>
        public void DrawLine(Vector3 xyz1, Vector3 xyz2)
        {
            this.DrawLine(xyz1.X, xyz1.Y, xyz1.Z, xyz2.X, xyz2.Y, xyz2.Z);
        }

        /// <summary>
        /// Draws a uv mapped(textured) line between two points.
        /// </summary>
        /// <param name="x1">The x coordinate of the first point.</param>
        /// <param name="y1">The y coordinate of the first point.</param>
        /// <param name="x2">The x coordinate of the second point.</param>
        /// <param name="y2">The y coordinate of the second point.</param>
        public void DrawLine(float x1, float y1, float x2, float y2)
        {
            this.DrawLine(x1, y1, 0, x2, y2, 0);
        }

        /// <summary>
        /// Draws a uv mapped(textured) line between two points.
        /// </summary>
        /// <param name="x1">The x coordinate of the first point.</param>
        /// <param name="y1">The y coordinate of the first point.</param>
        /// <param name="z1">The z coordinate of the first point.</param>
        /// <param name="x2">The x coordinate of the second point.</param>
        /// <param name="y2">The y coordinate of the second point.</param>
        /// <param name="z2">The z coordinate of the second point.</param>
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
