using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    /// <summary>
    /// Light vertex data used for post processing.
    /// </summary>
    public struct PostProcessVertexData : IVertexData
    {
        // add attributes and constructors here
        /// <summary>
        /// The position
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// The uv coordinates
        /// </summary>
        public Vector2 TexCoord;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostProcessVertexData"/> struct.
        /// </summary>
        /// <param name="position">The screen position.</param>
        public PostProcessVertexData(Vector2 position)
        {
            this.Position = position;
            this.TexCoord.X = (position.X + 1) * 0.5f;
            this.TexCoord.Y = (position.Y + 1) * 0.5f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostProcessVertexData"/> struct.
        /// </summary>
        /// <param name="x">The x screen coordinate.</param>
        /// <param name="y">The y screen coordinate.</param>
        public PostProcessVertexData(float x, float y)
        {
            this.Position.X = x;
            this.Position.Y = y;
            this.TexCoord.X = (x + 1) * 0.5f;
            this.TexCoord.Y = (y + 1) * 0.5f;
        }

        static private VertexAttribute[] vertexAttributes;

        static private void setVertexAttributes()
        {
            PostProcessVertexData.vertexAttributes = new VertexAttribute[]{
                // add new VertexAttributes here
                new VertexAttribute("v_position", 2, VertexAttribPointerType.Float, 16, 0),
                new VertexAttribute("v_texCoord", 2, VertexAttribPointerType.Float, 16, 8)
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
            if (PostProcessVertexData.vertexAttributes == null)
                PostProcessVertexData.setVertexAttributes();
            return PostProcessVertexData.vertexAttributes;
        }

        /// <summary>
        /// This method returns the size of the vertex data struct in bytes
        /// </summary>
        /// <returns>
        /// Struct's size in bytes
        /// </returns>
        public int Size()
        {
            // return size of struct (in bytes) here
            return 16;
        }
    }
}
