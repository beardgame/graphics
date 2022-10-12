using System;
using Bearded.Graphics.Textures;

namespace Bearded.Graphics.Pipelines.Context
{
    sealed class FramebufferChange<TState> : ContextChange<TState, int>
    {
        public FramebufferChange(PipelineRenderTarget target)
            : base(target.Handle)
        {
        }

        public FramebufferChange(Func<TState, RenderTarget> getTarget)
            : base(s => getTarget(s).Handle)
        {
        }

        protected override int GetCurrent() => GLState.Framebuffer;

        protected override void Set(int value) => GLState.BindFramebuffer(value);
    }
}
