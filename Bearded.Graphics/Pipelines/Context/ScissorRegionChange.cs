using System;

namespace Bearded.Graphics.Pipelines.Context
{
    sealed class ScissorRegionChange<TState> : ContextChange<TState, ScissorRegion>
    {
        public ScissorRegionChange(Func<TState, ScissorRegion> getNewValue) : base(getNewValue)
        {
        }

        protected override ScissorRegion GetCurrent() => GLState.ScissorRegion;

        protected override void Set(ScissorRegion value) => GLState.SetScissorRegion(value);
    }
}
