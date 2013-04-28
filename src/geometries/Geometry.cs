using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    /// <summary>
    /// Abstract base class for all geometries.
    /// </summary>
    /// <typeparam name="VertexData">The type of Vertex data used. Must match the surfaces of the geometry.</typeparam>
    public abstract class Geometry<VertexData> where VertexData : struct, IVertexData
    {
        /// <summary>
        /// The surface this geometry uses to draw.
        /// </summary>
        public VertexSurface<VertexData> Surface { private set; get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Geometry{VertexData}"/> class.
        /// </summary>
        /// <param name="surface">The surface this geometry uses to draw.</param>
        public Geometry(VertexSurface<VertexData> surface)
        {
            this.Surface = surface;
        }
        
    }
}
