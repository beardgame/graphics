namespace Bearded.Graphics.Pipelines.Context
{
    sealed class BlendModeChange<TState> : ContextChange<TState, BlendMode>
    {
        public BlendModeChange(BlendMode newValue) : base(newValue)
        {
        }

        protected override BlendMode GetCurrent() => GLState.BlendMode;

        protected override void Set(BlendMode value) => GLState.SetBlendMode(value);
    }
}
