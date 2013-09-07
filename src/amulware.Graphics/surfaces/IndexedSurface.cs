using System;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents an indexed vertex buffer object, that can be draws with a specified <see cref="BeginMode"/>.
    /// </summary>
    public class IndexedSurface<TVertexData> : VertexSurface<TVertexData>
        where TVertexData : struct, IVertexData
    {
        /// <summary>
        /// The OpenGL index buffer container the rendered indices
        /// </summary>
        protected IndexBuffer indexBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedSurface"/> class.
        /// </summary>
        /// <param name="primitiveType">Type of the primitives to draw.</param>
        public IndexedSurface(BeginMode primitiveType = BeginMode.Triangles)
            : base(primitiveType)
        {
            this.indexBuffer = new IndexBuffer();
        }

        /// <summary>
        /// Adds an index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void AddIndex(ushort index)
        {
            this.indexBuffer.AddIndex(index);
        }

        /// <summary>
        /// Adds indices.
        /// </summary>
        /// <param name="indices">The indices.</param>
        public void AddIndices(params ushort[] indices)
        {
            this.indexBuffer.AddIndices(indices);
        }

        /// <summary>
        /// Renders from the index and vertex buffers and clears them afterwards, if <see cref="ClearOnRender"/> is set to true.
        /// </summary>
        protected override void render()
        {
            if (this.indexBuffer.Count == 0)
                return;

            GL.BindVertexArray(this.vertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBuffer);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.indexBuffer);

            bool upload = true;
            if (this.isStatic)
            {
                if (this.staticBufferUploaded)
                    upload = false;
                else
                    this.staticBufferUploaded = true;
            }

            if (upload)
            {
                this.vertexBuffer.BufferData();
                this.indexBuffer.BufferData();
            }

            GL.DrawElements(this.beginMode, this.indexBuffer.Count, DrawElementsType.UnsignedShort, 0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            if (this.ClearOnRender)
                this.Clear();
        }

        /// <summary>
        /// Clears index and vertex buffers.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            this.indexBuffer.Clear();
        }

    }
}
