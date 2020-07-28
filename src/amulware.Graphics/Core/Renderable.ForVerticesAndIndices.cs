using System;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public static partial class Renderable
    {
        public static IRenderable ForVerticesAndIndices<T>(
            Buffer<T> vertexBuffer, Buffer<byte> indexBuffer, PrimitiveType primitiveType)
            where T : struct, IVertexData
        {
            return new WithVerticesAndIndices<T, byte>(vertexBuffer, indexBuffer, primitiveType);
        }

        public static IRenderable ForVerticesAndIndices<T>(
            Buffer<T> vertexBuffer, Buffer<uint> indexBuffer, PrimitiveType primitiveType)
            where T : struct, IVertexData
        {
            return new WithVerticesAndIndices<T, uint>(vertexBuffer, indexBuffer, primitiveType);
        }

        public static IRenderable ForVerticesAndIndices<T>(
            Buffer<T> vertexBuffer, Buffer<ushort> indexBuffer, PrimitiveType primitiveType)
            where T : struct, IVertexData
        {
            return new WithVerticesAndIndices<T, ushort>(vertexBuffer, indexBuffer, primitiveType);
        }

        public static IRenderable ForVerticesAndIndices<T>(
            BufferStream<T> vertexBuffer, BufferStream<byte> indexBuffer, PrimitiveType primitiveType)
            where T : struct, IVertexData
        {
            return new WithVerticesAndIndicesStreaming<T, byte>(vertexBuffer, indexBuffer, primitiveType);
        }

        public static IRenderable ForVerticesAndIndices<T>(
            BufferStream<T> vertexBuffer, BufferStream<uint> indexBuffer, PrimitiveType primitiveType)
            where T : struct, IVertexData
        {
            return new WithVerticesAndIndicesStreaming<T, uint>(vertexBuffer, indexBuffer, primitiveType);
        }

        public static IRenderable ForVerticesAndIndices<T>(
            BufferStream<T> vertexBuffer, BufferStream<ushort> indexBuffer, PrimitiveType primitiveType)
            where T : struct, IVertexData
        {
            return new WithVerticesAndIndicesStreaming<T, ushort>(vertexBuffer, indexBuffer, primitiveType);
        }

        private sealed class WithVerticesAndIndicesStreaming<TVertex, TIndex> : IRenderable
            where TVertex : struct, IVertexData
            where TIndex : struct
        {
            private readonly BufferStream<TVertex> vertexBufferStream;
            private readonly BufferStream<TIndex> indexBufferStream;
            private readonly WithVerticesAndIndices<TVertex, TIndex> bufferRenderable;

            public WithVerticesAndIndicesStreaming(BufferStream<TVertex> vertexBufferStream,
                BufferStream<TIndex> indexBufferStream,
                PrimitiveType primitiveType)
            {
                this.vertexBufferStream = vertexBufferStream;
                this.indexBufferStream = indexBufferStream;
                bufferRenderable = new WithVerticesAndIndices<TVertex, TIndex>(
                    vertexBufferStream.Buffer, indexBufferStream.Buffer, primitiveType);
            }

            public void ConfigureBoundVertexArray(ShaderProgram program)
            {
                bufferRenderable.ConfigureBoundVertexArray(program);
            }

            public void Render()
            {
                vertexBufferStream.UploadIfDirty();
                indexBufferStream.UploadIfDirty();
                bufferRenderable.Render();
            }
        }

        private sealed class WithVerticesAndIndices<TVertex, TIndex> : IRenderable
            where TVertex : struct, IVertexData
            where TIndex : struct
        {
            private static readonly DrawElementsType drawElementsType =
                default(TIndex) switch
                {
                    byte _ => DrawElementsType.UnsignedByte,
                    ushort _ => DrawElementsType.UnsignedShort,
                    uint _ => DrawElementsType.UnsignedInt,
                    _ => throw new NotSupportedException()
                };

            private readonly Buffer<TVertex> vertexBuffer;
            private readonly Buffer<TIndex> indexBuffer;
            private readonly PrimitiveType primitiveType;

            public WithVerticesAndIndices(Buffer<TVertex> vertexBuffer, Buffer<TIndex> indexBuffer,
                PrimitiveType primitiveType)
            {
                this.vertexBuffer = vertexBuffer;
                this.indexBuffer = indexBuffer;
                this.primitiveType = primitiveType;
            }

            public void ConfigureBoundVertexArray(ShaderProgram program)
            {
                using (vertexBuffer.Bind(BufferTarget.ArrayBuffer))
                {
                    VertexData.SetAttributes<TVertex>(program);
                }

                // TODO: is this right? can we unbind it somewhere?
                indexBuffer.Bind(BufferTarget.ElementArrayBuffer);
            }

            public void Render()
            {
                GL.DrawElements(primitiveType, indexBuffer.Count, drawElementsType, 0);
            }
        }
    }
}
