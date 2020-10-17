using System;
using System.Drawing;

namespace amulware.Graphics.Pipelines.Context
{
    sealed class Viewport : ContextChange<Rectangle>
    {
        public Viewport(Func<Rectangle> getViewport)
            : base(getViewport)
        {
        }

        protected override Rectangle GetCurrent() => GLState.Viewport;

        protected override void Set(Rectangle value) => GLState.SetViewport(value);
    }
}
