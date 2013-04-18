using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class QuadSurface<VertexData> : VertexSurface<VertexData>
        where VertexData : struct, IVertexData
    {
        public QuadSurface() : base(BeginMode.Quads) { }

        public void AddQuad(VertexData v0, VertexData v1, VertexData v2, VertexData v3)
        {
            this.AddVertices(new VertexData[] { v0, v1, v2, v3 });
        }
    }
}
