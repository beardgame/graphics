using System.Collections.Immutable;
using amulware.Graphics.Pipelines.Context;

namespace amulware.Graphics.Pipelines.Steps
{
    sealed class WithContext : IPipeline
    {
        private readonly ImmutableArray<IContextChange> changes;
        private readonly IPipeline inner;

        public WithContext(ImmutableArray<IContextChange> changes, IPipeline inner)
        {
            this.changes = changes;
            this.inner = inner;
        }

        public void Execute()
        {
            foreach (var change in changes)
            {
                change.StoreCurrentValueAndApplyChange();
            }

            inner.Execute();

            foreach (var change in changes)
            {
                change.RestoreToStoredValue();
            }
        }
    }
}
