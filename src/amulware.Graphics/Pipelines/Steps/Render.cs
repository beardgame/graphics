using amulware.Graphics.Rendering;

namespace amulware.Graphics.Pipelines.Steps
{
    class Render<TState> : IPipeline<TState>
    {
        private readonly IRenderer renderer;

        public Render(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void Execute(TState state)
        {
            renderer.Render();
        }
    }
}
