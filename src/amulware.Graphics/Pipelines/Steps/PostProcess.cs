using amulware.Graphics.PostProcessing;

namespace amulware.Graphics.Pipelines.Steps
{
    sealed class PostProcess : Render
    {
        public PostProcess(PostProcessor postProcessor)
            : base(postProcessor)
        {
        }
    }
}
