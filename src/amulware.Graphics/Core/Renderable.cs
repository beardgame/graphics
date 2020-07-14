using System;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public interface IRenderable
    {
        void ConfigureBoundVertexArray(ShaderProgram program);
        void Render();
    }

    public static class Renderable
    {
        public static IRenderable ForVertices<T>(Buffer<T> vertexBuffer, PrimitiveType primitiveType)
            where T : struct, IVertexData
        {
            return new WithVertices<T>(vertexBuffer, primitiveType);
        }

        public static IRenderable ForVerticesAndIndices<T>(Buffer<T> vertexBuffer, Buffer<byte> indexBuffer, PrimitiveType primitiveType)
            where T : struct, IVertexData
        {
            return new WithVerticesAndIndices<T, byte>(vertexBuffer, indexBuffer, primitiveType);
        }

        public static IRenderable ForVerticesAndIndices<T>(Buffer<T> vertexBuffer, Buffer<uint> indexBuffer, PrimitiveType primitiveType)
            where T : struct, IVertexData
        {
            return new WithVerticesAndIndices<T, uint>(vertexBuffer, indexBuffer, primitiveType);
        }

        public static IRenderable ForVerticesAndIndices<T>(Buffer<T> vertexBuffer, Buffer<ushort> indexBuffer, PrimitiveType primitiveType)
            where T : struct, IVertexData
        {
            return new WithVerticesAndIndices<T, ushort>(vertexBuffer, indexBuffer, primitiveType);
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
                vertexBuffer.Bind(BufferTarget.ArrayBuffer);
                VertexData.SetAttributes<TVertex>(program);
            }

            public void Render()
            {
                GL.DrawArrays(primitiveType, 0, vertexBuffer.Count);
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
                vertexBuffer.Bind(BufferTarget.ArrayBuffer);
                VertexData.SetAttributes<TVertex>(program);

                indexBuffer.Bind(BufferTarget.ElementArrayBuffer);
            }

            public void Render()
            {
                GL.DrawElements(primitiveType, indexBuffer.Count, drawElementsType, 0);

                // TODO: Do we need to implement clearing after rendering for streaming?
            }
        }
    }
}
