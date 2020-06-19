using System;
using OpenToolkit.Mathematics;

namespace amulware.Graphics
{
    /// <summary>
    /// Geometry that draws textured quads or sprites in two dimensional space
    /// </summary>
    public class Sprite2DGeometry : UVQuadGeometry<UVColorVertexData>
    {
        /// <summary>
        /// The color to draw with
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite2DGeometry"/> class.
        /// </summary>
        /// <param name="surface">The surface used for drawing</param>
        public Sprite2DGeometry(IndexedSurface<UVColorVertexData> surface)
            : base(surface)
        {
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        /// <param name="angle">The angle to rotate the sprite by, around the z axis, in radians.</param>
        /// <param name="scale">An additional scalar to scale the sprite by, relative to <see cref="Size"/>.</param>
        public override void DrawSprite(Vector3 position, float angle, float scale)
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
                topLeft = rotation.Times(topLeft);
                topRight = rotation.Times(topRight);
                bottomLeft = rotation.Times(bottomLeft);
                bottomRight = rotation.Times(bottomRight);
            }
            this.Surface.AddQuad(
                new UVColorVertexData(position.X + topLeft.X, position.Y + topLeft.Y, position.Z, this.UV.TopLeft, this.Color),
                new UVColorVertexData(position.X + topRight.X, position.Y + topRight.Y, position.Z, this.UV.TopRight, this.Color),
                new UVColorVertexData(position.X + bottomRight.X, position.Y + bottomRight.Y, position.Z, this.UV.BottomRight, this.Color),
                new UVColorVertexData(position.X + bottomLeft.X, position.Y + bottomLeft.Y, position.Z, this.UV.BottomLeft, this.Color)
                );
        }

        /// <summary>
        /// Draws a uv mapped(textured) rectangle.
        /// </summary>
        /// <param name="x">The x coordinate of the rectangle's corner.</param>
        /// <param name="y">The y coordinate of the rectangle's corner.</param>
        /// <param name="z">The z coordinate of the rectangle's corner..</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        public override void DrawRectangle(float x, float y, float z, float w, float h)
        {
            this.Surface.AddQuad(
                new UVColorVertexData(x, y, z, this.UV.TopLeft, this.Color),
                new UVColorVertexData(x + w, y, z, this.UV.TopRight, this.Color),
                new UVColorVertexData(x + w, y + h, z, this.UV.BottomRight, this.Color),
                new UVColorVertexData(x, y + h, z, this.UV.BottomLeft, this.Color)
                );
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
        public override void DrawLine(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float vx = x2 - x1;
            float vy = y1 - y2; // switch order for correct normal direction
            float ilxy = this.LineWidth / (float)Math.Sqrt(vx * vx + vy * vy);
            float nx = vy * ilxy;
            float ny = vx * ilxy;

            this.Surface.AddQuad(
                new UVColorVertexData(x1 + nx, y1 + ny, z1, this.UV.BottomLeft, this.Color),
                new UVColorVertexData(x1 - nx, y1 - ny, z1, this.UV.TopLeft, this.Color),
                new UVColorVertexData(x2 - nx, y2 - ny, z2, this.UV.TopRight, this.Color),
                new UVColorVertexData(x2 + nx, y2 + ny, z2, this.UV.BottomRight, this.Color)
                );
        }
    }
}
