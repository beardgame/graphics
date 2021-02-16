using System;
using Bearded.Graphics.Rendering;

namespace Bearded.Graphics.Pipelines.Steps
{
    class Render<TState> : IPipeline<TState>
    {
        private readonly Func<TState, IRenderer> getRenderer;

        public Render(IRenderer renderer)
            : this(_ => renderer)
        {
        }

        public Render(Func<TState, IRenderer> getRenderer)
        {
            this.getRenderer = getRenderer;
        }

        public void Execute(TState state)
        {
            getRenderer(state).Render();
        }
    }
}
