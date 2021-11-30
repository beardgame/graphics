using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using Bearded.Graphics.Debugging;
using Bearded.Graphics.Textures;

namespace Bearded.Graphics.Pipelines.Context
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

        public PipelineContextBuilder<TState> SetColorMask(ColorMask mask)
            => with(new ColorMask<TState>(mask));

        public PipelineContextBuilder<TState> SetDepthMode(DepthMode depthMode)
            => with(new DepthModeChange<TState>(depthMode));

        public PipelineContextBuilder<TState> SetBlendMode(BlendMode blendMode)
            => with(new BlendModeChange<TState>(blendMode));

        public PipelineContextBuilder<TState> SetScissorRegion(Func<TState, ScissorRegion> getScissorRegion)
            => with(new ScissorRegionChange<TState>(getScissorRegion));

        public PipelineContextBuilder<TState> SetViewport(Func<TState, Rectangle> getViewport)
            => with(new Viewport<TState>(getViewport));

        public PipelineContextBuilder<TState> SetDebugName(string name)
            => with(new DebugGroup<TState>(name));

        public PipelineContextBuilder<TState> SetDebugName(Func<TState, string> getName)
            => with(new DebugGroup<TState>(getName));

        private PipelineContextBuilder<TState> with(IContextChange<TState> change)
        {
            contextChanges.Add(change);
            return this;
        }

        public ImmutableArray<IContextChange<TState>> Build() => contextChanges.ToImmutableArray();
    }
}
