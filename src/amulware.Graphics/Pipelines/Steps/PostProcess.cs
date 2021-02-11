using amulware.Graphics.PostProcessing;

namespace amulware.Graphics.Pipelines.Steps
{
    sealed class PostProcess<TState> : Render<TState>
    {
        public PostProcess(PostProcessor postProcessor)
            : base(postProcessor)
        {
        }
    }
}
