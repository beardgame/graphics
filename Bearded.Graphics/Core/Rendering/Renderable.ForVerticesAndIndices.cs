using System;
using Bearded.Graphics.Shading;
using Bearded.Graphics.Vertices;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Rendering
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

        private sealed class WithVerticesAndIndicesStreaming<TVertex, TIndex> : WithVerticesAndIndices<TVertex, TIndex>
            where TVertex : struct, IVertexData
            where TIndex : struct
        {
            private readonly BufferStream<TVertex> vertexBufferStream;
            private readonly BufferStream<TIndex> indexBufferStream;

            public WithVerticesAndIndicesStreaming(BufferStream<TVertex> vertexBufferStream,
                BufferStream<TIndex> indexBufferStream,
                PrimitiveType primitiveType)
                : base(vertexBufferStream.Buffer, indexBufferStream.Buffer, primitiveType)
            {
                this.vertexBufferStream = vertexBufferStream;
                this.indexBufferStream = indexBufferStream;
            }

            protected override void Render()
            {
                vertexBufferStream.FlushIfDirty();
                indexBufferStream.FlushIfDirty();
                base.Render();
            }
        }

        private class WithVerticesAndIndices<TVertex, TIndex> : BaseRenderable
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

            protected override void ConfigureBoundVertexArray(ShaderProgram program)
            {
                using (vertexBuffer.Bind(BufferTarget.ArrayBuffer))
                {
                    VertexData.SetAttributes<TVertex>(program);
                }

                indexBuffer.Bind(BufferTarget.ElementArrayBuffer);
            }

            protected override void Render()
            {
                GL.DrawElements(primitiveType, indexBuffer.Count, drawElementsType, 0);
            }
        }
    }
}
