using System;
using amulware.Graphics.utilities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a vertex buffer object that can be rendered with a specified <see cref="BeginMode"/>.
    /// </summary>
    /// <typeparam name="TVertexData">The <see cref="IVertexData"/> used for the vertex buffer object</typeparam>
    public abstract class StaticVertexSurface<TVertexData> : Surface where TVertexData : struct, IVertexData
    {
        /// <summary>
        /// The OpenGL vertex buffer containing the rendered vertices
        /// </summary>
        protected VertexBuffer<TVertexData> vertexBuffer;

        /// <summary>
        /// The OpenGL vertex array object handle
        /// </summary>
        protected IVertexAttributeProvider<TVertexData> vertexAttributeProvider; 

        private readonly BeginMode _beginMode;

        protected BeginMode beginMode { get { return this._beginMode; } }

        /// <summary>
        /// Wether the vertex buffer object is assumed to be static.
        /// Static buffers will be uploaded to the GPU only once, non-static buffers will be uploaded every draw call.
        /// </summary>
        protected bool isStatic = true;

        /// <summary>
        /// Wether the static vertex buffer was already uploaded to the GPU.
        /// </summary>
        protected bool staticBufferUploaded = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticVertexSurface{TVertexData}"/> class.
        /// </summary>
        /// <param name="primitiveType">Type of the primitives to draw</param>
        public StaticVertexSurface(BeginMode primitiveType = BeginMode.Triangles)
        {
            this._beginMode = primitiveType;
            this.vertexBuffer = new VertexBuffer<TVertexData>();
            this.vertexAttributeProvider = InternalExtensions.IsInLegacyMode ?
                (IVertexAttributeProvider<TVertexData>)new LegacyVertexAttributeProvider<TVertexData>()
                : new VertexArray<TVertexData>(this.vertexBuffer);
        }

        /// <summary>
        /// Handles setting up (new) shader program with this surface.
        /// Calls <see cref="setVertexAttributes"/>.
        /// </summary>
        protected override void onNewShaderProgram()
        {
            this.vertexAttributeProvider.SetShaderProgram(this.program);
        }

        /// <summary>
        /// Renderes the vertex buffer.
        /// Does so by binding it and the vertex array object, uploading the vertices to the GPU (if first call or is not static), drawing with specified <see cref="BeginMode"/> and unbinding buffers afterwards.
        /// </summary>
        override protected void render()
        {
            if (this.vertexBuffer.Count == 0)
                return;

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBuffer);

            this.vertexAttributeProvider.SetVertexData();

            bool upload = true;
            if (this.isStatic)
            {
                if (this.staticBufferUploaded)
                    upload = false;
                else
                    this.staticBufferUploaded = true;
            }

            if (upload)
                this.vertexBuffer.BufferData();

            GL.DrawArrays(this._beginMode, 0, this.vertexBuffer.Count);

            this.vertexAttributeProvider.UnSetVertexData();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Clears the vertex buffer.
        /// </summary>
        public virtual void Clear()
        {
            this.vertexBuffer.Clear();
            this.staticBufferUploaded = false;
        }

        /// <summary>
        /// Forces vertex buffer upload next draw call, even if <see cref="IsStatic"/> is set to true.
        /// </summary>
        public virtual void ForceBufferUpload()
        {
            this.staticBufferUploaded = false;
        }

    }
}
