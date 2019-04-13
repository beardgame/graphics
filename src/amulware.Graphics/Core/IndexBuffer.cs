using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents an OpenGL index buffer object.
    /// </summary>
    /// <remarks>
    /// This object can hold no more than 2^31 indices, and indices are stored as 16 bit unsigned integer.
    /// </remarks>
    public sealed class IndexBuffer : IDisposable
    {
        #region Fields

        private ushort[] indices;

        #endregion

        #region Properties

        /// <summary>
        /// The OpenGL index buffer object handle.
        /// </summary>
        public int Handle { get; }

        /// <summary>
        /// The number of indices in this IndexBuffer.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// The size of the underlying array of indices.
        /// The array resizes automatically, when more indices are added.
        /// </summary>
        public int Capacity => indices.Length;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialises a new instance of <see cref="IndexBuffer"/>.
        /// </summary>
        /// <param name="capacity">The initial capacity of the buffer.</param>
        public IndexBuffer(int capacity = 0)
        {
            indices = new ushort[capacity > 0 ? capacity : 1];
            Handle = GL.GenBuffer();
        }

        #endregion

        #region Methods

        #region Private

        private void ensureCapacity(int minCapacity)
        {
            if (indices.Length <= minCapacity)
                Array.Resize(ref indices, Math.Max(indices.Length * 2, minCapacity));
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
            if (indices.Length == Count)
                Array.Resize(ref indices, indices.Length * 2);
            indices[Count] = index;
        }

        /// <summary>
        /// Adds indices.
        /// </summary>
        public void AddIndices(ushort index0, ushort index1)
        {
            var newCount = Count + 2;
            ensureCapacity(newCount);
            indices[Count] = index0;
            indices[Count + 1] = index1;
            Count = newCount;
        }

        /// <summary>
        /// Adds indices.
        /// </summary>
        public void AddIndices(ushort index0, ushort index1, ushort index2)
        {
            var newCount = Count + 3;
            ensureCapacity(newCount);
            indices[Count] = index0;
            indices[Count + 1] = index1;
            indices[Count + 2] = index2;
            Count = newCount;
        }

        /// <summary>
        /// Adds indices.
        /// </summary>
        /// <param name="newIndices">The indices.</param>
        public void AddIndices(params ushort[] newIndices)
        {
            var newCount = Count + newIndices.Length;
            ensureCapacity(newCount);
            Array.Copy(newIndices, 0, indices, Count, newIndices.Length);
            Count = newCount;
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
            var newCount = Count + count;
            ensureCapacity(newCount);
            offset = Count;
            Count = newCount;
            return indices;
        }

        #endregion

        #region Removing

        /// <summary>
        /// Removes the last <paramref name="count"/> indices.
        /// </summary>
        /// <param name="count">The number of indices to remove.</param>
        public void RemoveIndices(int count)
        {
            Count = count > Count
                ? 0
                : (Count - count);
        }

        /// <summary>
        /// Clears the index buffer.
        /// </summary>
        public void Clear()
        {
            Count = 0;
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
        public void BufferData(
            BufferTarget target = BufferTarget.ElementArrayBuffer,
            BufferUsageHint usageHint = BufferUsageHint.StreamDraw)
        {
            GL.BufferData(target, (IntPtr) (sizeof(ushort) * Count), indices, usageHint);
        }

        #endregion

        #endregion

        #endregion

        #region Operators

        /// <summary>
        /// Implicit cast to int for easy usage in GL methods that require an integer handle.
        /// </summary>
        public static implicit operator int(IndexBuffer buffer) => buffer.Handle;

        #endregion

        #region Disposing

        private bool disposed;

        /// <summary>
        /// Disposes the buffer, to make sure GL resources are freed.
        /// </summary>
        public void Dispose()
        {
            dispose();
            GC.SuppressFinalize(this);
        }

        private void dispose()
        {
            if (disposed)
                return;

            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return;

            GL.DeleteBuffer(this);

            disposed = true;
        }

        ~IndexBuffer()
        {
            dispose();
        }

        #endregion
    }
}
