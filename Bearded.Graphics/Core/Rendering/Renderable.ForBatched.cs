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
            return new WithBatched<Buffer<TV>>(batcher, primitiveType, ForVertices);
        }

        public static IBatchedRenderable ForBatchedVertices<TV>(Batcher<BufferStream<TV>> batcher, PrimitiveType primitiveType)
            where TV : struct, IVertexData
        {
            return new WithBatched<BufferStream<TV>>(batcher, primitiveType, ForVertices);
        }

        public static IBatchedRenderable ForBatchedVerticesAndIndices<TV>(
            Batcher<(Buffer<TV>, Buffer<ushort>)> batcher, PrimitiveType primitiveType)
            where TV : struct, IVertexData
        {
            return new WithBatched<(Buffer<TV>, Buffer<ushort>)>(batcher, primitiveType,
                (buffers, pt) => ForVerticesAndIndices(buffers.Item1, buffers.Item2, pt));
        }

        public static IBatchedRenderable ForBatchedVerticesAndIndices<TV, TBatchData>(
            Batcher<TBatchData> batcher, Func<TBatchData, (Buffer<TV>, Buffer<ushort>)> bufferSelector,
            PrimitiveType primitiveType)
            where TV : struct, IVertexData
        {
            return new WithBatched<TBatchData>(batcher, primitiveType,
                (batch, pt) =>
                {
                    var (vb, ib) = bufferSelector(batch);
                    return ForVerticesAndIndices(vb, ib, pt);
                });
        }

        public static IBatchedRenderable ForBatchedVerticesAndIndices<TV>(
            Batcher<(BufferStream<TV>, BufferStream<ushort>)> batcher, PrimitiveType primitiveType)
            where TV : struct, IVertexData
        {
            return new WithBatched<(BufferStream<TV>, BufferStream<ushort>)>(batcher, primitiveType,
                (buffers, pt) => ForVerticesAndIndices(buffers.Item1, buffers.Item2, pt));
        }

        public static IBatchedRenderable ForBatchedVerticesAndIndices<TV, TBatchData>(
            Batcher<TBatchData> batcher, Func<TBatchData, (BufferStream<TV>, BufferStream<ushort>)> bufferSelector,
            PrimitiveType primitiveType)
            where TV : struct, IVertexData
        {
            return new WithBatched<TBatchData>(batcher, primitiveType,
                (batch, pt) =>
                {
                    var (vb, ib) = bufferSelector(batch);
                    return ForVerticesAndIndices(vb, ib, pt);
                });
        }

        private sealed class WithBatched<TBatchData> : IBatchedRenderable
        {
            private readonly Batcher<TBatchData> batcher;
            private readonly PrimitiveType primitiveType;
            private readonly Func<TBatchData, PrimitiveType, IRenderable> createRenderable;

            private readonly Dictionary<Batcher<TBatchData>.Batch, IRenderable> renderables
                = new Dictionary<Batcher<TBatchData>.Batch, IRenderable>();

            public event Action<IRenderable>? BatchActivated;
            public event Action<IRenderable>? BatchDeactivated;

            public WithBatched(
                Batcher<TBatchData> batcher,
                PrimitiveType primitiveType,
                Func<TBatchData, PrimitiveType, IRenderable> createRenderable)
            {
                this.batcher = batcher;
                this.primitiveType = primitiveType;
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
                    renderable = createRenderable(batch.Data, primitiveType);
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
