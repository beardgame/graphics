using System;

namespace Bearded.Graphics.Pipelines.Steps
{
    sealed class Elevator<TStateInner, TStateOuter> : IPipeline<TStateOuter>
    {
        private readonly IPipeline<TStateInner> inner;
        private readonly Func<TStateOuter, TStateInner> selector;

        public Elevator(IPipeline<TStateInner> inner, Func<TStateOuter, TStateInner> selector)
        {
            this.inner = inner;
            this.selector = selector;
        }

        public void Execute(TStateOuter state)
        {
            inner.Execute(selector(state));
        }
    }
}
