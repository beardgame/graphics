using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents an OpenGL index buffer object.
    /// </summary>
    /// <remarks>Note that this object can hold no more than 2^31 indices, and that indices are stored as 16 bit unsigned integer.</remarks>
    sealed public class IndexBuffer : IDisposable
    {
        #region Fields

        private readonly int handle;
        private ushort[] indices;

        private int count;

        #endregion

        #region Properties

        /// <summary>
        /// The OpenGL index buffer object handle.
        /// </summary>
        public int Handle { get { return this.handle; } }

        /// <summary>
        /// The number of indices in this IndexBuffer.
        /// </summary>
        public int Count { get { return this.count; } }

        /// <summary>
        /// The size of the underlying array of indices.
        /// The array resizes automatically, when more indices are added.
        /// </summary>
        public int Capacity { get { return this.indices.Length; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialises a new instance of <see cref="IndexBuffer"/>.
        /// </summary>
        /// <param name="capacity">The initial capacity of the buffer.</param>
        public IndexBuffer(int capacity = 0)
        {
            this.indices = new ushort[capacity > 0 ? capacity : 1];
            this.handle = GL.GenBuffer();
        }

        #endregion

        #region Methods

        #region Private

        private void ensureCapacity(int minCapacity)
        {
            if (this.indices.Length <= minCapacity)
                Array.Resize(ref this.indices, Math.Max(this.indices.Length * 2, minCapacity));
        }

        #endregion

        #region Public

        #region Adding

        /// <summary>
        /// Adds an index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void AddIndex(ushort index)
        {
            if (this.indices.Length == this.count)
                Array.Resize(ref this.indices, this.indices.Length * 2);
            this.indices[this.count] = index;
        }

        /// <summary>
        /// Adds indices.
        /// </summary>
        public void AddIndices(ushort index0, ushort index1)
        {
            int newCount = this.count + 2;
            this.ensureCapacity(newCount);
            this.indices[this.count] = index0;
            this.indices[this.count + 1] = index1;
            this.count = newCount;
        }
        
        /// <summary>
        /// Adds indices.
        /// </summary>
        public void AddIndices(ushort index0, ushort index1, ushort index2)
        {
            int newCount = this.count + 3;
            this.ensureCapacity(newCount);
            this.indices[this.count] = index0;
            this.indices[this.count + 1] = index1;
            this.indices[this.count + 2] = index2;
            this.count = newCount;
        }

        /// <summary>
        /// Adds indices.
        /// </summary>
        /// <param name="indices">The indices.</param>
        public void AddIndices(params ushort[] indices)
        {
            int newCount = this.count + indices.Length;
            this.ensureCapacity(newCount);
            Array.Copy(indices, 0, this.indices, this.count, indices.Length);
            this.count = newCount;
        }

        /// <summary>
        /// Exposes the underlying array of the index buffer directly,
        /// to allow for faster index creation.
        /// </summary>
        /// <param name="count">The amount of indices to write.
        /// The returned array is guaranteed to have this much space.
        /// Do not write more than this number of indices.
        /// Note also that writing less indices than specified may result in undefined behaviour.</param>
        /// <param name="offset">The offset of the first index to write to.</param>
        /// <remarks>Write indices to the array in the full range [offset, offset + count[.
        /// Writing more or less or outside that range may result in undefined behaviour.</remarks>
        /// <returns>The underlying array of indices to write to. This array is only valid for this single call.
        /// To copy more indices, call this method again and use the new return value.</returns>
        public ushort[] WriteIndicesDirectly(int count, out int offset)
        {
            int newCount = this.count + count;
            this.ensureCapacity(newCount);
            offset = this.count;
            this.count = newCount;
            return this.indices;
        }

        #endregion

        #region Removing

        /// <summary>
        /// Removes the last <paramref name="count"/> indices.
        /// </summary>
        /// <param name="count"></param>
        public void RemoveIndices(int count)
        {
            this.count = count > this.count
                ? 0
                : (this.count - count);
        }

        /// <summary>
        /// Clears the index buffer.
        /// </summary>
        public void Clear()
        {
            this.count = 0;
        }

        #endregion

        #region GL

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
            GL.BufferData(target, (IntPtr)(sizeof(ushort) * this.count), this.indices, usageHint);
        }

        #endregion

        #endregion

        #endregion

        #region Operators

        /// <summary>
        /// Implicit cast to int for easy usage in GL methods that require an integer handle.
        /// </summary>
        static public implicit operator int(IndexBuffer buffer)
        {
            return buffer.handle;
        }

        #endregion

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
