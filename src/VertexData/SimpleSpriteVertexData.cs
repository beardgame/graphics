using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    /// <summary>
    /// Custom vertex data used for drawing GPU accelerated camera space aligned sprites in three dimensional space
    /// </summary>
    public struct SimpleSpriteVertexData : IVertexData
    {
        /// <summary>
        /// The position
        /// </summary>
        public Vector3 Position; // 12 bytes
        /// <summary>
        /// The uv coordinate
        /// </summary>
        public Vector2 TexCoord; // 8 bytes
        /// <summary>
        /// The color
        /// </summary>
        public Color Color; // 4 bytes
        /// <summary>
        /// The vector by which to move the vertex in camera space, parallel to the view plane
        /// </summary>
        public Vector2 Expand; // 8 bytes

        static private VertexAttribute[] vertexAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSpriteVertexData"/> struct.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="uv">The uv coordinates.</param>
        /// <param name="color">The color.</param>
        /// <param name="expand">The vector by which to move the vertex in camera space, parallel to the view plane.</param>
        public SimpleSpriteVertexData(Vector3 position, Vector2 uv, Color color, Vector2 expand)
        {
            this.Position = position;
            this.TexCoord = uv;
            this.Color = color;
            this.Expand = expand;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSpriteVertexData"/> struct.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="uv">The uv coordinate.</param>
        /// <param name="color">The color.</param>
        /// <param name="expandX">The x offset by which to move the vertex in camera space, parallel to the view plane.</param>
        /// <param name="expandY">The y offset by which to move the vertex in camera space, parallel to the view plane</param>
        public SimpleSpriteVertexData(Vector3 position, Vector2 uv, Color color, float expandX, float expandY)
        {
            this.Position = position;
            this.TexCoord = uv;
            this.Color = color;
            this.Expand.X = expandX;
            this.Expand.Y = expandY;
        }

        static private void setVertexAttributes()
        {
            SimpleSpriteVertexData.vertexAttributes = new VertexAttribute[]{
                new VertexAttribute("v_position", 3, VertexAttribPointerType.Float, 32, 0),
                new VertexAttribute("v_texcoord", 2, VertexAttribPointerType.Float, 32, 12),
                new VertexAttribute("v_color", 4, VertexAttribPointerType.UnsignedByte, 32, 20, true),
                new VertexAttribute("v_expand", 2, VertexAttribPointerType.Float, 32, 24),
            };
        }

        /// <summary>
        /// Returns the vertex' <see cref="VertexAttributes" />
        /// </summary>
        /// <returns>
        /// Array of <see cref="VertexAttribute" />
        /// </returns>
        public VertexAttribute[] VertexAttributes()
        {
            if (SimpleSpriteVertexData.vertexAttributes == null)
                SimpleSpriteVertexData.setVertexAttributes();
            return SimpleSpriteVertexData.vertexAttributes;
        }

        /// <summary>
        /// This method returns the size of the vertex data struct in bytes
        /// </summary>
        /// <returns>
        /// Struct's size in bytes
        /// </returns>
        public int Size()
        {
            return 32;
        }
    }
}
