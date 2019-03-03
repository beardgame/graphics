using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents and OpenGL vertex buffer object.
    /// </summary>
    /// <remarks>This object can hold no more than 2^16 vertices.</remarks>
    /// <typeparam name="TVertexData">The type of vertex in the buffer.</typeparam>
    public sealed class VertexBuffer<TVertexData> : IDisposable where TVertexData : struct, IVertexData
    {
        #region Fields

        private TVertexData[] vertices;

        #endregion

        #region Properties

        /// <summary>
        /// The OpenGL vertex buffer object handle.
        /// </summary>
        public int Handle { get; }

        /// <summary>
        /// The size of a vertex in bytes.
        /// </summary>
        public int VertexSize { get; }

        /// <summary>
        /// The number of vertices in this VertexBuffer.
        /// </summary>
        public ushort Count { get; private set; }

        /// <summary>
        /// The size of the underlying array of vertices.
        /// The array resizes automatically, when more vertices are added.
        /// </summary>
        public int Capacity => vertices.Length;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialises a new instance of <see cref="VertexBuffer{TVertexData}"/>.
        /// </summary>
        /// <param name="capacity">The initial capacity of the buffer.</param>
        public VertexBuffer(int capacity = 0)
        {
            VertexSize = VertexData.SizeOf<TVertexData>();

            vertices = new TVertexData[capacity > 0 ? capacity : 4];

            Handle = GL.GenBuffer();
        }

        #endregion

        #region Methods

        #region Private

        private void ensureCapacity(int minCapacity)
        {
            if (vertices.Length <= minCapacity)
                Array.Resize(ref vertices, Math.Max(vertices.Length * 2, minCapacity));
        }

        #endregion

        #region Public

        #region Adding

        /// <summary>
        /// Adds a vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Index of the vertex in vertex buffer.</returns>
        public ushort AddVertex(TVertexData vertex)
        {
            if (vertices.Length == Count)
                Array.Resize(ref vertices, vertices.Length * 2);
            vertices[Count] = vertex;
            return Count++;
        }

        /// <summary>
        /// Adds two vertices.
        /// </summary>
        /// <returns>Index of first new vertex in vertex buffer.</returns>
        public ushort AddVertices(TVertexData vertex0, TVertexData vertex1)
        {
            var oldCount = Count;
            var newCount = oldCount + 2;
            ensureCapacity(newCount);
            Count = (ushort)newCount;

            vertices[oldCount] = vertex0;
            vertices[oldCount + 1] = vertex1;

            return oldCount;
        }

        /// <summary>
        /// Adds three vertices.
        /// </summary>
        /// <returns>Index of first new vertex in vertex buffer.</returns>
        public ushort AddVertices(TVertexData vertex0, TVertexData vertex1, TVertexData vertex2)
        {
            var oldCount = Count;
            var newCount = oldCount + 3;
            ensureCapacity(newCount);
            Count = (ushort)newCount;

            vertices[oldCount] = vertex0;
            vertices[oldCount + 1] = vertex1;
            vertices[oldCount + 2] = vertex2;

            return oldCount;
        }

        /// <summary>
        /// Adds four vertices.
        /// </summary>
        /// <returns>Index of first new vertex in vertex buffer.</returns>
        public ushort AddVertices(TVertexData vertex0, TVertexData vertex1, TVertexData vertex2, TVertexData vertex3)
        {
            var oldCount = Count;
            var newCount = oldCount + 4;
            ensureCapacity(newCount);
            Count = (ushort)newCount;

            vertices[oldCount] = vertex0;
            vertices[oldCount + 1] = vertex1;
            vertices[oldCount + 2] = vertex2;
            vertices[oldCount + 3] = vertex3;

            return oldCount;
        }

        /// <summary>
        /// Adds vertices.
        /// </summary>
        /// <param name="newVertices">The vertices.</param>
        /// <returns>Index of first new vertex in vertex buffer.</returns>
        public ushort AddVertices(params TVertexData[] newVertices)
        {
            var oldCount = Count;
            var newCount = oldCount + newVertices.Length;
            ensureCapacity(newCount);
            Array.Copy(newVertices, 0, vertices, Count, newVertices.Length);
            Count = (ushort)newCount;
            return oldCount;
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
        /// <remarks>Write vertices to the array in the full range [offset, offset + count[.
        /// Writing more or less or outside that range may result in undefined behaviour.</remarks>
        /// <returns>The underlying array of vertices to write to. This array is only valid for this single call.
        /// To copy more vertices, call this method again and use the new return value.</returns>
        public TVertexData[] WriteVerticesDirectly(int count, out ushort offset)
        {
            var oldCount = Count;
            var newCount = oldCount + count;

            ensureCapacity(newCount);

            Count = (ushort)newCount;

            offset = oldCount;
            return vertices;
        }

        #endregion

        #region Removing

        /// <summary>
        /// Removes a the last <paramref name="count"/> vertices added.
        /// </summary>
        public void RemoveVertices(int count) =>
            Count = count > Count
                ? (ushort)0
                : (ushort)(Count - count);

        /// <summary>
        /// Clears the vertex buffer.
        /// </summary>
        public void Clear()
        {
            Count = 0;
        }

        #endregion

        #endregion

        #region GL

        /// <summary>
        /// Binds the vertex buffer object.
        /// </summary>
        /// <param name="target">The target.</param>
        public void Bind(BufferTarget target = BufferTarget.ArrayBuffer)
        {
            GL.BindBuffer(target, this);
        }

        /// <summary>
        /// Uploads the vertex buffer to the GPU.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="usageHint">The usage hint.</param>
        public void BufferData(
            BufferTarget target = BufferTarget.ArrayBuffer, BufferUsageHint usageHint = BufferUsageHint.StreamDraw)
        {
            GL.BufferData(target, (IntPtr)(VertexSize * Count), vertices, usageHint);
        }

        /// <summary>
        /// Reserve a number of bytes in GPU memory for this vertex buffer.
        /// </summary>
        /// <remarks>
        /// This is useful for advanced features like transform feedback.
        /// </remarks>
        /// <param name="vertexCount">The amount of vertices reserved.</param>
        /// <param name="target">The target.</param>
        /// <param name="usageHint">The usage hin.t</param>
        /// <param name="setVertexCount">Whether to set the given vertex count as size of this vertex buffer.</param>
        public void BufferNoData(
            int vertexCount,
            BufferTarget target = BufferTarget.ArrayBuffer,
            BufferUsageHint usageHint = BufferUsageHint.StreamDraw,
            bool setVertexCount = false)
        {
            GL.BufferData(target, (IntPtr)(VertexSize * vertexCount), IntPtr.Zero, usageHint);
            if (setVertexCount)
                Count = (ushort)vertexCount;
        }

        #endregion

        #endregion

        #region Operators

        /// <summary>
        /// Implicit cast to easily use vertex buffers in GL functions expecting an integer handle.
        /// </summary>
        public static implicit operator int(VertexBuffer<TVertexData> buffer) => buffer.Handle;

        #endregion

        #region Disposing

        private bool disposed;

        /// <summary>
        /// Disposes the vertex buffer and deletes the underlying GL object.
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

        ~VertexBuffer()
        {
            dispose();
        }

        #endregion
    }
}
