using OpenTK.Graphics.OpenGL;
using System;

namespace amulware.Graphics.surfaces
{
    class IndexedQuadSurface<TVertexData> : IndexedSurface<TVertexData>
        where TVertexData : struct, IVertexData
    {
        public IndexedQuadSurface()
            : base(BeginMode.Triangles)
        {

        }

        /// <summary>
        /// Adds a quad of vertices and automatically adds the correct indices.
        /// </summary>
        /// <remarks>The vertices are expected in clockwise or anticlockwise order.</remarks>
        /// <param name="v0">The first vertex.</param>
        /// <param name="v1">The second vertex.</param>
        /// <param name="v2">The third vertex.</param>
        /// <param name="v3">The fourth vertex.</param>
        public void AddQuad(TVertexData v0, TVertexData v1, TVertexData v2, TVertexData v3, bool alternateTriangulation = false)
        {
            ushort i = this.vertexBuffer.AddVertices(v0, v1, v2, v3);

            ushort t0v3;
            ushort t1v3;

            if (alternateTriangulation)
            {
                t0v3 = (ushort)(i + 2);
                t1v3 = i;
            }
            else
            {
                t0v3 = (ushort)(i + 3);
                t1v3 = (ushort)(i + 1);
            }


            this.indexBuffer.AddIndices(
                i, (ushort)(i + 1), t0v3,
                (ushort)(i + 2), (ushort)(i + 3), t1v3);
        }
    }
}
