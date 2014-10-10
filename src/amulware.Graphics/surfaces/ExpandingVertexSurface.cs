using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// A surface that can render any number of vertices
    /// by automatically utilizing multiple <see cref="VertexBuffer{TVertexData}"/> if needed.
    /// </summary>
    /// <typeparam name="TVertexData">The <see cref="IVertexData" /> used.</typeparam>
    public class ExpandingVertexSurface<TVertexData> : Surface where TVertexData : struct, IVertexData
    {
        private readonly List<VertexBuffer<TVertexData>> vertexBuffers;
        private readonly List<VertexArray<TVertexData>> vertexArrays;

        private readonly PrimitiveType _primitiveType;

        protected PrimitiveType primitiveType { get { return this._primitiveType; } }

        private int activeBufferIndex;
        private VertexBuffer<TVertexData> activeVertexBuffer;

        
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandingVertexSurface{TVertexData}"/> class.
        /// </summary>
        /// <param name="primitiveType">Type of the primitives to draw</param>
        public ExpandingVertexSurface(PrimitiveType primitiveType = PrimitiveType.Triangles)
        {
            this._primitiveType = primitiveType;
            this.vertexBuffers = new List<VertexBuffer<TVertexData>>
                { (this.activeVertexBuffer = new VertexBuffer<TVertexData>()) };

            this.vertexArrays = new List<VertexArray<TVertexData>>
                { new VertexArray<TVertexData>(this.activeVertexBuffer) };
        }

        /// <summary>
        /// Handles setting up (new) shader program with this surface.
        /// Calls <see cref="setVertexAttributes"/>.
        /// </summary>
        protected override void onNewShaderProgram()
        {
            foreach (var vertexArray in this.vertexArrays)
            {
                vertexArray.SetShaderProgram(this.program);
            }
        }

        /// <summary>
        /// Whether the surface has any vertices to render.
        /// If this is false, calling <see cref="Surface.Render"/> has no visual effect.
        /// </summary>
        public bool HasVerticesToRender
        {
            get { return this.vertexBuffers[0].Count > 0; }
        }

        protected override void render()
        {
            if (this.vertexBuffers[0].Count == 0)
                return;

            for (int i = 0; i < this.vertexBuffers.Count; i++)
            {
                var vertexBuffer = this.vertexBuffers[i];
                if (vertexBuffer.Count == 0)
                    break;

                var vertexArray = this.vertexArrays[i];

                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

                vertexArray.SetVertexData();
                vertexBuffer.BufferData();

                GL.DrawArrays(this._primitiveType, 0, vertexBuffer.Count);

                vertexArray.UnSetVertexData();

                vertexBuffer.Clear();
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.activeBufferIndex = 0;
            this.activeVertexBuffer = this.vertexBuffers[0];
        }

        /// <summary>
        /// Adds a vertex to be drawn.
        /// </summary>
        public void AddVertex(TVertexData vertex)
        {
            this.makeBufferSpaceFor(1);
            this.activeVertexBuffer.AddVertex(vertex);
        }
        /// <summary>
        /// Adds two vertices to be drawn.
        /// </summary>
        public void AddVertices(TVertexData v0, TVertexData v1)
        {
            this.makeBufferSpaceFor(2);
            this.activeVertexBuffer.AddVertices(v0, v1);
        }
        /// <summary>
        /// Adds three vertices to be drawn.
        /// </summary>
        public void AddVertices(TVertexData v0, TVertexData v1, TVertexData v2)
        {
            this.makeBufferSpaceFor(3);
            this.activeVertexBuffer.AddVertices(v0, v1, v2);
        }
        /// <summary>
        /// Adds four vertices to be drawn.
        /// </summary>
        public void AddVertices(TVertexData v0, TVertexData v1, TVertexData v2, TVertexData v3)
        {
            this.makeBufferSpaceFor(4);
            this.activeVertexBuffer.AddVertices(v0, v1, v2, v3);
        }
        /// <summary>
        /// Adds vertices to be drawn.
        /// </summary>
        public void AddVertices(params TVertexData[] vertices)
        {
            this.makeBufferSpaceFor(vertices.Length);
            this.activeVertexBuffer.AddVertices(vertices);
        }

        /// <summary>
        /// Exposes the underlying array of the vertex buffer directly,
        /// to allow for faster vertex creation.
        /// </summary>
        /// <param name="count">The amount of vertices to write.
        /// The returned array is guaranteed to have this much space.
        /// Do not write more than this number of vertices.
        /// Note also that writing less vertices than specified may result in undefined behaviour.</param>
        /// <param name="offset">The offset of the first vertex to write to.</param>
        /// <remarks>
        /// <para>Make sure to write vertices to the array in the full range [offset, offset + count[.
        /// Writing more or less or outside that range may result in undefined behaviour.</para>
        /// <para>Do not write more than 2^16 vertices at the same time this way.
        /// Doing so may result in undefined behaviour.</para>
        /// </remarks>
        /// <returns>The underlying array of vertices to write to. This array is only valid for this single call.
        /// To copy more vertices, call this method again and use the new return value.</returns>
        public TVertexData[] WriteVerticesDirectly(int count, out ushort offset)
        {
            this.makeBufferSpaceFor(count);
            return this.activeVertexBuffer.WriteVerticesDirectly(count, out offset);
        }


        /// <summary>
        /// Makes sure the active vertex buffer can handle the given number of new vertices
        /// or switches to/creates a new one of needed.
        /// </summary>
        /// <remarks>If more than 2^16 vertices are added at the same time, this will still result in
        /// undefined behaviour.</remarks>
        private void makeBufferSpaceFor(int i)
        {
            var c = this.activeVertexBuffer.Count;
            if (c + i <= 65536 || c == 0)
                return;

            this.activeBufferIndex++;

            if (this.vertexBuffers.Count > this.activeBufferIndex)
            {
                this.activeVertexBuffer = this.vertexBuffers[this.activeBufferIndex];
                return;
            }

            this.vertexBuffers.Add(this.activeVertexBuffer = new VertexBuffer<TVertexData>(i));
            this.vertexArrays.Add(new VertexArray<TVertexData>(this.activeVertexBuffer));
        }
    }
}
