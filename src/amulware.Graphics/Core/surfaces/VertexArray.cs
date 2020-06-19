using System;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    internal class VertexArray<TVertexData>
        : IVertexAttributeProvider<TVertexData>, IDisposable
        where TVertexData : struct, IVertexData
    {
        private readonly VertexBuffer<TVertexData> vertexBuffer;
        private int handle;
        private bool vertexArrayGenerated;

        public VertexArray(VertexBuffer<TVertexData> vertexBuffer)
        {
            this.vertexBuffer = vertexBuffer;
        }

        public void SetVertexData()
        {
            GL.BindVertexArray(this.handle);
        }

        public void UnSetVertexData()
        {
            GL.BindVertexArray(0);
        }

        public void SetShaderProgram(ShaderProgram program)
        {
            if (this.vertexArrayGenerated)
                GL.DeleteVertexArrays(1, ref this.handle);

            GL.GenVertexArrays(1, out this.handle);
            this.vertexArrayGenerated = true;

            GL.BindVertexArray(this.handle);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBuffer);

            program.SetVertexAttributes(new TVertexData().VertexAttributes());

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
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

            if (this.vertexArrayGenerated)
                GL.DeleteVertexArray(this.handle);

            this.disposed = true;
        }

        ~VertexArray()
        {
            this.dispose(false);
        }

        #endregion
    }
}
