using System.Collections.Immutable;
using Bearded.Graphics.Pipelines.Context;

namespace Bearded.Graphics.Pipelines.Steps
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

            for (var i = changes.Length - 1; i >= 0; i--)
            {
                var change = changes[i];
                change.RestoreToStoredValue();
            }
        }
    }
}
