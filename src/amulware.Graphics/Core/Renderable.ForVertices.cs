using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
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

        private sealed class WithVerticesStreaming<TVertex> : IRenderable
            where TVertex : struct, IVertexData
        {
            private readonly BufferStream<TVertex> vertexBufferStream;
            private readonly WithVertices<TVertex> bufferRenderable;

            public WithVerticesStreaming(BufferStream<TVertex> vertexBufferStream, PrimitiveType primitiveType)
            {
                this.vertexBufferStream = vertexBufferStream;
                bufferRenderable = new WithVertices<TVertex>(vertexBufferStream.Buffer, primitiveType);
            }

            public void ConfigureBoundVertexArray(ShaderProgram program)
            {
                bufferRenderable.ConfigureBoundVertexArray(program);
            }

            public void Render()
            {
                vertexBufferStream.UploadIfDirty();
                bufferRenderable.Render();
            }
        }

        private sealed class WithVertices<TVertex> : IRenderable
            where TVertex : struct, IVertexData
        {
            private readonly Buffer<TVertex> vertexBuffer;
            private readonly PrimitiveType primitiveType;

            public WithVertices(Buffer<TVertex> vertexBuffer, PrimitiveType primitiveType)
            {
                this.vertexBuffer = vertexBuffer;
                this.primitiveType = primitiveType;
            }

            public void ConfigureBoundVertexArray(ShaderProgram program)
            {
                using var _ = vertexBuffer.Bind(BufferTarget.ArrayBuffer);
                VertexData.SetAttributes<TVertex>(program);
            }

            public void Render()
            {
                GL.DrawArrays(primitiveType, 0, vertexBuffer.Count);
            }
        }
    }
}
