using System;
using amulware.Graphics.Shading;

namespace amulware.Graphics.Rendering
{
    public sealed class DrawCall : IDisposable
    {
        private readonly VertexArray vertexArray;
        private readonly Action drawCall;

        public static DrawCall For(IRenderable renderable, ShaderProgram program)
        {
            var vertexArray = new VertexArray();

            using (vertexArray.Bind())
            {
                renderable.ConfigureBoundVertexArray(program);
            }

            return new DrawCall(vertexArray, renderable.Render);
        }

        public DrawCall(VertexArray vertexArray, Action drawCall)
        {
            this.vertexArray = vertexArray;
            this.drawCall = drawCall;
        }

        public void Invoke()
        {
            using var _ = vertexArray.Bind();
            drawCall();
        }

        public void Dispose()
        {
            vertexArray.Dispose();
        }
    }
}
