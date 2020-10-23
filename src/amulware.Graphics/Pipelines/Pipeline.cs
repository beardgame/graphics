using amulware.Graphics.Pipelines.Steps;

namespace amulware.Graphics.Pipelines
{
    public static partial class Pipeline<TState>
    {
        public static IPipeline<TState> Do(System.Action<TState> action)
        {
            return new Action<TState>(action);
        }

        public static IPipeline<TState> InOrder(params IPipeline<TState>[] steps)
        {
            return new Composite<TState>(steps);
        }
    }
}
