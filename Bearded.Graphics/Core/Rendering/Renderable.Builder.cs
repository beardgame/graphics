using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bearded.Graphics.Shading;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Rendering;

public static partial class Renderable
{
    public static IRenderable Build(PrimitiveType primitiveType, Action<Builder> configure)
    {
        var builder = new Builder(primitiveType);
        configure(builder);
        return builder.Build();
    }

    public sealed class Builder(PrimitiveType primitiveType)
    {
        private readonly List<IVertexBuffer> vertexBuffers = [];
        private IIndexBuffer? indexBuffer;
        private Func<int>? instanceCount;

        public Builder With(IVertexBuffer buffer)
        {
            vertexBuffers.Add(buffer);
            return this;
        }

        public Builder With(params IVertexBuffer[] buffers)
        {
            vertexBuffers.AddRange(buffers);
            return this;
        }

        public Builder With(ReadOnlySpan<IVertexBuffer> buffers)
        {
            vertexBuffers.AddRange(buffers);
            return this;
        }

        public Builder With(IEnumerable<IVertexBuffer> buffers)
        {
            vertexBuffers.AddRange(buffers);
            return this;
        }

        public Builder With(IIndexBuffer buffer)
        {
            indexBuffer = buffer;
            return this;
        }

        public Builder InstancedWith(Func<int> getInstanceCount)
        {
            instanceCount = getInstanceCount;
            return this;
        }

        public IRenderable Build()
        {
            if (vertexBuffers.Count == 0)
                throw new InvalidOperationException("Renderable must have at least one vertex buffer.");

            return build(primitiveType, [..vertexBuffers], indexBuffer, instanceCount);
        }

        private static IRenderable build(
            PrimitiveType type,
            ImmutableArray<IVertexBuffer> vertices,
            IIndexBuffer? indices,
            Func<int>? instanceCount)
        {
            var flushables = listFlushableBuffers(vertices, indices);

            Action draw = (indices, instanceCount) switch
            {
                (null, null) => () => GL.DrawArrays(type, 0, vertices[0].Count),
                (null, not null) => () => GL.DrawArraysInstanced(type, 0, vertices[0].Count, instanceCount()),
                (not null, null) => () => GL.DrawElements(type, indices.Count, indices.ElementType, 0),
                (not null, not null) => () => GL.DrawElementsInstanced(type, indices.Count, indices.ElementType, 0, instanceCount()),
            };

            return new Implementation(configure, flushables.IsDefaultOrEmpty ? draw : flushAndDraw);

            void configure(ShaderProgram program)
            {
                foreach (var buffer in vertices)
                {
                    buffer.ConfigureBoundVertexArray(program);
                }

                indices?.ConfigureBoundVertexArray();
            }

            void flushAndDraw()
            {
                foreach (var flushable in flushables)
                {
                    flushable.FlushIfNeeded();
                }

                draw();
            }

        }

        private static ImmutableArray<IFlushableBuffer> listFlushableBuffers(
            ImmutableArray<IVertexBuffer> vertices, IIndexBuffer? indices)
        {
            var flushableCount = vertices.Count(b => b is IFlushableBuffer);
            flushableCount += indices is IFlushableBuffer ? 1 : 0;

            if (flushableCount == 0)
                return ImmutableArray<IFlushableBuffer>.Empty;

            var builder = ImmutableArray.CreateBuilder<IFlushableBuffer>(flushableCount);

            foreach (var buffer in vertices)
            {
                if (buffer is IFlushableBuffer flushable)
                {
                    builder.Add(flushable);
                }
            }
            if (indices is IFlushableBuffer indexFlushable)
            {
                builder.Add(indexFlushable);
            }

            return builder.MoveToImmutable();
        }
    }

    private sealed class Implementation(Action<ShaderProgram> configureBoundVertexArray, Action render) : IRenderable
    {
        public DrawCall MakeDrawCallFor(ShaderProgram program)
        {
            return DrawCall.With(() => configureBoundVertexArray(program), render);
        }
    }
}
