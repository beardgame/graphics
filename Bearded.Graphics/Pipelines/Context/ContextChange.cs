using System;

namespace Bearded.Graphics.Pipelines.Context
{
    abstract class ContextChange<TState, T> : IContextChange<TState>
    {
        private readonly Func<TState, T> getNewValue;
        // We force the default value here because
        // this should always be set in StoreCurrentValueAndApplyChange
        // before it is used in RestoreToStoredValue
        private T previousValue = default!;

        protected ContextChange(Func<TState, T> getNewValue)
        {
            this.getNewValue = getNewValue;
        }

        protected ContextChange(T newValue)
        {
            getNewValue = _ => newValue;
        }

        public void StoreCurrentValueAndApplyChange(TState state)
        {
            previousValue = GetCurrent();
            Set(getNewValue(state));
        }

        public void RestoreToStoredValue()
        {
            Set(previousValue);
        }

        protected abstract T GetCurrent();

        protected abstract void Set(T value);
    }
}
