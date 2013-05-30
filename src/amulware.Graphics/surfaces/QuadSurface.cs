using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// Specialises <see cref="VertexSurface{VertexData}" /> to use <see cref="BeginMode.Quads" /> and adds method to add quads.
    /// </summary>
    /// <typeparam name="VertexData">The <see cref="IVertexData"/> to use.</typeparam>
    public class QuadSurface<VertexData> : VertexSurface<VertexData>
        where VertexData : struct, IVertexData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuadSurface{VertexData}"/> class.
        /// </summary>
        public QuadSurface() : base(BeginMode.Quads) { }

        /// <summary>
        /// Adds a quad of vertices.
        /// </summary>
        /// <param name="v0">The first vertex.</param>
        /// <param name="v1">The second vertex.</param>
        /// <param name="v2">The third vertex.</param>
        /// <param name="v3">The fourth vertex.</param>
        public void AddQuad(VertexData v0, VertexData v1, VertexData v2, VertexData v3)
        {
            this.AddVertices(new VertexData[] { v0, v1, v2, v3 });
        }
    }
}
