using System;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents an indexed vertex buffer object. It is not up to date and cannot be used at this point.
    /// </summary>
    public class IndexedSurface<VertexData> : VertexSurface<VertexData>
        where VertexData : struct, IVertexData
    {
        //todo: check file for functionality

        ushort[] indices = new ushort[16];
        int indexCount = 0;

        readonly int indexBytes;

        int indexBuffer;

        BeginMode beginMode;

        public IndexedSurface(BeginMode primitiveType = BeginMode.Triangles)
            : base()
        {
            this.beginMode = primitiveType;
            this.indexBytes = sizeof(ushort);
            throw new ApplicationException("Indexed surfaces not up to date!");
        }

        public int AddIndex(ushort index)
        {
            if (this.indices.Length == this.indexCount)
                this.indices.CopyTo(this.indices = new ushort[this.indices.Length * 2], 0);
            this.indices[this.indexCount] = index;
            return this.indexCount++;
        }

        public int AddIndices(ushort[] indices)
        {
            int ret = this.indexCount;
            if (this.indices.Length <= this.indexCount + indices.Length)
                this.indices.CopyTo(this.indices = new ushort[Math.Max(this.indices.Length * 2, this.indexCount + indices.Length)], 0);
            Array.Copy(indices, 0, this.indices, this.indexCount, indices.Length);
            this.indexCount += indices.Length;
            return ret;
        }

        protected override void onNewShaderProgram()
        {
            int[] buffers = new int[] { this.vertexBuffer, this.indexBuffer };

            //this.initBuffers(ref buffers);

            //this.vertexBuffer = buffers[0];
            this.indexBuffer = buffers[1];

            this.setVertexAttributes();
        }

        protected override void render()
        {
            GL.UseProgram(this.program);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBuffer);
            //GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.vertexSize * this.vertexCount), this.vertices, BufferUsageHint.StreamDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(this.indexBytes * this.indexCount), this.indices, BufferUsageHint.StreamDraw);

            GL.DrawElements(this.beginMode, this.indexCount, DrawElementsType.UnsignedShort, 0);
        }

        public override void Clear()
        {
            base.Clear();
            this.indexCount = 0;
        }
    }
}
