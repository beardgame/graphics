namespace amulware.Graphics.Pipelines.Context
{
    class FramebufferContextChange : ContextChange<int>
    {
        public FramebufferContextChange(PipelineRenderTarget target)
            : base(target.Handle)
        {
        }

        protected override int GetCurrent() => GLState.Framebuffer;

        protected override void Set(int value) => GLState.BindFramebuffer(value);
    }
}
