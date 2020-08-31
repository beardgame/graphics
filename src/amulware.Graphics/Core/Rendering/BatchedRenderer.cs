using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using amulware.Graphics.RenderSettings;
using amulware.Graphics.Shading;

namespace amulware.Graphics.Rendering
{
    public sealed class BatchedRenderer : IRenderer, IDisposable
    {
        private readonly IBatchedRenderable renderable;
        private readonly ImmutableArray<IRenderSetting> settings;

        private readonly LinkedList<DrawCall> activeDrawCallsInOrder = new LinkedList<DrawCall>();
        private readonly Dictionary<IRenderable, LinkedListNode<DrawCall>> activeDrawCalls = new Dictionary<IRenderable, LinkedListNode<DrawCall>>();
        private readonly Dictionary<IRenderable, LinkedListNode<DrawCall>> inactiveDrawCalls = new Dictionary<IRenderable, LinkedListNode<DrawCall>>();
        private readonly List<IRenderable> batchesWaitingForActivation = new List<IRenderable>();

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
            batchesWaitingForActivation.Add(batch);
        }

        private void onBatchDeactivated(IRenderable batch)
        {
            if (activeDrawCalls.TryGetValue(batch, out var drawCall))
            {
                activeDrawCallsInOrder.Remove(drawCall);
                inactiveDrawCalls[batch] = drawCall;
                activeDrawCalls.Remove(batch);
            }
            else
            {
                batchesWaitingForActivation.Remove(batch);
            }
        }

        public void SetShaderProgram(ShaderProgram program)
        {
            shaderProgram = program;

            activeDrawCallsInOrder.Clear();
            disposeAndClear(activeDrawCalls);
            disposeAndClear(inactiveDrawCalls);

            batchesWaitingForActivation.Clear();
            batchesWaitingForActivation.AddRange(renderable.GetActiveBatches());

            settingsForProgram = settings.Select(s => s.ForProgram(program)).ToImmutableArray();
        }

        public void Render()
        {
            activateQueuedDrawCalls();

            using (shaderProgram.Use())
            {
                foreach (var setting in settingsForProgram)
                {
                    setting.Set();
                }

                foreach (var drawCall in activeDrawCallsInOrder)
                {
                    drawCall.Invoke();
                }
            }
        }

        private void activateQueuedDrawCalls()
        {
            foreach (var batch in batchesWaitingForActivation)
            {
                if (inactiveDrawCalls.TryGetValue(batch, out var node))
                {
                    inactiveDrawCalls.Remove(batch);
                }
                else
                {
                    var drawCall = batch.MakeDrawCallFor(shaderProgram);
                    node = new LinkedListNode<DrawCall>(drawCall);
                }
                activeDrawCalls[batch] = node;
                activeDrawCallsInOrder.AddLast(node);
            }
            batchesWaitingForActivation.Clear();
        }

        public void Dispose()
        {
            disposeAndClear(activeDrawCalls);
            disposeAndClear(inactiveDrawCalls);
        }

        private void disposeAndClear(Dictionary<IRenderable, LinkedListNode<DrawCall>> drawCalls)
        {
            foreach (var (_, drawCall) in drawCalls)
            {
                drawCall.Value.Dispose();
            }

            drawCalls.Clear();
        }
    }
}
