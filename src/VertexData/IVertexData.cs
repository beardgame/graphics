using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public interface IVertexData
    {
        VertexAttribute[] VertexAttributes();

        /// <summary>
        /// This method returns the size of the vertex data struct in bytes
        /// </summary>
        /// <returns>Struct's size in bytes</returns>
        int Size();
    }
}
