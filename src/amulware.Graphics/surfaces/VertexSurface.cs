using System;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{

    /// <summary>
    /// Extends <see cref="StaticVertexSurface{TVertexdata}" /> with the ability to add vertices at will.
    /// </summary>
    /// <typeparam name="TVertexData">The <see cref="IVertexData" /> used.</typeparam>
    public class VertexSurface<TVertexData> : StaticVertexSurface<TVertexData>
        where TVertexData : struct, IVertexData
    {
        /// <summary>
        /// Wether to clear vertex buffer after drawing.
        /// </summary>
        public bool ClearOnRender = true;

        /// <summary>
        /// Set to true to not upload vertices to the GPU with every draw call.
        /// </summary>
        public bool IsStatic
        {
            get { return this.isStatic; }
            set { this.isStatic = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexSurface{VertexData}"/> class.
        /// </summary>
        /// <param name="primitiveType">Type of the primitives to draw.</param>
        public VertexSurface(PrimitiveType primitiveType = PrimitiveType.Triangles)
            : base(primitiveType)
        {
            this.isStatic = false;
        }

        /// <summary>
        /// Adds a vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Index of the vertex in vertex buffer.</returns>
        public ushort AddVertex(TVertexData vertex)
        {
            return this.vertexBuffer.AddVertex(vertex);
        }

        /// <summary>
        /// Adds vertices.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <returns>Index of first new vertex in vertex buffer.</returns>
        public ushort AddVertices(params TVertexData[] vertices)
        {
            return this.vertexBuffer.AddVertices(vertices);
        }

        /// <summary>
        /// Renders the vertex buffer and clears it afterwards, if <see cref="ClearOnRender"/> is set to true.
        /// </summary>
        protected override void render()
        {
            base.render();
            if (this.ClearOnRender)
                this.Clear();
        }
    }
}
