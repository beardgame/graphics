namespace amulware.Graphics.Pipelines.Context
{
    sealed class DepthModeChange : ContextChange<DepthMode>
    {
        public DepthModeChange(DepthMode newValue) : base(newValue)
        {
        }

        protected override DepthMode GetCurrent() => GLState.DepthMode;

        protected override void Set(DepthMode value) => GLState.SetDepthMode(value);
    }
}
