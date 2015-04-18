using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents an OpenGL index buffer
    /// </summary>
    sealed public class IndexBuffer : IDisposable
    {
        private ushort[] indices = new ushort[1];

        private int indexCount;

        /// <summary>
        /// The number of indices in this IndexBuffer.
        /// </summary>
        public int Count { get { return this.indexCount; } }

        private readonly int handle;

        /// <summary>
        /// The OpenGL index buffer object handle.
        /// </summary>
        public int Handle { get { return this.handle; } }

        /// <summary>
        /// Initialises a new instance of <see cref="IndexBuffer"/>.
        /// </summary>
        public IndexBuffer()
        {
            this.handle = GL.GenBuffer();
        }

        /// <summary>
        /// Adds am index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void AddIndex(ushort index)
        {
            if (this.indices.Length == this.indexCount)
                Array.Resize(ref this.indices, this.indices.Length * 2);
            this.indices[this.indexCount] = index;
        }

        /// <summary>
        /// Adds indices.
        /// </summary>
        /// <param name="indices">The indices.</param>
        public void AddIndices(params ushort[] indices)
        {
            int newCount = this.indexCount + indices.Length;
            if (this.indices.Length <= newCount)
                Array.Resize(ref this.indices, Math.Max(this.indices.Length * 2, newCount));
            Array.Copy(indices, 0, this.indices, this.indexCount, indices.Length);
            this.indexCount = newCount;
        }

        /// <summary>
        /// Returns a reference to the backing array, and the offset from where to write the indices.
        /// </summary>
        /// <remarks>
        /// The backing array is guaranteed to be large enough to write the given number of indices, providing that the total number of indices is less than 2^31.
        /// </remarks>
        /// <param name="count">The number of new indices we will write.</param>
        /// <param name="offset">The offset of the first new index to write.</param>
        /// <returns>The backing array containing the indices.</returns>
        public ushort[] WriteIndicesDirectly(int count, out int offset)
        {
            int newCount = this.indexCount + count;
            if (this.indices.Length <= newCount)
                Array.Resize(ref this.indices, Math.Max(this.indices.Length * 2, newCount));
            offset = this.indexCount;
            this.indexCount = newCount;
            return this.indices;
        }

        /// <summary>
        /// Binds the index buffer.
        /// </summary>
        /// <param name="target">The target.</param>
        public void Bind(BufferTarget target = BufferTarget.ElementArrayBuffer)
        {
            GL.BindBuffer(target, this);
        }

        /// <summary>
        /// Uploads the index buffer to the GPU.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="usageHint">The usage hint.</param>
        public void BufferData(BufferTarget target = BufferTarget.ElementArrayBuffer, BufferUsageHint usageHint = BufferUsageHint.StreamDraw)
        {
            GL.BufferData(target, (IntPtr)(sizeof(ushort) * this.indexCount), this.indices, usageHint);
        }

        /// <summary>
        /// Clears the index buffer.
        /// </summary>
        public void Clear()
        {
            this.indexCount = 0;
        }


        /// <summary>
        /// Implicit cast to int for easy usage in GL methods that require an integer handle.
        /// </summary>
        static public implicit operator int(IndexBuffer buffer)
        {
            return buffer.handle;
        }
        
        #region Disposing

        private bool disposed;

        /// <summary>
        /// Disposes the buffer, to make sure GL resources are freed.
        /// </summary>
        public void Dispose()
        {
            this.dispose();
            GC.SuppressFinalize(this);
        }

        private void dispose()
        {
            if (this.disposed)
                return;

            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return; 

            GL.DeleteBuffer(this);

            this.disposed = true;
        }

        ~IndexBuffer()
        {
            this.dispose();
        }

        #endregion

    }
}
