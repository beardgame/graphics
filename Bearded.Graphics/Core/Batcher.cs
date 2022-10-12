using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bearded.Graphics
{
    public sealed class Batcher<TBatchData> : IDisposable
    {
        private readonly Func<TBatchData> factory;

        public sealed class Batch
        {
            public TBatchData Data { get; }

            public Batch(TBatchData data)
            {
                Data = data;
            }
        }

        private readonly List<Batch> activeBatches = new();
        private readonly Stack<Batch> inactiveBatches = new();

        public event Action<Batch>? BatchActivated;
        public event Action<Batch>? BatchDeactivated;

        public ReadOnlyCollection<Batch> ActiveBatches { get; }

        public Batcher(Func<TBatchData> factory)
        {
            ActiveBatches = activeBatches.AsReadOnly();
            this.factory = factory;
        }

        public Batch AllocateBatch()
        {
            var batch = inactiveBatches.TryPop(out var b)
                ? b
                : createBatch();

            activeBatches.Add(batch);

            BatchActivated?.Invoke(batch);

            return batch;
        }

        private Batch createBatch()
        {
            return new Batch(factory());
        }

        public void Free(Batch batch)
        {
            if (!activeBatches.Remove(batch))
                throw new InvalidOperationException(
                    $"Can only free batches currently allocated with {nameof(AllocateBatch)}.");

            inactiveBatches.Push(batch);

            BatchDeactivated?.Invoke(batch);
        }

        public void FreeAll()
        {
            foreach (var batch in activeBatches)
            {
                inactiveBatches.Push(batch);
                BatchDeactivated?.Invoke(batch);
            }

            activeBatches.Clear();
        }

        public void Dispose()
        {
            dispose(activeBatches);
            dispose(inactiveBatches);
            activeBatches.Clear();
            inactiveBatches.Clear();
        }

        private static void dispose(IEnumerable<Batch> batches)
        {
            foreach (var batch in batches)
            {
                if (batch.Data is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}
