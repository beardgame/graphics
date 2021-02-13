namespace Bearded.Graphics.Pipelines.Context
{
    sealed class Culling<TState> : ContextChange<TState, CullMode>
    {
        public Culling(CullMode newValue) : base(newValue)
        {
        }

        protected override CullMode GetCurrent() => GLState.CullMode;

        protected override void Set(CullMode value) => GLState.SetCullMode(value);
    }
}
