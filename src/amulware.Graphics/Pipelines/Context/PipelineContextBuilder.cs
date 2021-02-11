using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using amulware.Graphics.Textures;

namespace amulware.Graphics.Pipelines.Context
{
    public sealed class PipelineContextBuilder<TState>
    {
        private readonly List<IContextChange<TState>> contextChanges = new List<IContextChange<TState>>();

        public PipelineContextBuilder<TState> BindRenderTarget(PipelineRenderTarget renderTarget)
            => with(new FramebufferChange<TState>(renderTarget));

        public PipelineContextBuilder<TState> BindRenderTarget(Func<TState, RenderTarget> renderTarget)
            => with(new FramebufferChange<TState>(renderTarget));

        public PipelineContextBuilder<TState> SetCullMode(CullMode cullMode)
            => with(new Culling<TState>(cullMode));

        public PipelineContextBuilder<TState> SetDepthMode(DepthMode depthMode)
            => with(new DepthModeChange<TState>(depthMode));

        public PipelineContextBuilder<TState> SetBlendMode(BlendMode blendMode)
            => with(new BlendModeChange<TState>(blendMode));

        public PipelineContextBuilder<TState> SetScissorRegion(Func<TState, ScissorRegion> getScissorRegion)
            => with(new ScissorRegionChange<TState>(getScissorRegion));

        public PipelineContextBuilder<TState> SetViewport(Func<TState, Rectangle> getViewport)
            => with(new Viewport<TState>(getViewport));

        private PipelineContextBuilder<TState> with(IContextChange<TState> change)
        {
            contextChanges.Add(change);
            return this;
        }

        public ImmutableArray<IContextChange<TState>> Build() => contextChanges.ToImmutableArray();
    }
}
