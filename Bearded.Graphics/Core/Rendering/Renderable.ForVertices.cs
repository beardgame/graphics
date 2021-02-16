using Bearded.Graphics.Shading;
using Bearded.Graphics.Vertices;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Rendering
{
    public static partial class Renderable
    {
        public static IRenderable ForVertices<T>(Buffer<T> vertexBuffer, PrimitiveType primitiveType)
            where T : struct, IVertexData
        {
            return new WithVertices<T>(vertexBuffer, primitiveType);
        }

        public static IRenderable ForVertices<T>(BufferStream<T> vertexBufferStream, PrimitiveType primitiveType)
            where T : struct, IVertexData
        {
            return new WithVerticesStreaming<T>(vertexBufferStream, primitiveType);
        }

        private sealed class WithVerticesStreaming<TVertex> : WithVertices<TVertex>
            where TVertex : struct, IVertexData
        {
            private readonly BufferStream<TVertex> vertexBufferStream;

            public WithVerticesStreaming(BufferStream<TVertex> vertexBufferStream, PrimitiveType primitiveType)
                : base(vertexBufferStream.Buffer, primitiveType)
            {
                this.vertexBufferStream = vertexBufferStream;
            }

            protected override void Render()
            {
                vertexBufferStream.FlushIfDirty();
                base.Render();
            }
        }

        private class WithVertices<TVertex> : BaseRenderable
            where TVertex : struct, IVertexData
        {
            private readonly Buffer<TVertex> vertexBuffer;
            private readonly PrimitiveType primitiveType;

            public WithVertices(Buffer<TVertex> vertexBuffer, PrimitiveType primitiveType)
            {
                this.vertexBuffer = vertexBuffer;
                this.primitiveType = primitiveType;
            }

            protected override void ConfigureBoundVertexArray(ShaderProgram program)
            {
                using var _ = vertexBuffer.Bind(BufferTarget.ArrayBuffer);
                VertexData.SetAttributes<TVertex>(program);
            }

            protected override void Render()
            {
                GL.DrawArrays(primitiveType, 0, vertexBuffer.Count);
            }
        }

        private abstract class BaseRenderable : IRenderable
        {
            public DrawCall MakeDrawCallFor(ShaderProgram program)
            {
                return DrawCall.With(() => ConfigureBoundVertexArray(program), Render);
            }

            protected abstract void ConfigureBoundVertexArray(ShaderProgram program);

            protected abstract void Render();
        }
    }
}
