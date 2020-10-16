namespace amulware.Graphics.Pipelines.Context
{
    public interface IContextChange
    {
        void StoreCurrentValueAndApplyChange();
        void RestoreToStoredValue();
    }
}