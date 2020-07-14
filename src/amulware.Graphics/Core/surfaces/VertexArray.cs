using System;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public sealed class VertexArray : IDisposable
    {
        private readonly int handle;
        private readonly IRenderable renderable;

        public static VertexArray For(IRenderable renderable, ShaderProgram program)
        {
            return new VertexArray(renderable, program);
        }

        private VertexArray(IRenderable renderable, ShaderProgram program)
        {
            this.renderable = renderable;
            GL.GenVertexArrays(1, out handle);
            using (Bind())
            {
                renderable.ConfigureBoundVertexArray(program);
            }
        }

        // TODO: should this be in a separate RenderState class?
        public void Render()
        {
            using (Bind())
            {
                renderable.Render();
            }
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
