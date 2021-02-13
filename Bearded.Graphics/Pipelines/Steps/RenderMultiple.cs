using System;
using System.Collections.Generic;
using Bearded.Graphics.Rendering;

namespace Bearded.Graphics.Pipelines.Steps
{
    sealed class RenderMultiple<TState> : IPipeline<TState>
    {
        private readonly Func<TState, IEnumerable<IRenderer>> getRenderers;

        public RenderMultiple(IEnumerable<IRenderer> renderers)
            : this(_ => renderers)
        {
        }

        public RenderMultiple(Func<TState, IEnumerable<IRenderer>> getRenderers)
        {
            this.getRenderers = getRenderers;
        }

        public void Execute(TState state)
        {
            foreach (var renderer in getRenderers(state))
            {
                renderer.Render();
            }
        }
    }
}
