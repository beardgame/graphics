using System;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{

    public class VertexSurface<VertexData> : StaticVertexSurface<VertexData> where VertexData : struct, IVertexData
    {
        public bool ClearOnRender = true;

        public bool IsStatic
        {
            get { return this.isStatic; }
            set { this.isStatic = value; }
        }

        public VertexSurface(BeginMode primitiveType = BeginMode.Triangles)
            : base(primitiveType)
        {
            this.isStatic = false;
            this.vertices = new VertexData[4];
        }

        public ushort AddVertex(VertexData vertex)
        {
            if (this.vertices.Length == this.vertexCount)
                // not that it matters, but the selfmade copying may be insignificantly slower(not tested)
                //this.vertices.CopyTo(this.vertices = new VertexData[this.vertices.Length * 2], 0);
                Array.Resize<VertexData>(ref this.vertices, this.vertices.Length * 2);
            this.vertices[this.vertexCount] = vertex;
            return this.vertexCount++;
        }

        public ushort AddVertices(VertexData[] vertices)
        {
            ushort ret = this.vertexCount;
            if (this.vertices.Length <= this.vertexCount + vertices.Length)
                this.vertices.CopyTo(this.vertices = new VertexData[Math.Max(this.vertices.Length * 2, this.vertexCount + vertices.Length)], 0);
            Array.Copy(vertices, 0, this.vertices, this.vertexCount, vertices.Length);
            this.vertexCount += (ushort)vertices.Length;
            return ret;
        }

        protected override void render()
        {
            base.render();
            if (this.ClearOnRender)
                this.Clear();
        }

        public virtual void Clear()
        {
            this.vertexCount = 0;
            this.staticBufferUploaded = false;
        }
    }
}
