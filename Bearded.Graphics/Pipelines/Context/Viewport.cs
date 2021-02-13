using System;
using System.Drawing;

namespace Bearded.Graphics.Pipelines.Context
{
    sealed class Viewport<TState> : ContextChange<TState, Rectangle>
    {
        public Viewport(Func<TState, Rectangle> getViewport)
            : base(getViewport)
        {
        }

        protected override Rectangle GetCurrent() => GLState.Viewport;

        protected override void Set(Rectangle value) => GLState.SetViewport(value);
    }
}
