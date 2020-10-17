using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using amulware.Graphics.Pipelines.Context;

namespace amulware.Graphics.Pipelines
{
    public class PipelineContextBuilder
    {
        private readonly List<IContextChange> contextChanges = new List<IContextChange>();

        public PipelineContextBuilder BindRenderTarget(PipelineRenderTarget renderTarget)
        {
            contextChanges.Add(new FramebufferContextChange(renderTarget));
            return this;
        }

        public PipelineContextBuilder SetDepthMode(DepthMode depthMode)
        {
            contextChanges.Add(new DepthModeChange(depthMode));
            return this;
        }

        public PipelineContextBuilder SetBlendMode(BlendMode blendMode)
        {
            contextChanges.Add(new BlendModeChange(blendMode));
            return this;
        }

        public PipelineContextBuilder SetViewport(Func<Rectangle> getViewport)
        {
            contextChanges.Add(new Viewport(getViewport));
            return this;
        }

        public ImmutableArray<IContextChange> Build() => contextChanges.ToImmutableArray();
    }
}
