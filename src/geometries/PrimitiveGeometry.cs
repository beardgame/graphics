using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{


    /// <summary>
    /// Geometry used for drawing of coloured primitives like rectangles and lines.
    /// </summary>
    public class PrimitiveGeometry : Geometry<PrimitiveVertexData>
    {

        /// <summary>
        /// The color used for drawing
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// The width of lines, and edges of non-solid primitives
        /// </summary>
        public float LineWidth = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveGeometry"/> class.
        /// </summary>
        /// <param name="surface">The surface used for drawing.</param>
        public PrimitiveGeometry(QuadSurface<PrimitiveVertexData> surface) : base(surface)
        {

        }

        #region DrawRectangle

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="xy">The coordinate of the rectangle's corner.</param>
        /// <param name="wh">The size of the rectangle.</param>
        public void DrawRectangle(Vector2 xy, Vector2 wh)
        {
            this.DrawRectangle(xy.X, xy.Y, 0, wh.X, wh.Y);
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="xyz">The coordinate of the rectangle's corner.</param>
        /// <param name="wh">The size of the rectangle.</param>
        public void DrawRectangle(Vector3 xyz, Vector2 wh)
        {
            this.DrawRectangle(xyz.X, xyz.Y, xyz.Z, wh.X, wh.Y);
        }

        /// <summary>
        /// Draws a rectangle.
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
        /// Draws a rectangle.
        /// </summary>
        /// <param name="x">The x coordinate of the rectangle's corner.</param>
        /// <param name="y">The y coordinate of the rectangle's corner.</param>
        /// <param name="z">The z coordinate of the rectangle's corner.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        public void DrawRectangle(float x, float y, float z, float w, float h)
        {
             this.Surface.AddVertices(new PrimitiveVertexData[] {
                new PrimitiveVertexData(x,      y,      z, this.Color),
                new PrimitiveVertexData(x + w,  y,      z, this.Color),
                new PrimitiveVertexData(x + w,  y + h,  z, this.Color),
                new PrimitiveVertexData(x,      y + h,  z, this.Color)
                });
        }

        #endregion


        #region DrawCircle

        /// <summary>
        /// Draws a circle
        /// </summary>
        /// <param name="x">X coordinate of circle's center</param>
        /// <param name="y">Y coordinate of circle's center</param>
        /// <param name="r">Radius of circle</param>
        /// <param name="filled">Whether circle is filled, if false draws only a border with width <see cref="LineWidth"/></param>
        /// <param name="edges">Number of edges used to draw the circle</param>
        public void DrawCircle(float x, float y, float r, bool filled = true, int edges = 32)
        {
            this.drawOval(x, y, 0, r, r, edges, filled);
        }

        /// <summary>
        /// Draws a circle
        /// </summary>
        /// <param name="xy">Center of circle</param>
        /// <param name="r">Radius of circle</param>
        /// <param name="filled">Whether circle is filled, if false draws only a border with width <see cref="LineWidth"/></param>
        /// <param name="edges">Number of edges used to draw the circle</param>
        public void DrawCircle(Vector2 xy, float r, bool filled = true, int edges = 32)
        {
            this.drawOval(xy.X, xy.Y, 0, r, r, edges, filled);
        }

        /// <summary>
        /// Draws a circle
        /// </summary>
        /// <param name="x">X coordinate of circle's center</param>
        /// <param name="y">Y coordinate of circle's center</param>
        /// <param name="z">Z coordinate of circle's center</param>
        /// <param name="r">Radius of circle</param>
        /// <param name="filled">Whether circle is filled, if false draws only a border with width <see cref="LineWidth"/></param>
        /// <param name="edges">Number of edges used to draw the circle</param>
        public void DrawCircle(float x, float y, float z, float r, bool filled = true, int edges = 32)
        {
            this.drawOval(x, y, z, r, r, edges, filled);
        }

        /// <summary>
        /// Draws a circle
        /// </summary>
        /// <param name="xyz">Center of circle</param>
        /// <param name="r">Radius of circle</param>
        /// <param name="filled">Whether circle is filled, if false draws only a border with width <see cref="LineWidth"/></param>
        /// <param name="edges">Number of edges used to draw the circle</param>
        public void DrawCircle(Vector3 xyz, float r, bool filled = true, int edges = 32)
        {
            this.drawOval(xyz.X, xyz.Y, xyz.Z, r, r, edges, filled);
        }

        #endregion

        #region DrawOval

        /// <summary>
        /// Draws an oval.
        /// </summary>
        /// <param name="xy">The coordinates of the oval's bounding rectangle's corner.</param>
        /// <param name="wh">The size of the oval's bounding rectangle.</param>
        /// <param name="filled">if set to <c>true</c> the oval is drawn solid, otherwise it is drawn with the specified line width.</param>
        /// <param name="edges">The number of edges to draw the oval width.</param>
        public void DrawOval(Vector2 xy, Vector2 wh, bool filled = true, int edges = 32)
        {
            wh *= 0.5f;
            this.drawOval(xy.X + wh.X, xy.Y + wh.Y, 0, wh.X, wh.Y, edges, filled);
        }

        /// <summary>
        /// Draws an oval.
        /// </summary>
        /// <param name="xyz">The coordinates of the oval's bounding rectangle's corner.</param>
        /// <param name="wh">The size of the oval's bounding rectangle.</param>
        /// <param name="filled">if set to <c>true</c> the oval is drawn solid, otherwise it is drawn with the specified line width.</param>
        /// <param name="edges">The number of edges to draw the oval width.</param>
        public void DrawOval(Vector3 xyz, Vector2 wh, bool filled = true, int edges = 32)
        {
            wh *= 0.5f;
            this.drawOval(xyz.X + wh.X, xyz.Y + wh.Y, xyz.Z, wh.X, wh.Y, edges, filled);
        }

        /// <summary>
        /// Draws an oval.
        /// </summary>
        /// <param name="x">The x coordinate of the oval's bounding rectangle's corner.</param>
        /// <param name="y">The y coordinate of the oval's bounding rectangle's corner.</param>
        /// <param name="w">The width of the oval's bounding rectangle.</param>
        /// <param name="h">The height of the oval's bounding rectangle.</param>
        /// <param name="filled">if set to <c>true</c> the oval is drawn solid, otherwise it is drawn with the specified line width.</param>
        /// <param name="edges">The number of edges to draw the oval width.</param>
        public void DrawOval(float x, float y, float w, float h, bool filled = true, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            this.drawOval(x + w, y + h, 0, w, h, edges, filled);
        }

        /// <summary>
        /// Draws an oval.
        /// </summary>
        /// <param name="x">The x coordinate of the oval's bounding rectangle's corner.</param>
        /// <param name="y">The y coordinate of the oval's bounding rectangle's corner.</param>
        /// <param name="z">The z coordinate of the oval's bounding rectangle's corner.</param>
        /// <param name="w">The width of the oval's bounding rectangle.</param>
        /// <param name="h">The height of the oval's bounding rectangle.</param>
        /// <param name="filled">if set to <c>true</c> the oval is drawn solid, otherwise it is drawn with the specified line width.</param>
        /// <param name="edges">The number of edges to draw the oval width.</param>
        public void DrawOval(float x, float y, float z, float w, float h, bool filled = true, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            this.drawOval(x + w, y + h, z, w, h, edges, filled);
        }

        #region Actual drawing of ovals and circles

        /// <summary>
        /// Draws an oval.
        /// </summary>
        /// <param name="centerX">The center X.</param>
        /// <param name="centerY">The center Y.</param>
        /// <param name="centerZ">The center Z.</param>
        /// <param name="halfWidth">Half of the width.</param>
        /// <param name="halfHeight">Half of the height.</param>
        /// <param name="edges">The number of edges to draw the oval width.</param>
        /// <param name="filled">if set to <c>true</c> the oval is drawn solid, otherwise it is drawn with the specified line width.</param>
        private void drawOval(float centerX, float centerY, float centerZ, float halfWidth, float halfHeight, int edges, bool filled)
        {
            if (filled || this.LineWidth >= halfWidth || this.LineWidth >= halfHeight)
                this.drawOvalFilled(centerX, centerY, centerZ, halfWidth, halfHeight, edges);
            else
                this.drawOvalUnfilled(centerX, centerY, centerZ, halfWidth, halfHeight, edges);
        }

        /// <summary>
        /// Draws a filled oval.
        /// </summary>
        /// <param name="centerX">The center X.</param>
        /// <param name="centerY">The center Y.</param>
        /// <param name="centerZ">The center Z.</param>
        /// <param name="halfWidth">Half of the width.</param>
        /// <param name="halfHeight">Half of the height.</param>
        /// <param name="edges">The number of edges to draw the oval width.</param>
        private void drawOvalFilled(float centerX, float centerY, float centerZ, float halfWidth, float halfHeight, int edges)
        {
            edges &= ~1;
            int parts = Math.Max((edges - 2) / 4, 1);
            int vertexCount = parts * 8;

            PrimitiveVertexData[] vertices = new PrimitiveVertexData[vertexCount];

            Matrix2 rotation = Matrix2.CreateRotation(MathHelper.Pi / (parts * 2 + 1));

            Vector2 lastN = new Vector2(-1, 0);

            Vector2 last = new Vector2(lastN.X * halfWidth, lastN.Y * halfHeight);

            Color argb = this.Color;


            for (int i = 0; i < parts; i++)
            {
                int j = i * 4;
                int k = parts * 4 + i * 4;

                lastN = rotation * lastN;

                Vector2 next = new Vector2(lastN.X * halfWidth, lastN.Y * halfHeight);

                vertices[j + 0] = new PrimitiveVertexData(centerX + last.X, centerY + last.Y, centerZ, argb);
                vertices[j + 1] = new PrimitiveVertexData(centerX + next.X, centerY + next.Y, centerZ, argb);
                vertices[j + 2] = new PrimitiveVertexData(centerX - next.X, centerY + next.Y, centerZ, argb);
                vertices[j + 3] = new PrimitiveVertexData(centerX - last.X, centerY + last.Y, centerZ, argb);

                vertices[k + 0] = new PrimitiveVertexData(centerX + last.X, centerY - last.Y, centerZ, argb);
                vertices[k + 1] = new PrimitiveVertexData(centerX - last.X, centerY - last.Y, centerZ, argb);
                vertices[k + 2] = new PrimitiveVertexData(centerX - next.X, centerY - next.Y, centerZ, argb);
                vertices[k + 3] = new PrimitiveVertexData(centerX + next.X, centerY - next.Y, centerZ, argb);

                last = next;
            }

            this.Surface.AddVertices(vertices);
        }

        /// <summary>
        /// Draws an unfilled oval.
        /// </summary>
        /// <param name="centerX">The center X.</param>
        /// <param name="centerY">The center Y.</param>
        /// <param name="centerZ">The center Z.</param>
        /// <param name="halfWidth">Half of the width.</param>
        /// <param name="halfHeight">Half of the height.</param>
        /// <param name="edges">The number of edges to draw the oval width.</param>
        private void drawOvalUnfilled(float centerX, float centerY, float centerZ, float halfWidth, float halfHeight, int edges)
        {
            int vertexCount = edges * 4;

            PrimitiveVertexData[] vertices = new PrimitiveVertexData[vertexCount];

            Matrix2 rotation = Matrix2.CreateRotation(MathHelper.TwoPi / edges);

            Vector2 lastN = new Vector2(-1, 0);

            float halfWInner = halfWidth - this.LineWidth;
            float halfHInner = halfHeight - this.LineWidth;

            Vector2 lastOuter = new Vector2(lastN.X * halfWidth, lastN.Y * halfHeight);
            Vector2 lastInner = new Vector2(lastN.X * halfWInner, lastN.Y * halfHInner);

            Color argb = this.Color;


            for (int i = 0; i < edges / 2; i++)
            {
                int j = i * 4;

                lastN = rotation * lastN;

                Vector2 nextOuter = new Vector2(lastN.X * halfWidth, lastN.Y * halfHeight);
                Vector2 nextInner = new Vector2(lastN.X * halfWInner, lastN.Y * halfHInner);

                vertices[j + 0] = new PrimitiveVertexData(centerX + lastOuter.X, centerY + lastOuter.Y, centerZ, argb);
                vertices[j + 1] = new PrimitiveVertexData(centerX + nextOuter.X, centerY + nextOuter.Y, centerZ, argb);
                vertices[j + 2] = new PrimitiveVertexData(centerX + nextInner.X, centerY + nextInner.Y, centerZ, argb);
                vertices[j + 3] = new PrimitiveVertexData(centerX + lastInner.X, centerY + lastInner.Y, centerZ, argb);

                vertices[vertexCount - j - 1] = new PrimitiveVertexData(centerX + lastOuter.X, centerY - lastOuter.Y, centerZ, argb);
                vertices[vertexCount - j - 2] = new PrimitiveVertexData(centerX + nextOuter.X, centerY - nextOuter.Y, centerZ, argb);
                vertices[vertexCount - j - 3] = new PrimitiveVertexData(centerX + nextInner.X, centerY - nextInner.Y, centerZ, argb);
                vertices[vertexCount - j - 4] = new PrimitiveVertexData(centerX + lastInner.X, centerY - lastInner.Y, centerZ, argb);

                lastOuter = nextOuter;
                lastInner = nextInner;
            }

            if ((edges & 1) != 0)
            {
                int j = (edges / 2) * 4;
                vertices[j + 0] = new PrimitiveVertexData(centerX + lastInner.X, centerY + lastInner.Y, centerZ, argb);
                vertices[j + 1] = new PrimitiveVertexData(centerX + lastOuter.X, centerY + lastOuter.Y, centerZ, argb);
                vertices[j + 2] = new PrimitiveVertexData(centerX + lastOuter.X, centerY - lastOuter.Y, centerZ, argb);
                vertices[j + 3] = new PrimitiveVertexData(centerX + lastInner.X, centerY - lastInner.Y, centerZ, argb);
            }

            this.Surface.AddVertices(vertices);
        }
        #endregion

        #endregion


        #region DrawLine

        /// <summary>
        /// Draws a line between two points width the set <see cref="LineWidth"/>.
        /// </summary>
        /// <param name="xy1">The first point.</param>
        /// <param name="xy2">The second point.</param>
        public void DrawLine(Vector2 xy1, Vector2 xy2)
        {
            this.DrawLine(xy1.X, xy1.Y, 0, xy2.X, xy2.Y, 0);
        }

        /// <summary>
        /// Draws a line between two points width the set <see cref="LineWidth"/>.
        /// </summary>
        /// <param name="xyz1">The first point.</param>
        /// <param name="xyz2">The second point.</param>
        public void DrawLine(Vector3 xyz1, Vector3 xyz2)
        {
            this.DrawLine(xyz1.X, xyz1.Y, xyz1.Z, xyz2.X, xyz2.Y, xyz2.Z);
        }

        /// <summary>
        /// Draws a line between two points width the set <see cref="LineWidth"/>.
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
        /// Draws a line between two points width the set <see cref="LineWidth"/>.
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
            this.Surface.AddVertices(new PrimitiveVertexData[] { 
                new PrimitiveVertexData(x1 + nx, y1 + ny, z1, this.Color),
                new PrimitiveVertexData(x1 - nx, y1 - ny, z1, this.Color),
                new PrimitiveVertexData(x2 - nx, y2 - ny, z2, this.Color),
                new PrimitiveVertexData(x2 + nx, y2 + ny, z2, this.Color)
                });
        }

        #endregion

    }
}
