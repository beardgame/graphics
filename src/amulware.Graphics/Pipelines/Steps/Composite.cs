using System.Collections.Generic;
using System.Collections.Immutable;

namespace amulware.Graphics.Pipelines.Steps
{
    sealed class Composite : IPipeline
    {
        private bool flattened;
        private ImmutableArray<IPipeline> steps;

        public Composite(params IPipeline[] steps)
        {
            this.steps = steps.ToImmutableArray();
        }

        public void Execute()
        {
            if (!flattened)
                flatten();

            foreach (var step in steps)
            {
                step.Execute();
            }
        }

        private void flatten()
        {
            var builder = ImmutableArray.CreateBuilder<IPipeline>(steps.Length);

            builder.AddRange(enumerateSteps());

            steps = builder.MoveToImmutable();
            flattened = true;
        }

        private IEnumerable<IPipeline> enumerateSteps()
        {
            foreach (var step in steps)
            {
                if (step is Composite composite)
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
