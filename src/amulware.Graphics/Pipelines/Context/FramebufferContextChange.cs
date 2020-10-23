using System;
using amulware.Graphics.Textures;

namespace amulware.Graphics.Pipelines.Context
{
    class FramebufferContextChange<TState> : ContextChange<TState, int>
    {
        public FramebufferContextChange(PipelineRenderTarget target)
            : base(target.Handle)
        {
        }

        public FramebufferContextChange(Func<TState, RenderTarget> getTarget)
            : base(s => getTarget(s).Handle)
        {
        }

        protected override int GetCurrent() => GLState.Framebuffer;

        protected override void Set(int value) => GLState.BindFramebuffer(value);
    }
}
