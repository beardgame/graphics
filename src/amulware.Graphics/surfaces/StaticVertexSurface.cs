using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a vertex buffer object that can be rendered with a specified <see cref="BeginMode"/>.
    /// </summary>
    /// <typeparam name="VertexData">The <see cref="IVertexData"/> used for the vertex buffer object</typeparam>
    public abstract class StaticVertexSurface<VertexData> : Surface where VertexData : struct, IVertexData
    {
        /// <summary>
        /// The array of vertices
        /// </summary>
        protected VertexData[] vertices = new VertexData[1];

        /// <summary>
        /// The number of vertices in <see cref="vertices"/>. Can be less than vertices.Length, but not more.
        /// </summary>
        protected ushort vertexCount;

        private bool buffersGenerated = false;

        /// <summary>
        /// The OpenGL vertex buffer object handle
        /// </summary>
        protected int vertexBuffer;

        private bool vertexArrayGenerated = false;

        /// <summary>
        /// The OpenGL vertex array object handle
        /// </summary>
        protected int vertexArray;

        /// <summary>
        /// This size of a vertex in bytes.
        /// </summary>
        protected int vertexSize { private set; get; }

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
        /// Initializes a new instance of the <see cref="StaticVertexSurface{VertexData}"/> class.
        /// </summary>
        /// <param name="primitiveType">Type of the primitives to draw</param>
        public StaticVertexSurface(BeginMode primitiveType = BeginMode.Triangles)
        {
            this.beginMode = primitiveType;
            this.vertexSize = vertices[0].Size();
        }

        /// <summary>
        /// Handles setting up (new) shader program with this surface.
        /// Calls <see cref="initBuffers"/> on first call to initialise the vertex buffer.
        /// Calls <see cref="setVertexAttributes"/>.
        /// </summary>
        protected override void onNewShaderProgram()
        {
            if (!buffersGenerated)
            {
                int[] buffers = new int[] { this.vertexBuffer };

                this.initBuffers(ref buffers);

                this.vertexBuffer = buffers[0];
            }
            this.setVertexAttributes();
        }

        /// <summary>
        /// (Re)initialises buffers.
        /// </summary>
        /// <param name="buffers">Buffers to (re)initialise</param>
        protected void initBuffers(ref int[] buffers)
        {
            if (this.buffersGenerated)
                GL.DeleteBuffers(buffers.Length, buffers);

            GL.GenBuffers(buffers.Length, buffers);
            this.buffersGenerated = true;
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
            
            this.program.SetVertexAttributes(this.vertices[0].VertexAttributes());

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Renderes the vertex buffer.
        /// Does so by binding it and the vertex array object, uploading the vertices to the GPU (if first call or is not static), drawing with specified <see cref="BeginMode"/> and unbinding buffers afterwards.
        /// </summary>
        override protected void render()
        {
            if (this.vertexCount == 0)
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
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.vertexSize * this.vertexCount), this.vertices, BufferUsageHint.StreamDraw);

            GL.DrawArrays(this.beginMode, 0, this.vertexCount);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Clears the vertex buffer.
        /// </summary>
        public virtual void Clear()
        {
            this.vertexCount = 0;
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
