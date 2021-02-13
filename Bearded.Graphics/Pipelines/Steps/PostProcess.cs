using Bearded.Graphics.PostProcessing;

namespace Bearded.Graphics.Pipelines.Steps
{
    sealed class PostProcess<TState> : Render<TState>
    {
        public PostProcess(PostProcessor postProcessor)
            : base(postProcessor)
        {
        }
    }
}
