using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Graphics.Vertices;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Rendering
{
    public static partial class Renderable
    {
        public static IBatchedRenderable ForBatchedVertices<TV>(Batcher<Buffer<TV>> batcher, PrimitiveType primitiveType)
            where TV : struct, IVertexData
        {
            return new WithBatched<Buffer<TV>>(batcher, buffer => Build(primitiveType, b => b.With(buffer.AsVertexBuffer())));
        }

        public static IBatchedRenderable ForBatchedVertices<TV>(Batcher<BufferStream<TV>> batcher, PrimitiveType primitiveType)
            where TV : struct, IVertexData
        {
            return new WithBatched<BufferStream<TV>>(batcher, stream => Build(primitiveType, b => b.With(stream.AsVertexBuffer())));
        }

        public static IBatchedRenderable ForBatchedVerticesAndIndices<TV>(
            Batcher<(Buffer<TV>, Buffer<ushort>)> batcher, PrimitiveType primitiveType)
            where TV : struct, IVertexData
        {
            return new WithBatched<(Buffer<TV> Vertices, Buffer<ushort> Indices)>(batcher,
                buffers => Build(primitiveType, b => b
                    .With(buffers.Vertices.AsVertexBuffer())
                    .With(buffers.Indices.AsIndexBuffer())
                ));
        }

        public static IBatchedRenderable ForBatchedVerticesAndIndices<TV, TBatchData>(
            Batcher<TBatchData> batcher, Func<TBatchData, (Buffer<TV>, Buffer<ushort>)> bufferSelector,
            PrimitiveType primitiveType)
            where TV : struct, IVertexData
        {
            return new WithBatched<TBatchData>(batcher,
                batch =>
                {
                    var (vb, ib) = bufferSelector(batch);
                    return Build(primitiveType, b => b
                        .With(vb.AsVertexBuffer())
                        .With(ib.AsIndexBuffer()));
                });
        }

        public static IBatchedRenderable ForBatchedVerticesAndIndices<TV>(
            Batcher<(BufferStream<TV>, BufferStream<ushort>)> batcher, PrimitiveType primitiveType)
            where TV : struct, IVertexData
        {
            return new WithBatched<(BufferStream<TV>, BufferStream<ushort>)>(batcher,
                buffers => Build(primitiveType, b => b
                    .With(buffers.Item1.AsVertexBuffer())
                    .With(buffers.Item2.AsIndexBuffer())
                ));
        }

        public static IBatchedRenderable ForBatchedVerticesAndIndices<TV, TBatchData>(
            Batcher<TBatchData> batcher, Func<TBatchData, (BufferStream<TV>, BufferStream<ushort>)> bufferSelector,
            PrimitiveType primitiveType)
            where TV : struct, IVertexData
        {
            return new WithBatched<TBatchData>(batcher,
                batch =>
                {
                    var (vb, ib) = bufferSelector(batch);
                    return Build(primitiveType, b => b
                        .With(vb.AsVertexBuffer())
                        .With(ib.AsIndexBuffer())
                    );
                });
        }

        private sealed class WithBatched<TBatchData> : IBatchedRenderable
        {
            private readonly Batcher<TBatchData> batcher;
            private readonly Func<TBatchData, IRenderable> createRenderable;

            private readonly Dictionary<Batcher<TBatchData>.Batch, IRenderable> renderables = new();

            public event Action<IRenderable>? BatchActivated;
            public event Action<IRenderable>? BatchDeactivated;

            public WithBatched(
                Batcher<TBatchData> batcher,
                Func<TBatchData, IRenderable> createRenderable)
            {
                this.batcher = batcher;
                this.createRenderable = createRenderable;

                batcher.BatchActivated += onBatchActivated;
                batcher.BatchDeactivated += onBatchDeactivated;

                foreach (var batch in batcher.ActiveBatches)
                {
                    getOrCreateRenderableFor(batch);
                }
            }

            private void onBatchActivated(Batcher<TBatchData>.Batch batch)
            {
                var renderable = getOrCreateRenderableFor(batch);

                BatchActivated?.Invoke(renderable);
            }

            private IRenderable getOrCreateRenderableFor(Batcher<TBatchData>.Batch batch)
            {
                if (!renderables.TryGetValue(batch, out var renderable))
                {
                    renderable = createRenderable(batch.Data);
                    renderables.Add(batch, renderable);
                }

                return renderable;
            }

            private void onBatchDeactivated(Batcher<TBatchData>.Batch batch)
            {
                BatchDeactivated?.Invoke(renderables[batch]);
            }

            public IEnumerable<IRenderable> GetActiveBatches()
            {
                return batcher.ActiveBatches.Select(batch => renderables[batch]);
            }
        }
    }
}
