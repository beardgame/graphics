using System;

namespace amulware.Graphics.Pipelines.Context
{
    abstract class ContextChange<T> : IContextChange
    {
        private readonly Func<T> getNewValue;
        // We force the default value here because
        // this should always be set in StoreCurrentValueAndApplyChange
        // before it is used in RestoreToStoredValue
        private T previousValue = default!;

        protected ContextChange(Func<T> getNewValue)
        {
            this.getNewValue = getNewValue;
        }

        protected ContextChange(T newValue)
        {
            getNewValue = () => newValue;
        }

        public void StoreCurrentValueAndApplyChange()
        {
            previousValue = GetCurrent();
            Set(getNewValue());
        }

        public void RestoreToStoredValue()
        {
            Set(previousValue);
        }

        protected abstract T GetCurrent();

        protected abstract void Set(T value);
    }
}
