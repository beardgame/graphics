using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class StaticVertexSurface<VertexData> : Surface where VertexData : struct, IVertexData
    {
        protected VertexData[] vertices = new VertexData[1];

        protected ushort vertexCount;

        private bool buffersGenerated = false;
        protected int vertexBuffer;

        private bool vertexArrayGenerated = false;
        protected int vertexArray;

        protected int vertexSize { private set; get; }

        private BeginMode beginMode;

        protected bool isStatic = true;
        protected bool staticBufferUploaded = false;

        public StaticVertexSurface(BeginMode primitiveType = BeginMode.Triangles)
        {
            this.beginMode = primitiveType;
            this.vertexSize = vertices[0].Size();
        }

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

        protected void initBuffers(ref int[] buffers)
        {
            if (this.buffersGenerated)
                GL.DeleteBuffers(buffers.Length, buffers);

            GL.GenBuffers(buffers.Length, buffers);
            this.buffersGenerated = true;
        }

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
    }
}
