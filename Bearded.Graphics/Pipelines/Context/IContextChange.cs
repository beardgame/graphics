namespace Bearded.Graphics.Pipelines.Context
{
    public interface IContextChange<in TState>
    {
        void StoreCurrentValueAndApplyChange(TState state);
        void RestoreToStoredValue();
    }
}
