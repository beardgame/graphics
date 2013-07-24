using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a vertex buffer object that can be rendered with a specified <see cref="BeginMode"/>.
    /// </summary>
    /// <typeparam name="TVertexData">The <see cref="IVertexData"/> used for the vertex buffer object</typeparam>
    public abstract class StaticVertexSurface<TVertexData> : Surface, IDisposable where TVertexData : struct, IVertexData
    {
        /// <summary>
        /// The OpenGL vertex buffer containing the rendered vertices
        /// </summary>
        protected VertexBuffer<TVertexData> vertexBuffer;

        private bool vertexArrayGenerated = false;

        /// <summary>
        /// The OpenGL vertex array object handle
        /// </summary>
        protected int vertexArray;

        private BeginMode beginMode;

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
            this.beginMode = primitiveType;
            this.vertexBuffer = new VertexBuffer<TVertexData>();
        }

        /// <summary>
        /// Handles setting up (new) shader program with this surface.
        /// Calls <see cref="setVertexAttributes"/>.
        /// </summary>
        protected override void onNewShaderProgram()
        {
            this.setVertexAttributes();
        }

        /// <summary>
        /// Sets the vertex attributes of the used <see cref="IVertexData"/> for the current program using a OpenGL vertex array object.
        /// </summary>
        protected void setVertexAttributes()
        {
            if (this.vertexArrayGenerated)
                GL.DeleteVertexArrays(1, ref this.vertexArray);

            GL.GenVertexArrays(1, out this.vertexArray);
            this.vertexArrayGenerated = true;

            GL.BindVertexArray(this.vertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBuffer);
            
            this.program.SetVertexAttributes(new TVertexData().VertexAttributes());

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Renderes the vertex buffer.
        /// Does so by binding it and the vertex array object, uploading the vertices to the GPU (if first call or is not static), drawing with specified <see cref="BeginMode"/> and unbinding buffers afterwards.
        /// </summary>
        override protected void render()
        {
            if (this.vertexBuffer.Count == 0)
                return;

            GL.BindVertexArray(this.vertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBuffer);

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

            GL.DrawArrays(this.beginMode, 0, this.vertexBuffer.Count);

            GL.BindVertexArray(0);
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

        public void Dispose()
        {
            if (this.vertexArrayGenerated)
                GL.DeleteVertexArrays(1, ref this.vertexArray);
        }
    }
}
