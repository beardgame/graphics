using amulware.Graphics.Rendering;

namespace amulware.Graphics.Pipelines.Steps
{
    class Render : IPipeline
    {
        private readonly IRenderer renderer;

        public Render(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void Execute()
        {
            renderer.Render();
        }
    }
}
