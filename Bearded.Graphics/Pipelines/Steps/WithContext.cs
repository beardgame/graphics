using System.Collections.Immutable;
using amulware.Graphics.Pipelines.Context;

namespace amulware.Graphics.Pipelines.Steps
{
    sealed class WithContext<TState> : IPipeline<TState>
    {
        private readonly ImmutableArray<IContextChange<TState>> changes;
        private readonly IPipeline<TState> inner;

        public WithContext(ImmutableArray<IContextChange<TState>> changes, IPipeline<TState> inner)
        {
            this.changes = changes;
            this.inner = inner;
        }

        public void Execute(TState state)
        {
            foreach (var change in changes)
            {
                change.StoreCurrentValueAndApplyChange(state);
            }

            inner.Execute(state);

            foreach (var change in changes)
            {
                change.RestoreToStoredValue();
            }
        }
    }
}
