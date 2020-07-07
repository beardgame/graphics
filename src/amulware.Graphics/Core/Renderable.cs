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
        // TODO: probably need overloads for mutable buffers
        // - or is there any way to create an interface?
        public static IRenderable ForVertices<T>(Buffer<T> vertexBuffer)
            where T : struct, IVertexData
        {
            return new WithVertices<T>(vertexBuffer);
        }

        public static IRenderable ForVerticesAndIndices<T>(Buffer<T> vertexBuffer, Buffer<int> indexBuffer)
            where T : struct, IVertexData
        {
            return withVerticesAndIndices(vertexBuffer, indexBuffer);
        }

        public static IRenderable ForVerticesAndIndices<T>(Buffer<T> vertexBuffer, Buffer<uint> indexBuffer)
            where T : struct, IVertexData
        {
            return withVerticesAndIndices(vertexBuffer, indexBuffer);
        }

        public static IRenderable ForVerticesAndIndices<T>(Buffer<T> vertexBuffer, Buffer<short> indexBuffer)
            where T : struct, IVertexData
        {
            return withVerticesAndIndices(vertexBuffer, indexBuffer);
        }

        public static IRenderable ForVerticesAndIndices<T>(Buffer<T> vertexBuffer, Buffer<ushort> indexBuffer)
            where T : struct, IVertexData
        {
            return withVerticesAndIndices(vertexBuffer, indexBuffer);
        }

        private static WithVerticesAndIndices<TVertex, TIndex> withVerticesAndIndices<TVertex, TIndex>(
            Buffer<TVertex> vertexBuffer, Buffer<TIndex> indexBuffer)
            where TVertex : struct, IVertexData
            where TIndex : struct
        {
            return new WithVerticesAndIndices<TVertex, TIndex>(vertexBuffer, indexBuffer);
        }

        private class WithVertices<TVertex> : IRenderable
            where TVertex : struct, IVertexData
        {
            private readonly Buffer<TVertex> vertexBuffer;

            public WithVertices(Buffer<TVertex> vertexBuffer)
            {
                this.vertexBuffer = vertexBuffer;
            }

            public void ConfigureBoundVertexArray(ShaderProgram program)
            {
                vertexBuffer.Bind(BufferTarget.ArrayBuffer);
                VertexData.SetAttributes<TVertex>(program);
            }

            public void Render()
            {
                // TODO: somewhat unclear how to implement this
                // if we can get a reliable count from the buffer, that'll probably be all we need?
                // need to also inject primitive type
                throw new NotImplementedException();
            }
        }

        private class WithVerticesAndIndices<TVertex, TIndex> : IRenderable
            where TVertex : struct, IVertexData
            where TIndex : struct
        {
            private readonly Buffer<TVertex> vertexBuffer;
            private readonly Buffer<TIndex> indexBuffer;

            public WithVerticesAndIndices(Buffer<TVertex> vertexBuffer, Buffer<TIndex> indexBuffer)
            {
                this.vertexBuffer = vertexBuffer;
                this.indexBuffer = indexBuffer;
            }

            public void ConfigureBoundVertexArray(ShaderProgram program)
            {
                vertexBuffer.Bind(BufferTarget.ArrayBuffer);
                VertexData.SetAttributes<TVertex>(program);
                // todo: bind index buffer
            }

            public void Render()
            {
                throw new NotImplementedException();
            }
        }
    }
}
