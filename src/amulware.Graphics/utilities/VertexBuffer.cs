using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents and OpenGL vertex buffer
    /// </summary>
    /// <typeparam name="TVertexData">The type of vertex in the buffer.</typeparam>
    sealed public class VertexBuffer<TVertexData> : IDisposable where TVertexData : struct, IVertexData
    {
        /// <summary>
        /// The array of vertices
        /// </summary>
        private TVertexData[] vertices = new TVertexData[1];

        /// <summary>
        /// The number of vertices in <see cref="vertices"/>. Can be less than vertices.Length, but not more.
        /// </summary>
        private ushort vertexCount;

        /// <summary>
        /// The number of vertices in this VertexBuffer.
        /// </summary>
        public ushort Count { get { return this.vertexCount; } }

        private readonly int handle;

        /// <summary>
        /// The OpenGL vertex buffer object handle
        /// </summary>
        public int Handle { get { return this.handle; } }

        private readonly int vertexSize;

        /// <summary>
        /// The size of a vertex in bytes.
        /// </summary>
        public int VertexSize { get { return this.vertexSize; } }

        /// <summary>
        /// Initialises a new instance of <see cref="VertexBuffer"/>
        /// </summary>
        public VertexBuffer()
        {
            this.vertexSize = new TVertexData().Size();

            this.handle = GL.GenBuffer();
        }

        /// <summary>
        /// Adds a vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Index of the vertex in vertex buffer.</returns>
        public ushort AddVertex(TVertexData vertex)
        {
            if (this.vertices.Length == this.vertexCount)
                Array.Resize(ref this.vertices, this.vertices.Length * 2);
            this.vertices[this.vertexCount] = vertex;
            return this.vertexCount++;
        }

        /// <summary>
        /// Adds vertices.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <returns>Index of first new vertex in vertex buffer.</returns>
        public ushort AddVertices(params TVertexData[] vertices)
        {
            ushort oldCount = this.vertexCount;
            int newCount = oldCount + vertices.Length;
            if (this.vertices.Length <= newCount)
                Array.Resize(ref this.vertices, Math.Max(this.vertices.Length * 2, newCount));
            Array.Copy(vertices, 0, this.vertices, this.vertexCount, vertices.Length);
            this.vertexCount = (ushort)newCount;
            return oldCount;
        }

        /// <summary>
        /// Uploads the vertex buffer to the GPU
        /// </summary>
        /// <param name="target">The target</param>
        /// <param name="usageHint">The usage hint</param>
        public void BufferData(BufferTarget target = BufferTarget.ArrayBuffer, BufferUsageHint usageHint = BufferUsageHint.StreamDraw)
        {
            GL.BufferData(target, (IntPtr)(this.vertexSize * this.vertexCount), this.vertices, usageHint);
        }

        /// <summary>
        /// Clears the vertex buffer.
        /// </summary>
        public void Clear()
        {
            this.vertexCount = 0;
        }

        static public implicit operator int(VertexBuffer<TVertexData> buffer)
        {
            return buffer.handle;
        }

        #region Disposing

        private bool disposed = false;

        public void Dispose()
        {
            this.dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return;

            GL.DeleteBuffer(this);

            this.disposed = true;
        }

        ~VertexBuffer()
        {
            this.dispose(false);
        }

        #endregion
    }
}
