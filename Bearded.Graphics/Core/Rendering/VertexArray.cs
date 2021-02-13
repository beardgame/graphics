using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Rendering
{
    public sealed class VertexArray : IDisposable
    {
        private readonly int handle;

        public VertexArray()
        {
            GL.GenVertexArrays(1, out handle);
        }

        public Bound Bind()
        {
            return new Bound(in handle);
        }

        public struct Bound : IDisposable
        {
            internal Bound(in int handle)
            {
                GL.BindVertexArray(handle);
            }

            public void Dispose()
            {
                GL.BindVertexArray(0);
            }
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(handle);
        }
    }
}
