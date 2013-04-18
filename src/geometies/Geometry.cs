using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public abstract class Geometry<VertexData> where VertexData : struct, IVertexData
    {
        public VertexSurface<VertexData> Surface { private set; get; }

        public Geometry(VertexSurface<VertexData> surface)
        {
            this.Surface = surface;
        }
        
    }
}
