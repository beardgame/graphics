using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
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

        /// <summary>
        /// The OpenGL vertex buffer object handle
        /// </summary>
        public int Handle { get; private set; }

        private bool bufferGenerated = false;

        private readonly int vertexSize;

        /// <summary>
        /// This size of a vertex in bytes.
        /// </summary>
        public int VertexSize { get { return this.vertexSize; } }

        public VertexBuffer()
        {
            this.vertexSize = new TVertexData().Size();
            this.initBuffer();
        }

        private void initBuffer()
        {
            int[] buffers = new int[] { this.Handle };

            if (this.bufferGenerated)
                GL.DeleteBuffers(1, buffers);

            GL.GenBuffers(1, buffers);

            this.Handle = buffers[0];
            this.bufferGenerated = true;
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
        public ushort AddVertices(TVertexData[] vertices)
        {
            ushort oldCount = this.vertexCount;
            int newCount = oldCount + vertices.Length;
            if (this.vertices.Length <= newCount)
                //this.vertices.CopyTo(this.vertices = new TVertexData[Math.Max(this.vertices.Length * 2, this.vertexCount + vertices.Length)], 0);
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
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.vertexSize * this.vertexCount), this.vertices, BufferUsageHint.StreamDraw);
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
            return buffer.Handle;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
