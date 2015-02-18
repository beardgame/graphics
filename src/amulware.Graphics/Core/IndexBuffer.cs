using System;
using OpenTK;
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
        /// The OpenGL index buffer object handle
        /// </summary>
        public int Handle { get { return this.handle; } }

        /// <summary>
        /// Initialises a new instance of <see cref="IndexBuffer"/>
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
        /// Uploads the index buffer to the GPU
        /// </summary>
        /// <param name="target">The target</param>
        /// <param name="usageHint">The usage hint</param>
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


        static public implicit operator int(IndexBuffer buffer)
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

        ~IndexBuffer()
        {
            this.dispose(false);
        }

        #endregion

    }
}
