using System;
using Bearded.Graphics.Textures;

namespace Bearded.Graphics.Pipelines
{
    public class PipelineRenderTarget : IDisposable
    {
        public int Handle => renderTarget.Handle;

        private readonly RenderTarget renderTarget;

        public PipelineRenderTarget(RenderTarget renderTarget)
        {
            this.renderTarget = renderTarget;
        }

        public void Dispose()
        {
            renderTarget.Dispose();
        }
    }
}
