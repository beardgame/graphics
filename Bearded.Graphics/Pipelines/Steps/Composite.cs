using System.Collections.Generic;
using System.Collections.Immutable;

namespace Bearded.Graphics.Pipelines.Steps
{
    sealed class Composite<TState> : IPipeline<TState>
    {
        private bool flattened;
        private ImmutableArray<IPipeline<TState>> steps;

        public Composite(params IPipeline<TState>[] steps)
        {
            this.steps = steps.ToImmutableArray();
        }

        public void Execute(TState state)
        {
            if (!flattened)
                flatten();

            foreach (var step in steps)
            {
                step.Execute(state);
            }
        }

        private void flatten()
        {
            var builder = ImmutableArray.CreateBuilder<IPipeline<TState>>(steps.Length);

            builder.AddRange(enumerateSteps());

            steps = builder.Count == steps.Length ? builder.MoveToImmutable() : builder.ToImmutable();
            flattened = true;
        }

        private IEnumerable<IPipeline<TState>> enumerateSteps()
        {
            foreach (var step in steps)
            {
                if (step is Composite<TState> composite)
                {
                    foreach (var s in composite.enumerateSteps())
                    {
                        yield return s;
                    }
                }
                else
                {
                    yield return step;
                }
            }
        }
    }
}
