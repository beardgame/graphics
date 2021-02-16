namespace Bearded.Graphics.Pipelines.Context
{
    sealed class DepthModeChange<TState> : ContextChange<TState, DepthMode>
    {
        public DepthModeChange(DepthMode newValue) : base(newValue)
        {
        }

        protected override DepthMode GetCurrent() => GLState.DepthMode;

        protected override void Set(DepthMode value) => GLState.SetDepthMode(value);
    }
}
