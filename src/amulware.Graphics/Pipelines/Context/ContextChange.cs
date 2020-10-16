namespace amulware.Graphics.Pipelines.Context
{
    abstract class ContextChange<T> : IContextChange
    {
        private readonly T newValue;
        // We force the default value here because
        // this should always be set in StoreCurrentValueAndApplyChange
        // before it is used in RestoreToStoredValue
        private T previousValue = default!;

        protected ContextChange(T newValue)
        {
            this.newValue = newValue;
        }

        public void StoreCurrentValueAndApplyChange()
        {
            previousValue = GetCurrent();
            Set(newValue);
        }

        public void RestoreToStoredValue()
        {
            Set(previousValue);
        }

        protected abstract T GetCurrent();

        protected abstract void Set(T value);
    }
}
