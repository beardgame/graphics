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
            if (inactiveDrawCalls.ContainsKey(batch))
            {
                activeDrawCalls[batch] = inactiveDrawCalls[batch];
                inactiveDrawCalls.Remove(batch);
            }
            else
            {
                activeBatchesWithoutDrawCall.Add(batch);
            }
        }

        private void onBatchDeactivated(IRenderable batch)
        {
            if (activeDrawCalls.ContainsKey(batch))
            {
                inactiveDrawCalls[batch] = activeDrawCalls[batch];
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

                foreach (var (_, drawCall) in activeDrawCalls)
                    drawCall.Invoke();
            }
        }

        private void createMissingDrawCalls()
        {
            foreach (var batch in activeBatchesWithoutDrawCall)
            {
                activeDrawCalls[batch] = DrawCall.For(batch, shaderProgram);
            }
            activeBatchesWithoutDrawCall.Clear();
        }
    }
}
