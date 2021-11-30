using System;
using Bearded.Graphics.Pipelines.Steps;
using Void = Bearded.Utilities.Void;

namespace Bearded.Graphics.Pipelines
{
    public static partial class Pipeline
    {
        public static IPipeline<TState> Then<TState>(this IPipeline<TState> basePipeline, IPipeline<TState> continuation)
        {
            return new Composite<TState>(basePipeline, continuation);
        }

        public static IPipeline<TStateOuter> Elevate<TStateInner, TStateOuter>(
            this IPipeline<TStateInner> innerPipeline, Func<TStateOuter, TStateInner> selector)
        {
            return new Elevator<TStateInner,TStateOuter>(innerPipeline, selector);
        }

        public static void Execute(this IPipeline<Void> pipeline)
        {
            pipeline.Execute(default);
        }
    }
}
