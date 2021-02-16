using Bearded.Graphics.Pipelines.Steps;

namespace Bearded.Graphics.Pipelines
{
    public static partial class Pipeline<TState>
    {
        public static IPipeline<TState> Do(System.Action<TState> action)
        {
            return new Action<TState>(action);
        }

        public static IPipeline<TState> Do(System.Action action)
        {
            return new Action<TState>(_ => action());
        }

        public static IPipeline<TState> InOrder(params IPipeline<TState>[] steps)
        {
            return new Composite<TState>(steps);
        }
    }
}
