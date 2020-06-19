using OpenToolkit.Graphics.OpenGL;

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
        public IndexedSurface(PrimitiveType primitiveType = PrimitiveType.Triangles)
            : base(primitiveType)
        {
            this.indexBuffer = new IndexBuffer();
        }

        public int IndexCount
        {
            get { return this.indexBuffer.Count; }
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

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBuffer);
            this.vertexAttributeProvider.SetVertexData();

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

            GL.DrawElements(this.primitiveType, this.indexBuffer.Count, DrawElementsType.UnsignedShort, 0);

            this.vertexAttributeProvider.UnSetVertexData();
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

        /// <summary>
        /// Adds a triangle of vertices and automatically adds the correct indices.
        /// </summary>
        /// <param name="v0">The first vertex.</param>
        /// <param name="v1">The second vertex.</param>
        /// <param name="v2">The third vertex.</param>
        public void AddTriangle(TVertexData v0, TVertexData v1, TVertexData v2)
        {
            ushort i = this.vertexBuffer.AddVertices(v0, v1, v2);

            this.indexBuffer.AddIndices(i, (ushort)(i + 1), (ushort)(i + 2));
        }

        /// <summary>
        /// Adds a quad of vertices and automatically adds the correct indices.
        /// </summary>
        /// <remarks>The vertices are expected in clockwise or anticlockwise order.</remarks>
        /// <param name="v0">The first vertex.</param>
        /// <param name="v1">The second vertex.</param>
        /// <param name="v2">The third vertex.</param>
        /// <param name="v3">The fourth vertex.</param>
        public void AddQuad(TVertexData v0, TVertexData v1, TVertexData v2, TVertexData v3)
        {
            ushort i = this.vertexBuffer.AddVertices(v0, v1, v2, v3);

            this.indexBuffer.AddIndices(
                i, (ushort)(i + 1), (ushort)(i + 3),
                (ushort)(i + 2), (ushort)(i + 3), (ushort)(i + 1));
        }

        /// <summary>
        /// Adds a quad of vertices and automatically adds the correct indices.
        /// </summary>
        /// <remarks>The vertices are expected in clockwise or anticlockwise order.</remarks>
        /// <param name="v0">The first vertex.</param>
        /// <param name="v1">The second vertex.</param>
        /// <param name="v2">The third vertex.</param>
        /// <param name="v3">The fourth vertex.</param>
        /// <param name="alternateTriangulation">Switch quad triangulation.</param>
        public void AddQuad(TVertexData v0, TVertexData v1, TVertexData v2, TVertexData v3, bool alternateTriangulation)
        {
            ushort i = this.vertexBuffer.AddVertices(v0, v1, v2, v3);

            ushort t0v3;
            ushort t1v3;

            if (alternateTriangulation)
            {
                t0v3 = (ushort)(i + 2);
                t1v3 = i;
            }
            else
            {
                t0v3 = (ushort)(i + 3);
                t1v3 = (ushort)(i + 1);
            }


            this.indexBuffer.AddIndices(
                i, (ushort)(i + 1), t0v3,
                (ushort)(i + 2), (ushort)(i + 3), t1v3);
        }

        public TVertexData[] WriteQuadsDirectly(int count, out int offset)
        {
            ushort vOffset;
            var vertices = this.vertexBuffer.WriteVerticesDirectly(count * 4, out vOffset);
            offset = vOffset;

            int iOffset;
            var indices = this.indexBuffer.WriteIndicesDirectly(count * 6, out iOffset);

            var iMax = iOffset + count * 6;
            for (int i = iOffset, v = vOffset; i < iMax; i += 6, v += 4)
            {
                indices[i] = (ushort)v;
                indices[i + 1] = (ushort)(v + 1);
                indices[i + 2] = (ushort)(v + 3);

                indices[i + 3] = (ushort)(v + 2);
                indices[i + 4] = (ushort)(v + 3);
                indices[i + 5] = (ushort)(v + 1);
            }

            return vertices;
        }

        public ushort[] WriteIndicesDirectly(int count, out int offset)
        {
            return this.indexBuffer.WriteIndicesDirectly(count, out offset);
        }

        public override void Dispose()
        {
            base.Dispose();
            indexBuffer.Dispose();
        }
    }
}
