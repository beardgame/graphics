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
        {
            contextChanges.Add(new FramebufferContextChange<TState>(renderTarget));
            return this;
        }

        public PipelineContextBuilder<TState> BindRenderTarget(Func<TState, RenderTarget> renderTarget)
        {
            contextChanges.Add(new FramebufferContextChange<TState>(renderTarget));
            return this;
        }

        public PipelineContextBuilder<TState> SetDepthMode(DepthMode depthMode)
        {
            contextChanges.Add(new DepthModeChange<TState>(depthMode));
            return this;
        }

        public PipelineContextBuilder<TState> SetBlendMode(BlendMode blendMode)
        {
            contextChanges.Add(new BlendModeChange<TState>(blendMode));
            return this;
        }

        public PipelineContextBuilder<TState> SetViewport(Func<TState, Rectangle> getViewport)
        {
            contextChanges.Add(new Viewport<TState>(getViewport));
            return this;
        }

        public ImmutableArray<IContextChange<TState>> Build() => contextChanges.ToImmutableArray();
    }
}
