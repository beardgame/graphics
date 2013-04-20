using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    /// <summary>
    /// Extends <see cref="StaticVertexSurface" /> with automatically adding vertices needed for post processing.
    /// </summary>
    public class PostProcessSurface : StaticVertexSurface<PostProcessVertexData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostProcessSurface"/> class.
        /// </summary>
        public PostProcessSurface()
            : base(BeginMode.Quads)
        {
            initVertices();
        }

        private void initVertices()
        {
            this.vertices = new PostProcessVertexData[4];
            this.vertexCount = 4;
            SetRectangle(-1, -1, 1, 1);
        }

        /// <summary>
        /// Sets the screen rectangle the post processing is applied to.
        /// </summary>
        /// <remarks>Note that the screen space is [-1, 1]x[-1, 1].</remarks>
        /// <param name="from">First corner of rectangle.</param>
        /// <param name="to">Second corner of rectangle.</param>
        public void SetRectangle(Vector2 from, Vector2 to)
        {
            this.SetRectangle(from.X, from.Y, to.X, to.Y);
        }

        /// <summary>
        /// Sets the screen rectangle the post processing is applied to.
        /// </summary>
        /// <remarks>Note that the screen space is [-1, 1]x[-1, 1].</remarks>
        /// <param name="fromX">First corner's x cordinate.</param>
        /// <param name="fromY">First corner's y cordinate.</param>
        /// <param name="toX">Second corner's x cordinate.</param>
        /// <param name="toY">Second corner's y cordinate.</param>
        public void SetRectangle(float fromX, float fromY, float toX, float toY)
        {
            this.vertices[0] = new PostProcessVertexData(fromX, fromY);
            this.vertices[1] = new PostProcessVertexData(toX, fromY);
            this.vertices[2] = new PostProcessVertexData(toX, toY);
            this.vertices[3] = new PostProcessVertexData(fromX, toY);

        }
    }
}
