using amulware.Graphics.Pipelines.Steps;

namespace amulware.Graphics.Pipelines
{
    public static partial class Pipeline
    {
        public static IPipeline Do(System.Action action)
        {
            return new Action(action);
        }

        public static IPipeline Then(this IPipeline basePipeline, IPipeline continuation)
        {
            return new Composite(basePipeline, continuation);
        }

        public static IPipeline InOrder(params IPipeline[] steps)
        {
            return new Composite(steps);
        }
    }
}
