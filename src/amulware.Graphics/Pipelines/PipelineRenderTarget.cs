using amulware.Graphics.Textures;

namespace amulware.Graphics.Pipelines
{
    public class PipelineRenderTarget
    {
        public int Handle => renderTarget.Handle;

        private readonly RenderTarget renderTarget;

        public PipelineRenderTarget(RenderTarget renderTarget)
        {
            this.renderTarget = renderTarget;
        }
    }
}