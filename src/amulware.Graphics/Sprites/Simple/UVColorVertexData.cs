using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// Light vertex data used for rendering textured vertices, like sprites.
    /// </summary>
    public struct UVColorVertexData : IVertexData
    {
        // add attributes and constructors here
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

        static private VertexAttribute[] vertexAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="UVColorVertexData"/> struct.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="uv">The uv coordinate.</param>
        /// <param name="color">The color.</param>
        public UVColorVertexData(Vector3 position, Vector2 uv, Color color)
        {
            this.Position = position;
            this.TexCoord = uv;
            this.Color = color;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UVColorVertexData"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="uv">The uv coordinate.</param>
        /// <param name="color">The color.</param>
        public UVColorVertexData(float x, float y, float z, Vector2 uv, Color color)
        {
            this.Position.X = x;
            this.Position.Y = y;
            this.Position.Z = z;
            this.TexCoord = uv;
            this.Color = color;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UVColorVertexData"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="u">The u coordinate.</param>
        /// <param name="v">The v coordinate.</param>
        /// <param name="color">The color.</param>
        public UVColorVertexData(float x, float y, float z, float u, float v, Color color)
        {
            this.Position.X = x;
            this.Position.Y = y;
            this.Position.Z = z;
            this.TexCoord.X = u;
            this.TexCoord.Y = v;
            this.Color = color;
        }

        static private void setVertexAttributes()
        {
            UVColorVertexData.vertexAttributes = new VertexAttribute[]{
                new VertexAttribute("v_position", 3, VertexAttribPointerType.Float, 24, 0),
                new VertexAttribute("v_texcoord", 2, VertexAttribPointerType.Float, 24, 12),
                new VertexAttribute("v_color", 4, VertexAttribPointerType.UnsignedByte, 24, 20, true)
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
            if (UVColorVertexData.vertexAttributes == null)
                UVColorVertexData.setVertexAttributes();
            return UVColorVertexData.vertexAttributes;
        }

        /// <summary>
        /// This method returns the size of the vertex data struct in bytes
        /// </summary>
        /// <returns>
        /// Struct's size in bytes
        /// </returns>
        public int Size()
        {
            return 24;
        }
    }
}
