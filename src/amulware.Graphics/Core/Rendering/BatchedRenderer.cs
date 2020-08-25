using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using amulware.Graphics.RenderSettings;
using amulware.Graphics.Shading;

namespace amulware.Graphics.Rendering
{
    public sealed class BatchedRenderer
    {
        private readonly IBatchedRenderable renderable;
        private readonly ImmutableArray<IRenderSetting> settings;
        private readonly Dictionary<IRenderable, DrawCall> activeDrawCalls = new Dictionary<IRenderable, DrawCall>();
        private readonly Dictionary<IRenderable, DrawCall> inactiveDrawCalls = new Dictionary<IRenderable, DrawCall>();
        private readonly List<IRenderable> activeBatchesWithoutDrawCall = new List<IRenderable>();

        private ShaderProgram shaderProgram = null!;
        private ImmutableArray<IProgramRenderSetting> settingsForProgram;

        private BatchedRenderer(
            IBatchedRenderable renderable, ShaderProgram shaderProgram, IEnumerable<IRenderSetting> settings)
        {
            this.renderable = renderable;
            this.settings = settings.ToImmutableArray();

            renderable.BatchActivated += onBatchActivated;
            renderable.BatchDeactivated += onBatchDeactivated;

            SetShaderProgram(shaderProgram);
        }

        private void onBatchActivated(IRenderable batch)
        {
            if (inactiveDrawCalls.TryGetValue(batch, out var drawCall))
            {
                activeDrawCalls[batch] = drawCall;
                inactiveDrawCalls.Remove(batch);
            }
            else
            {
                activeBatchesWithoutDrawCall.Add(batch);
            }
        }

        private void onBatchDeactivated(IRenderable batch)
        {
            if (activeDrawCalls.TryGetValue(batch, out var drawCall))
            {
                inactiveDrawCalls[batch] = drawCall;
                activeDrawCalls.Remove(batch);
            }
            else
            {
                activeBatchesWithoutDrawCall.Remove(batch);
            }
        }

        public void SetShaderProgram(ShaderProgram program)
        {
            shaderProgram = program;

            disposeAndClear(activeDrawCalls);
            disposeAndClear(inactiveDrawCalls);

            activeBatchesWithoutDrawCall.Clear();
            activeBatchesWithoutDrawCall.AddRange(renderable.GetActiveBatches());

            settingsForProgram = settings.Select(s => s.ForProgram(program)).ToImmutableArray();
        }

        private void disposeAndClear(Dictionary<IRenderable, DrawCall> drawCalls)
        {
            foreach (var (_, vertexArray) in drawCalls)
            {
                vertexArray.Dispose();
            }

            drawCalls.Clear();
        }

        public void Render()
        {
            createMissingDrawCalls();

            using (shaderProgram.Use())
            {
                foreach (var setting in settingsForProgram)
                {
                    setting.Set();
                }

                // TODO: these are not sorted, which can lead to out of order rendering of batches
                // could be fixed with sorted dictionary(?) or separate list
                foreach (var (_, drawCall) in activeDrawCalls)
                {
                    drawCall.Invoke();
                }
            }
        }

        private void createMissingDrawCalls()
        {
            foreach (var batch in activeBatchesWithoutDrawCall)
            {
                activeDrawCalls[batch] = batch.MakeDrawCallFor(shaderProgram);
            }
            activeBatchesWithoutDrawCall.Clear();
        }
    }
}
