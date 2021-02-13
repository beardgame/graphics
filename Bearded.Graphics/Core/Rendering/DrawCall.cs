using System;

namespace Bearded.Graphics.Rendering
{
    public sealed class DrawCall : IDisposable
    {
        private readonly VertexArray vertexArray;
        private readonly Action drawCall;

        public static DrawCall With(Action configureBoundVertexArray, Action drawCall)
        {
            var vertexArray = new VertexArray();

            using (vertexArray.Bind())
            {
                configureBoundVertexArray();
            }

            return new DrawCall(vertexArray, drawCall);
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
